using EvilDICOM.Core;
using QACatheterOverloadingLib.Enum;

namespace QACatheterOverloadingLib.Interfaces
{
    public interface ICatheterOverloadingParameters
    {
        int NumberOfChannelsInVerificationPlan { get;  }
        DICOMObject PatientPlan { get; }
        DICOMObject VerificationPlan{ get;  }
        string OutputPlanName { get; }
        PlanningSystemType TpsType { get; }
    }
}