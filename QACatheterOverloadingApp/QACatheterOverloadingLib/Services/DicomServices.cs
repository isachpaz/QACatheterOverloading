using System;
using System.IO;
using EvilDICOM.Core;
using EvilDICOM.Core.IO.Writing;
using EvilDICOM.Core.Selection;
using QACatheterOverloadingLib.Interfaces;

namespace QACatheterOverloadingLib.Services
{
    public class DicomServices : IDicomServices
    {
        public DICOMObject CreateDicomObjectFromFile(string file)
        {
            try
            {
                if (!string.IsNullOrEmpty(file))
                {
                    return DICOMObject.Read(file);
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public void Save(DICOMObject dcm, DICOMIOSettings settings, string toFile)
        {
            try
            {
                EvilDICOM.Core.IO.Writing.DICOMFileWriter.Write(toFile, settings, dcm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool IsBrachyPlan(DICOMObject plan)
        {
            int nChannels = GetTotalNumberOfChannelsInPlan(plan);
            if (nChannels > 0)
            {
                return true;
            }
            return false;
        }

        public int GetTotalNumberOfChannelsInPlan(DICOMObject plan)
        {
            try
            {
                var patientSelector = new DICOMSelector(plan);
                var channelSeq = patientSelector.ChannelSequence;
                if (channelSeq != null)
                {
                    var numberOfChannels = channelSeq.Items.Count;
                    return numberOfChannels;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public DICOMObject DeepCopy(DICOMObject dcm)
        {
            DICOMObject copy = null;
            using (var ms = new MemoryStream())
            {
                DICOMFileWriter.Write(ms, DICOMIOSettings.DefaultExplicit(), dcm);
                var data = ms.ToArray();
                copy = DICOMObject.Read(data);
            }
            return copy;
        }
       }
}