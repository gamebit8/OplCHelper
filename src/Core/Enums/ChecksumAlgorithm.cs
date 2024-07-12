namespace ChecksumCorrector.Core
{
    public enum ChecksumAlgorithm
    {
        Unknown = 0,
        SumOfBigEndian16BitNot,
        SumOfBigEndian16BitNotPlusOne,
    }
}
