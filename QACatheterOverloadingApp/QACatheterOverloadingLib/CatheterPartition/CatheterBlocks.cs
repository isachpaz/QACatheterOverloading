using System;
using System.Collections.Generic;
using QACatheterOverloadingLib.Interfaces;

namespace QACatheterOverloadingLib.CatheterPartition
{
    public class CatheterBlocks : ICatheterBlocks
    {
        public int NumberOfTotalChannelsInPatientPlan { get; protected set; }
        private int BlockSizeInCatheters { get; }

        public CatheterBlocks(int blockSizeInCatheters)
        {
            SanityCheck(blockSizeInCatheters);
            BlockSizeInCatheters = blockSizeInCatheters;
        }

        private void SanityCheck(int blockSizeInCatheters)
        {
            if ((blockSizeInCatheters <= 0) || 
                (blockSizeInCatheters > 100))
            {
                throw new ArgumentException("Block size must be between [1..100]");
            }
        }

        public int GetNumberOfBlocks(int numberOfChannels)
        {
            int numbeOfBlocks = numberOfChannels / BlockSizeInCatheters;
            if (numberOfChannels - (numbeOfBlocks * BlockSizeInCatheters) > 0.0)
            {
                ++numbeOfBlocks;
            }
            return numbeOfBlocks;
        }


        public ICollection<IBlockItem> CreateBlocks(int numberOfChannels)
        {
            var numberOfBlocks = GetNumberOfBlocks(numberOfChannels);
            ICollection<IBlockItem> blocks = new List<IBlockItem>();

            for (int i = 0; i < numberOfBlocks; ++i)
            {
                blocks.Add(BlockItem.CreateGivenBeginEnd(i*BlockSizeInCatheters + 1, (i+1)*BlockSizeInCatheters));
                
            }
            return blocks;
        }
    }
}