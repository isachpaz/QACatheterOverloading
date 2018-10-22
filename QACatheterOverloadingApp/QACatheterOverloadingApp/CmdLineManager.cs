using System;
using Plossum.CommandLine;

namespace QACatheterOverloadingApp
{
    public class CmdLineManager
    {
        public Options Options { get; protected set; }
        public CommandLineParser Parser { get; protected set; }

        public CmdLineManager(Options inOptions)
        {
            Options = inOptions;
            Parser = new CommandLineParser(Options);
        }

        public void ShowTitle()
        {
            Console.WriteLine(Parser.UsageInfo.GetHeaderAsString(100));
        }


        public void CheckInput()
        {
            Parser.Parse();
            if (Options.Help)
            {
                Console.WriteLine(Parser.UsageInfo.GetOptionsAsString(100));
            }
            else if (Parser.HasErrors)
            {
                Console.WriteLine(Parser.UsageInfo.GetErrorsAsString(100));
            }
        }

        public IInputParameters GetInput()
        {
            IInputParameters inputParameters = new InputParameters();

            //////////////////////////////////////////////////////////////////////
            if (!string.IsNullOrEmpty(Options.patientPlan))
            {
                inputParameters.PatientPlanFileName = Options.patientPlan;
            }
            else
            {
                inputParameters.IsValid = false;
            }

            //////////////////////////////////////////////////////////////////////
            if (!string.IsNullOrEmpty(Options.verificationPlan))
            {
                inputParameters.VerificationPlanFileName = Options.verificationPlan;
            }
            else
            {
                inputParameters.IsValid = false;
            }

            //////////////////////////////////////////////////////////////////////
            if (!string.IsNullOrEmpty(Options.outputPlan))
            {
                inputParameters.OutputPlanName = Options.outputPlan;
            }
            else
            {
                inputParameters.IsValid = false;
            }

            //////////////////////////////////////////////////////////////////////
            var result = Int32.TryParse(Options.numberOfChannelsInVerificationPlan, out int nChannels);
            if (result)
            {
                inputParameters.NumberOfChannelsInVerificationPlan = nChannels;
            }
            else
            {
                Console.WriteLine(
                    "NumberOfChannels cannot be retrieved from the user input. Set to default value = 8 channels.");
                inputParameters.NumberOfChannelsInVerificationPlan = 8;
            }
            //////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////
            if (!string.IsNullOrEmpty(Options.tpsType))
            {
                inputParameters.TpsType = Options.tpsType;
            }
            else
            {
                inputParameters.IsValid = false;
            }
            return inputParameters;
        }
    }
}