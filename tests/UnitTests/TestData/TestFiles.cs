using ChecksumCorrector.Core;
using UnitTests.Models;
using UnitTests.TestData;

namespace UnitTests.Consts
{
    public static class Test
    {
        public static readonly IDictionary<TestFileTypes, TestFile> Files = new Dictionary<TestFileTypes, TestFile>();
        
        static Test()
        {
            Files = GetTestFiles();
        }
        
        private static Dictionary<TestFileTypes, TestFile> GetTestFiles()
        {
            var files = new Dictionary<TestFileTypes, TestFile>();

            files.Add(TestFileTypes.SumOfBigEndian16BitNotOrig, new TestFile() {
                Path = "TestFiles\\SumOfBigEndian16Bit_orig",
                ChecksumAlgorithm = ChecksumAlgorithm.SumOfBigEndian16BitNot,
                СhecksumInFile = [71, 193],
                СorrectСhecksum = [71, 193]
            });

            files.Add(TestFileTypes.SumOfBigEndian16BitNotEdit, new TestFile() {
                Path = "TestFiles\\SumOfBigEndian16Bit_edit",
                ChecksumAlgorithm = ChecksumAlgorithm.SumOfBigEndian16BitNot,
                СhecksumInFile = [71, 193],
                СorrectСhecksum = [71, 185]
            });

            files.Add(TestFileTypes.SumOfBigEndian16BitNotPlusOneOrig, new TestFile() {
                Path = "TestFiles\\SumOfBigEndian16BitPlusOne_orig",
                ChecksumAlgorithm = ChecksumAlgorithm.SumOfBigEndian16BitNotPlusOne,
                СhecksumInFile = [159, 164],
                СorrectСhecksum = [159, 164]
            });

            files.Add(TestFileTypes.SumOfBigEndian16BitNotPlusOneEdit, new TestFile() {
                Path = "TestFiles\\SumOfBigEndian16BitPlusOne_edit",
                ChecksumAlgorithm = ChecksumAlgorithm.SumOfBigEndian16BitNotPlusOne,
                СhecksumInFile = [105, 28],
                СorrectСhecksum = [169, 164]
            });

            return files;
        }
    }
}
