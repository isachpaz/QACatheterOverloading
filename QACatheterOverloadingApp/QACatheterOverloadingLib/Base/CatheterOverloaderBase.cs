
using System;
using System.Collections.Generic;
using EvilDICOM.Core;
using EvilDICOM.Core.Element;
using EvilDICOM.Core.IO.Writing;
using EvilDICOM.Core.Selection;
using QACatheterOverloadingLib.Interfaces;
using QACatheterOverloadingLib.Results;

namespace QACatheterOverloadingLib.Base
{
    internal abstract class CatheterOverloaderBase : ICatheterOverloaderBase
    {
        private readonly ICatheterOverloadingParameters _configuration;
        private readonly DICOMObject _patientPlan;
        private readonly DICOMObject _verificationPlan;
        
        private ResultCollection _outputPlans = new ResultCollection();

        private IDicomServices DicomServices { get; }
        private ICatheterBlockService CatheterBlockService { get; }
        public bool AreResultsValid { get; private set; }

        protected CatheterOverloaderBase(ICatheterOverloadingParameters configuration, 
                                         IDicomServices dicomServices, 
                                         ICatheterBlockService catheterBlockService)
        {
            AreResultsValid = false;

            DicomServices = dicomServices ?? throw  new ArgumentException(nameof(dicomServices));
            CatheterBlockService = catheterBlockService ?? throw new ArgumentException(nameof(catheterBlockService));

            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
            _patientPlan = _configuration.PatientPlan ??
                                throw new ArgumentException(nameof(_configuration.PatientPlan));
            _verificationPlan = _configuration.VerificationPlan ??
                                     throw new ArgumentException(nameof(_configuration.VerificationPlan));
        }

