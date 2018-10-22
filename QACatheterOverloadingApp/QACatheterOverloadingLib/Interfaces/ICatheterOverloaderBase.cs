using System.Collections.Generic;
using QACatheterOverloadingLib.Results;

namespace QACatheterOverloadingLib.Interfaces
{
    public interface ICatheterOverloaderBase
    {
        ICollection<IResultItem> GetOverloadedPlans(); 
        void RunOverloading();
        void SaveToFile(string outputfileDcm);
        bool AreResultsValid { get;}
    }
}