using EvilDICOM.Core;

namespace QACatheterOverloadingLib.Results
{
    public interface IResultItem
    {
        string FileName { get; }
        DICOMObject VerificationPlan { get; }
        string ToString();
    }
}