        public void RunOverloading()
        {
            if (_configuration == null) return;
            if (!HasConfigurationBrachyPlans(_configuration)) return;

            try
            {
                CreateAllVerificationPlans();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            //string filewoExtension = GetFileWithoutExtension(outputPlan);
            //verificationPlan.Write($"{filewoExtension}-{block.ToString()}.dcm", DICOMIOSettings.DefaultExplicit());
            //Console.WriteLine($"\nOutput plan created: {outputPlan}");

            AreResultsValid = true;
        }

        private void CreateAllVerificationPlans()
        {
            _outputPlans = new ResultCollection();

            //Generate blocks and related plans
            var blocks = CatheterBlockService.CreateBlocks(DicomServices.GetTotalNumberOfChannelsInPlan(_patientPlan));
            
            foreach (IBlockItem blockItem in blocks)
            {
                var newVerificationPlan = DicomServices.DeepCopy(_verificationPlan);
                CreateNewVerificationPlan(blockItem, ref newVerificationPlan);
                _outputPlans.Add(ResultItem.CreateResultItem(blockItem.ToString(), newVerificationPlan));
            }
        }

        private bool HasConfigurationBrachyPlans(ICatheterOverloadingParameters configuration)
        {
            bool isPatienPlanBrachy = DicomServices.IsBrachyPlan(configuration.PatientPlan);
            bool isVerificationPlanBrachy = DicomServices.IsBrachyPlan(configuration.VerificationPlan);
            return isPatienPlanBrachy && isVerificationPlanBrachy;
        }


        private void CreateNewVerificationPlan(IBlockItem blockItem, ref DICOMObject newVerificationPlan)
        {
            CopyTreatmentMachineSequence(_patientPlan, newVerificationPlan);
            CopyFractionGroupSeq(_patientPlan, newVerificationPlan);
            CopySourceSequence(_patientPlan, newVerificationPlan);

            CopyApplicationSetupSequence(_patientPlan, newVerificationPlan, blockItem.Begin, blockItem.End);

            if (this.IsPrivateTagsNeededToBeTransfered())
            {
                CopyPrivateTags(_patientPlan, newVerificationPlan);
            }

            UpdateVerificationPlanLabel(_patientPlan, newVerificationPlan);
        }
        
        protected virtual bool IsPrivateTagsNeededToBeTransfered()
        {
            return false;
        }
        
        protected  virtual void CopyTreatmentMachineSequence(DICOMObject fromPatientPlan, DICOMObject toVerificationPlan)
        {
            var patientSelector = new DICOMSelector(fromPatientPlan);
            var verificationSelector = new DICOMSelector(toVerificationPlan);

            var machineSeq = patientSelector.TreatmentMachineSequence;
            verificationSelector.TreatmentMachineSequence = machineSeq;

        }
        protected virtual void CopyFractionGroupSeq(DICOMObject patientPlan, DICOMObject verificationPlan)
        {
            var patientSelector = new DICOMSelector(patientPlan);
            var verificationSelector = new DICOMSelector(verificationPlan);

            var fractionSeq = patientSelector.FractionGroupSequence;
            verificationSelector.FractionGroupSequence = fractionSeq;
        }

        protected virtual void CopySourceSequence(DICOMObject patientPlan, DICOMObject verificationPlan)
        {
            var patientSelector = new DICOMSelector(patientPlan);
            var verificationSelector = new DICOMSelector(verificationPlan);

            var patientLabel = patientSelector.RTPlanLabel;
            verificationSelector.RTPlanLabel.Data += " - " + patientLabel.Data;
            verificationSelector.RTPlanName.Data = verificationSelector.RTPlanLabel.Data;
        }

        protected virtual void CopyApplicationSetupSequence(DICOMObject patientPlan,
                                                    DICOMObject verificationPlan,
                                                    int fromChannelNumber,
                                                    int toChannelNumber)
        {
            int numberOfChannelsInValidationPlan = toChannelNumber - fromChannelNumber;
            var patientSelector = new DICOMSelector(patientPlan);
            var verificationSelector = new DICOMSelector(verificationPlan);

            var totalReferenceAirKerma = patientSelector.TotalReferenceAirKerma;
            verificationSelector.TotalReferenceAirKerma = totalReferenceAirKerma;

            var channelSeq = patientSelector.ChannelSequence;
            int channelId = 1;
            foreach (var item in channelSeq.Data_)
            {
                var itemSelector = new DICOMSelector(item);
                var channelNumber = itemSelector.ChannelNumber;

                if (channelNumber != null)
                {
                    bool bInsideTheBlock = IsInsideTheChannelBlock(channelNumber.Data,
                        fromChannelNumber,
                        toChannelNumber);
                    if (!bInsideTheBlock)
                        continue;

                    var numberOfControlPoints = itemSelector.NumberOfControlPoints;
                    var channelLength = itemSelector.ChannelLength;
                    var channelTotalTime = itemSelector.ChannelTotalTime;
                    var sourceApplicatorLength = itemSelector.SourceApplicatorLength;
                    var sourceApplicatorStep = itemSelector.SourceApplicatorStepSize;
                    var finalCumulativeTimeweight = itemSelector.FinalCumulativeTimeWeight;
                    var brachyControlPointSeq = itemSelector.BrachyControlPointSequence;

                    UpdateTarget(verificationSelector, channelNumber, numberOfControlPoints, channelLength,
                        channelTotalTime, sourceApplicatorLength, sourceApplicatorStep,
                        finalCumulativeTimeweight, brachyControlPointSeq, channelId);
                    ++channelId;
                }
            }
        }

        protected abstract void CopyPrivateTags(DICOMObject patientPlan, DICOMObject verificationPlan);


        protected virtual void UpdateVerificationPlanLabel(DICOMObject patientPlan, DICOMObject verificationPlan)
        {
            var patientSelector = new DICOMSelector(patientPlan);
            var verificationSelector = new DICOMSelector(verificationPlan);

            var patientLabel = patientSelector.RTPlanLabel;
            verificationSelector.RTPlanLabel.Data += " - " + patientLabel.Data;
            verificationSelector.RTPlanName.Data = verificationSelector.RTPlanLabel.Data;
        }
        
        private bool IsInsideTheChannelBlock(int channelNumber, int fromChannelNumber, int toChannelNumber)
        {
            if ((channelNumber >= fromChannelNumber) &&
                (channelNumber <= toChannelNumber))
            {
                return true;
            }
            return false;
        }

        protected virtual void UpdateTarget(DICOMSelector verificationSelector, IntegerString channelNumber, IntegerString numberOfControlPoints, DecimalString channelLength, DecimalString channelTotalTime, DecimalString sourceApplicatorLength, DecimalString sourceApplicatorStep, DecimalString finalCumulativeTimeweight, Sequence brachyControlPointSeq, int channelId)
        {
            var channelSeq = verificationSelector.ChannelSequence;
            foreach (var item in channelSeq.Data_)
            {
                var itemSelector = new DICOMSelector(item);
                var cn = itemSelector.ChannelNumber.Data;
                if (cn != channelId)
                    continue;
                itemSelector.NumberOfControlPoints = numberOfControlPoints;
                //itemSelector.ChannelLength;
                itemSelector.ChannelTotalTime = channelTotalTime;
                itemSelector.SourceApplicatorLength = sourceApplicatorLength;
                itemSelector.SourceApplicatorStepSize = sourceApplicatorStep;
                itemSelector.FinalCumulativeTimeWeight = finalCumulativeTimeweight;
                itemSelector.BrachyControlPointSequence = brachyControlPointSeq;
            }
        }
        
        public ICollection<IResultItem> GetOverloadedPlans()
        {
            return this._outputPlans;
        }
        public void SaveToFile(string outputfileDcm)
        {
            foreach (IResultItem item in _outputPlans)
            {
                try
                {
                    string fileName = outputfileDcm + "_" + item.FileName + ".dcm";
                    DicomServices.Save(item.VerificationPlan, DICOMIOSettings.DefaultExplicit(), fileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}