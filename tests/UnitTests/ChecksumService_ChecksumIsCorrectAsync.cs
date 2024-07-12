using ChecksumCorrector.Core;
using UnitTests.Consts;
using UnitTests.Models;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace UnitTests
{
    public class ChecksumService_ChecksumIsCorrectAsync : IClassFixture<ServicesFixture>
    {
        private static ChecksumService checksumService;
        public ChecksumService_ChecksumIsCorrectAsync(ServicesFixture servicesFixture)
        {
            checksumService = servicesFixture.ChecksumService;
        }

        [Theory]
        [MemberData(nameof(GetDataForInTestFilesWithInvalidChecksumInFileAndCreateFileWithCorrectChecksum))]
        public async void ChecksumService_ChecksumIsCorrectAsync_InTestFilesWithInvalidChecksumInFileAndCreateFileWithCorrectChecksum_ReturnEqualChangedAndCreateNewFile(string filePath, ChecksumAlgorithm checksumAlgorithm, byte[] expect)
        {
            var result = new byte[2];
            var modifiedFilePath = GetModifiedFilePath(filePath);

            var checksumStatus = await checksumService.ChecksumIsCorrectAsync(filePath, checksumAlgorithm, true);
            var newFileIsCreate = File.Exists(modifiedFilePath);

            using (var fs = File.Open(modifiedFilePath, FileMode.Open))
            {
                await fs.ReadAsync(result);
            }

            Assert.Equal(expect, result);
            Assert.Equal(Checksum.Сhanged, checksumStatus);
            Assert.True(newFileIsCreate);

            File.Delete(modifiedFilePath);
        }


        [Theory]
        [MemberData(nameof(GetDataForInTestFilesWithValidChecksumInFileAndNotCreateFileWithCorrectChecksum))]
        public async void ChecksumService_ChecksumIsCorrectAsync_InTestFilesWithValidChecksumInFile_ReturnEqualValidAndNotCreateNewFile(string filePath, ChecksumAlgorithm checksumAlgorithm)
        {
            var modifiedFilePath = GetModifiedFilePath(filePath);

            var checksumStatus = await checksumService.ChecksumIsCorrectAsync(filePath, checksumAlgorithm ,true);
            var newFileIsCreate = File.Exists(modifiedFilePath);

            Assert.Equal(Checksum.Valid, checksumStatus);
            Assert.False(newFileIsCreate);
        }

        [Theory]
        [MemberData(nameof(GetDataForInTestFilesWithCorrectChecksumInFileReturnEqualValid))]
        public async void ChecksumService_ChecksumIsCorrectAsync_InTestFilesWithCorrectChecksumInFile_ReturnEqualValid(string filePath, ChecksumAlgorithm checksumAlgorithm, Checksum expect)
        {
            var result = await checksumService.ChecksumIsCorrectAsync(filePath, checksumAlgorithm);

            Assert.Equal(expect, result);
        }

        [Theory]
        [MemberData(nameof(GetDataForInTestFilesWithInvalidChecksumInFileReturnEqualInvalid))]
        public async void ChecksumService_ChecksumIsCorrectAsync_InTestFilesWithInvalidChecksumInFile_ReturnEqualInvalid(string filePath, ChecksumAlgorithm checksumAlgorithm, Checksum expect)
        {
            var result = await checksumService.ChecksumIsCorrectAsync(filePath, checksumAlgorithm);

            Assert.Equal(expect, result);
        }

        private static string GetModifiedFilePath(string originalFilePath)
        {
            return originalFilePath + "_mod";
        }

        public static IEnumerable<object[]> GetDataForInTestFilesWithInvalidChecksumInFileAndCreateFileWithCorrectChecksum()
        {
            var testData = new List<object[]>();

            foreach (var file in Test.Files.Values)
            {
                if (!Enumerable.SequenceEqual(file.СhecksumInFile, file.СorrectСhecksum))
                {
                    testData.Add([file.Path, file.ChecksumAlgorithm, file.СorrectСhecksum]);
                }
            }

            AddTestDataWithUnknownChecksumAlgorithm(ref testData);

            return testData;
        }

        public static IEnumerable<object[]> GetDataForInTestFilesWithValidChecksumInFileAndNotCreateFileWithCorrectChecksum()
        {
            var testData = new List<object[]>();

            foreach (var file in Test.Files.Values)
            {
                if (Enumerable.SequenceEqual(file.СhecksumInFile, file.СorrectСhecksum))
                {
                    testData.Add([file.Path, file.ChecksumAlgorithm]);
                }
            }

            AddTestDataWithUnknownChecksumAlgorithm(ref testData);

            return testData;
        }
        
        private static void AddTestDataWithUnknownChecksumAlgorithm(ref List<object[]> testData)
        {
            if(testData.Count != 0 && testData[0][1] is ChecksumAlgorithm)
            {
                var testDataTiwhUnknowChecksumAlgorithm = testData[0].Clone() as object[];
                testDataTiwhUnknowChecksumAlgorithm[1] = ChecksumAlgorithm.Unknown;
                testData.Add(testDataTiwhUnknowChecksumAlgorithm);
            }
        }

        public static IEnumerable<object[]> GetDataForInTestFilesWithCorrectChecksumInFileReturnEqualValid()
        {
            return GetData(f => Enumerable.SequenceEqual(f.СhecksumInFile, f.СorrectСhecksum), Checksum.Valid);
        }

        public static IEnumerable<object[]> GetDataForInTestFilesWithInvalidChecksumInFileReturnEqualInvalid()
        {
            return GetData(f => !Enumerable.SequenceEqual(f.СhecksumInFile, f.СorrectСhecksum), Checksum.Invalid);
        }

        public static IEnumerable<object[]> GetData(Func<TestFile, bool> predicate, Checksum expectedChecksumStatus)
        {
            var testData = new List<object[]>();

            foreach (var file in Test.Files.Values)
            {
                if (predicate(file))
                {
                    testData.Add([file.Path, file.ChecksumAlgorithm, expectedChecksumStatus]);
                }
            }

            AddTestDataWithUnknownChecksumAlgorithm(ref testData);

            return testData;
        }
    }
}
