using System;
using EvilDICOM.Core;
using QACatheterOverloadingLib.Enum;
using QACatheterOverloadingLib.Interfaces;

namespace QACatheterOverloadingApp.Adapters
{
    public class CatheterOverloadingParametersAdapter : ICatheterOverloadingParameters
    {
        private DICOMObject _patientPlan;
        private DICOMObject _verificationPlan;
        private IInputParameters Input { get; set; }
        private IDicomServices DicomServices { get; }

        public CatheterOverloadingParametersAdapter(IInputParameters input, 
                                                    IDicomServices dicomServices)
        {
            Input = input ?? throw new ArgumentException("IInputParameters cannot be null");
            DicomServices = dicomServices ?? throw new ArgumentNullException(nameof(dicomServices));
        }
        
        public int NumberOfChannelsInVerificationPlan  => Input.NumberOfChannelsInVerificationPlan;
        public DICOMObject PatientPlan => _patientPlan ?? (_patientPlan =
                                              DicomServices.CreateDicomObjectFromFile(Input.PatientPlanFileName));
        public DICOMObject VerificationPlan => _verificationPlan ?? (_verificationPlan =
                                                   DicomServices.CreateDicomObjectFromFile(Input.VerificationPlanFileName));
        public string OutputPlanName => Input.OutputPlanName;
        public PlanningSystemType TpsType => GetTpsType();

        private PlanningSystemType GetTpsType()
        {
            var results = Int32.TryParse(Input.TpsType, out int code);
            if (results)
            {
                switch (code)
                {
                    case 1:
                        return PlanningSystemType.ONCENTRA_BRACHY;
                    case 2:
                        return PlanningSystemType.ONCENTRA_PROSTATE;
                    case 3:
                        return PlanningSystemType.VARIAN_BRACHY;
                }
            }
            return PlanningSystemType.UNKNOWN;
        }
    }
}