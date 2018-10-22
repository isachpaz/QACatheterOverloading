
using System;
using EvilDICOM.Core;

namespace QACatheterOverloadingLib.Results
{
    public class ResultItem : IResultItem
    {
        public DICOMObject VerificationPlan { get; }        
        public string FileName { get; }

        private ResultItem(string fileName, DICOMObject verificationPlan)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            VerificationPlan = verificationPlan ?? throw new ArgumentNullException(nameof(verificationPlan));
        }

        public static IResultItem CreateResultItem(string fileName, DICOMObject verificationPlan)
        {
            return new ResultItem(fileName, verificationPlan);
        }
        

        public override string ToString()
        {
            return $"{nameof(VerificationPlan)}: {VerificationPlan}, {nameof(FileName)}: {FileName}";
        }
    }
}