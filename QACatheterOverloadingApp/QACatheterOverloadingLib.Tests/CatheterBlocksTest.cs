using System;
using System.Linq;
using NUnit.Framework;
using QACatheterOverloadingLib.CatheterPartition;

namespace QACatheterOverloadingLib.Tests
{
    [TestFixture]
    public class CatheterBlocksTest
    {
        [Test]
        public void Catheter_Number_Of_Blocks_1_Test()
        {
            int blockSize = 8;
            CatheterBlocks cb = new CatheterBlocks(blockSize);
            var numberOfBlocks = cb.GetNumberOfBlocks(8);
            Assert.AreEqual(1, numberOfBlocks);
        }

        [Test]
        public void Catheter_Number_Of_Blocks_2_Test()
        {
            int blockSize = 8;
            CatheterBlocks cb = new CatheterBlocks(blockSize);
            var numberOfBlocks = cb.GetNumberOfBlocks(7);
            Assert.AreEqual(1, numberOfBlocks);
        }

        [Test]
        public void Catheter_Number_Of_Blocks_3_Test()
        {
            int blockSize = 8;
            CatheterBlocks cb = new CatheterBlocks(blockSize);
            var numberOfBlocks = cb.GetNumberOfBlocks(10);
            Assert.AreEqual(2, numberOfBlocks);
        }

        [Test]
        public void Catheter_Number_Of_Blocks_4_Test()
        {
            int blockSize = 8;
            CatheterBlocks cb = new CatheterBlocks(blockSize);
            var numberOfBlocks = cb.GetNumberOfBlocks(16);
            Assert.AreEqual(2, numberOfBlocks);
        }

        [Test]
        public void Catheter_Number_Of_Blocks_5_Test()
        {
            int blockSize = 8;
            CatheterBlocks cb = new CatheterBlocks(blockSize);
            var numberOfBlocks = cb.GetNumberOfBlocks(20);
            Assert.AreEqual(3, numberOfBlocks);
        }


        [Test]
        public void Catheter_Number_Of_Blocks_6_Test()
        {
            int blockSize = 10;
            CatheterBlocks cb = new CatheterBlocks(blockSize);
            var numberOfBlocks = cb.GetNumberOfBlocks(19);
            Assert.AreEqual(2, numberOfBlocks);
        }

        [Test]
        public void Catheter_Number_Of_Blocks_7_Test()
        {
            int blockSize = 1;
            CatheterBlocks cb = new CatheterBlocks(blockSize);
            var numberOfBlocks = cb.GetNumberOfBlocks(19);
            Assert.AreEqual(19, numberOfBlocks);
        }

        [Test]
        public void Catheter_Number_Of_Blocks_Less_Than_Zero_Test()
        {
            int blockSize = 0;
            Assert.Throws<ArgumentException>(() => new CatheterBlocks(blockSize));
        }

        [Test]
        public void Catheter_Number_Of_Blocks_More_Than_100_Test()
        {
            int blockSize = 101;
            Assert.Throws<ArgumentException>(() => new CatheterBlocks(blockSize));
        }
        
        [Test]
        public void Catheter_Creat_Block_1_Test()
        {
            int blockSize = 8;
            CatheterBlocks cb = new CatheterBlocks(blockSize);
            var blocks = cb.CreateBlocks(16);
            Assert.AreEqual(2, blocks.Count);
            Assert.AreEqual(1, blocks.ElementAt(0).Begin);
            Assert.AreEqual(8, blocks.ElementAt(0).End);
            Assert.AreEqual(9, blocks.ElementAt(1).Begin);
            Assert.AreEqual(16, blocks.ElementAt(1).End);
        }

        [Test]
        public void Catheter_Creat_Block_2_Test()
        {
            int blockSize = 8;
            CatheterBlocks cb = new CatheterBlocks(blockSize);
            var blocks = cb.CreateBlocks(15);
            Assert.AreEqual(2, blocks.Count);
            Assert.AreEqual(1, blocks.ElementAt(0).Begin);
            Assert.AreEqual(8, blocks.ElementAt(0).End);
            Assert.AreEqual(9, blocks.ElementAt(1).Begin);
            Assert.AreEqual(16, blocks.ElementAt(1).End);
        }

        [Test]
        public void Catheter_Creat_Block_3_Test()
        {
            int blockSize = 8;
            CatheterBlocks cb = new CatheterBlocks(blockSize);
            var blocks = cb.CreateBlocks(24);
            Assert.AreEqual(3, blocks.Count);
            Assert.AreEqual(1, blocks.ElementAt(0).Begin);
            Assert.AreEqual(8, blocks.ElementAt(0).End);
            Assert.AreEqual(9, blocks.ElementAt(1).Begin);
            Assert.AreEqual(16, blocks.ElementAt(1).End);
            Assert.AreEqual(17, blocks.ElementAt(2).Begin);
            Assert.AreEqual(24, blocks.ElementAt(2).End);
        }
    }
}
