namespace ChecksumCorrector.Core
{
    public interface IChecksumService
    {
        Task<byte[]> GetOriginalChecksumAsync(string path);
        Task<byte[]> GetCorrectedChecksumAsync(string path);
        Task<Checksum> ChecksumIsCorrectAsync(string path, ChecksumAlgorithm checksumAlgorithm, bool createAFileWithTheCorrectChecksum);
        Task<ChecksumAlgorithm> GetChecksumAlgorithmFromOriginalFileAsync(string path);
    }
}