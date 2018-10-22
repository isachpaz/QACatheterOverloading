namespace QACatheterOverloadingApp
{
    public class InputParameters : IInputParameters
    {
        public string PatientPlanFileName { get; set; }
        public string VerificationPlanFileName { get; set; }
        public bool IsValid { get; set;  }
        public string OutputPlanName { get; set; }
        public string TpsType { get; set; }
        public int NumberOfChannelsInVerificationPlan { get; set; }

        public InputParameters()
        {
            IsValid = true;
        }
    }
}