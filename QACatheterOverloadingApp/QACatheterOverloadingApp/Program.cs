using System;
using Plossum.CommandLine;
using QACatheterOverloadingApp.Adapters;
using QACatheterOverloadingLib.CatheterOverload;
using QACatheterOverloadingLib.CatheterPartition;
using QACatheterOverloadingLib.Dicom;
using QACatheterOverloadingLib.Interfaces;
using QACatheterOverloadingLib.Services;

namespace QACatheterOverloadingApp
{
    class Program
    {
        static int Main(string[] args)
        {
            CmdLineManager cmdLineManager = new CmdLineManager(new Options());
            cmdLineManager.ShowTitle();
            cmdLineManager.CheckInput();
            IInputParameters input = cmdLineManager.GetInput();
            
            if (!input.IsValid)
            {
                return -1;
            }
            else
            {
                Process(input);
            }
            return 0;
        }

        private static void Process(IInputParameters input)
        {            
            IDicomServices dicomServices = new DicomServices();
            ICatheterBlocks catheterBlocks = new CatheterBlocks(input.NumberOfChannelsInVerificationPlan);
            ICatheterBlockService catheterBlockService = new CatheterBlockService(catheterBlocks);

            var catheterOverloader =
                CatheterOverloaderFactory.Create(new CatheterOverloadingParametersAdapter(input, dicomServices),
                    dicomServices,
                    catheterBlockService);

            catheterOverloader.RunOverloading();
            catheterOverloader.SaveToFile("outputFile.dcm");
        }

        private static bool AreCmdLineInputParametersInPlace()
        {
            Options options = new Options();
            CommandLineParser parser = new CommandLineParser(options);
            parser.Parse();
            Console.WriteLine(parser.UsageInfo.GetHeaderAsString(100));

            if (options.Help)
            {
                Console.WriteLine(parser.UsageInfo.GetOptionsAsString(100));
                return false;
            }
            else if (parser.HasErrors)
            {
                Console.WriteLine(parser.UsageInfo.GetErrorsAsString(100));
                return false;
            }

            if (!string.IsNullOrEmpty(options.patientPlan))
            {
                Console.WriteLine($"Patient plan is: {options.patientPlan}");
            }

            if (!string.IsNullOrEmpty(options.verificationPlan))
            {
                Console.WriteLine($"Verification plan is: {options.verificationPlan}");
            }
            return true;
        }
    }
}
