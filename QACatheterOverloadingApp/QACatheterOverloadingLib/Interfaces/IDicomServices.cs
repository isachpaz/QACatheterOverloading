
using EvilDICOM.Core;
using EvilDICOM.Core.IO.Writing;

namespace QACatheterOverloadingLib.Interfaces
{
    public interface IDicomServices
    {
        DICOMObject CreateDicomObjectFromFile(string file);
        void Save(DICOMObject dcm, DICOMIOSettings settings, string toFile);
        bool IsBrachyPlan(DICOMObject plan);
        int GetTotalNumberOfChannelsInPlan(DICOMObject plan);
        DICOMObject DeepCopy(DICOMObject dcm);
    }
}