using ChecksumCorrector.Core;

namespace UnitTests.Models
{
    public record struct TestFile
    {
        public required string Path { get; init; }
        public required ChecksumAlgorithm ChecksumAlgorithm { get; init; }
        public required byte[] СhecksumInFile {  get; init; }
        public required byte[] СorrectСhecksum { get; init; }
    }
}
