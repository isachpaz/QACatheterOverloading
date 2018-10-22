
using System.Collections.Generic;
using QACatheterOverloadingLib.Interfaces;

namespace QACatheterOverloadingLib.Dicom
{
    public class CatheterBlockService : ICatheterBlockService
    {
        private readonly ICatheterBlocks _catheterBlocks;
        public CatheterBlockService(ICatheterBlocks catheterBlocks)
        {
            _catheterBlocks = catheterBlocks;
        }

        public ICollection<IBlockItem> CreateBlocks(int numberOfChannels)
        {
            return this._catheterBlocks.CreateBlocks(numberOfChannels);
        }
        public int GetNumberOfBlocks(int numberOfChannels)
        {
            return this._catheterBlocks.GetNumberOfBlocks(numberOfChannels);
        }
    }
}