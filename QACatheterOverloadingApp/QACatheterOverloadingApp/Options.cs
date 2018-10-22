using System;
using Plossum.CommandLine;

namespace QACatheterOverloadingApp
{
    [CommandLineManager(ApplicationName = "************************************************\n" +
                                           "*                                              *\n" +
                                           "*               OverloadingDicomTags           *\n" +
                                           "*                                              *\n" +
                                           "************************************************\n",
         Copyright = "Author: Dr.-Ing. Ilias Sachpazidis\n" +
                     "Email:<ilias.sachpazidis@uniklinik-freiburg.de>\n" +
                     "************************************************\n")]
    public class Options 
    {
        public Options()
        {
            _mNumberOfChannelsInVerificationPlan = "8";
        }

        [CommandLineOption(Description = "Displays this help text")]
        public bool Help = false;

        [CommandLineOption(Description = "Specifies the patient DICOM plan file (input)",
            MinOccurs = 1)]
        public string patientPlan
        {
            get { return _mPatientPlan; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new InvalidOptionValueException(
                        "The name must not be empty", false);
                _mPatientPlan = value;
            }
        }

        [CommandLineOption(Description = "Specifies the dummy verification DICOM plan file (input)",
            MinOccurs = 1)]
        public string verificationPlan
        {
            get { return _mVerificationPlan; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new InvalidOptionValueException(
                        "The name must not be empty", false);
                _mVerificationPlan = value;
            }
        }

        [CommandLineOption(Description = "Specifies the merged verification DICOM plan file (output)",
            MinOccurs = 1)]
        public string outputPlan
        {
            get { return _mOutputPlan; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new InvalidOptionValueException(
                        "The name must not be empty", false);
                _mOutputPlan = value;
            }
        }

        [CommandLineOption(Description = "Number of channels in the verification plan",
            MinOccurs = 1)]
        public string numberOfChannelsInVerificationPlan
        {
            get { return _mNumberOfChannelsInVerificationPlan; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new InvalidOptionValueException(
                        "Number of channels in the verification plan", false);
                _mNumberOfChannelsInVerificationPlan = value;
            }
        }

        [CommandLineOption(Description = "Treatment planning system should be given: [1] OncentraBrachy, [2] OncentraProstate, [3] VarianBrachy",
            MinOccurs = 1)]
        public string tpsType
        {
            get => _tpsType;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new InvalidOptionValueException(
                        "Treatment planning system should be given: [1] OncentraBrachy, [2] OncentraProstate, [3] VarianBrachy", false);

                _tpsType = value;
            }
        }


        private string _mPatientPlan;
        private string _mVerificationPlan;
        private string _mOutputPlan;
        private string _mNumberOfChannelsInVerificationPlan;
        private string _tpsType;
    }
}