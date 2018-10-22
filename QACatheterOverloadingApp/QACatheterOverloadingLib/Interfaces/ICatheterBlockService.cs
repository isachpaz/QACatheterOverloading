using System.Collections.Generic;

namespace QACatheterOverloadingLib.Interfaces
{
    public interface ICatheterBlockService
    {
        ICollection<IBlockItem> CreateBlocks(int numberOfChannels);
        int GetNumberOfBlocks(int numberOfChannels);
    }
}