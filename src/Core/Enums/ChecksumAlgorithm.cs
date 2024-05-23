namespace ChecksumCorrector.Core
{
    public enum ChecksumAlgorithm
    {
        None = 0,
        SumOfBigEndian16Bit,
        SumOfBigEndian16BitPlusOne,
    }
}
