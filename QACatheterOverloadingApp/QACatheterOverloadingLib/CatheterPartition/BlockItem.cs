using QACatheterOverloadingLib.Interfaces;

namespace QACatheterOverloadingLib.CatheterPartition
{
    public class BlockItem : IBlockItem
    {
        public int Begin { get; }
        public  int End { get; }

        private BlockItem(int begin, int end)
        {
            Begin = begin;
            End = end;
        }

        public static IBlockItem CreateGivenBeginEnd(int begin, int end)
        {
            return new BlockItem(begin, end);
        }

        public override string ToString()
        {
            return $"Catheters_{Begin}_to_{End}";
        }
    }
}