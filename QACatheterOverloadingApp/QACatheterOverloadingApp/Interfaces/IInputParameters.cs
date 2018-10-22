namespace QACatheterOverloadingApp
{
    public interface IInputParameters
    {
        int NumberOfChannelsInVerificationPlan { get; set; }
        string PatientPlanFileName { get; set; }
        string VerificationPlanFileName { get; set; }
        bool IsValid { get; set; }
        string OutputPlanName { get; set; }
        string TpsType { get; set; }
    }
}