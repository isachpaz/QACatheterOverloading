using System.Collections.Generic;

namespace QACatheterOverloadingLib.Interfaces
{
    public interface ICatheterBlocks
    {
        int NumberOfTotalChannelsInPatientPlan { get; }

        ICollection<IBlockItem> CreateBlocks(int numberOfChannels);
        int GetNumberOfBlocks(int numberOfChannels);
    }
}