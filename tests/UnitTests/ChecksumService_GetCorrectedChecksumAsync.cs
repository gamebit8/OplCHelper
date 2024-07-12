using ChecksumCorrector.Core;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Consts;
using UnitTests.Models;

namespace UnitTests
{
    public class ChecksumService_GetCorrectedChecksumAsync : IClassFixture<ServicesFixture>
    {
        private static ChecksumService checksumService;
        public ChecksumService_GetCorrectedChecksumAsync(ServicesFixture servicesFixture)
        {
            checksumService = servicesFixture.ChecksumService;
        }

        [Theory]
        [MemberData(nameof(GetDataForInTestFilesReturnEqual))]
        public async void ChecksumService_GetCorrectedChecksumAsync_InTestFilesWithCorrectChecksumInFile_ReturnEqual(string filePath, byte[] expect)
        {
            var result = await checksumService.GetCorrectedChecksumAsync(filePath);

            Assert.Equal(expect, result);
        }

        [Theory]
        [MemberData(nameof(GetDataForInTestFilesReturnNotEqual))]
        public async void ChecksumService_GetCorrectedChecksumAsync_InTestFilesWithNotCorrectChecksumInFile_ReturnNotEqual(string filePath, byte[] expect)
        {
            var result = await checksumService.GetCorrectedChecksumAsync(filePath);

            Assert.NotEqual(expect, result);
        }

        public static IEnumerable<object[]> GetDataForInTestFilesReturnEqual()
        {
            return GetData(f => Enumerable.SequenceEqual(f.СhecksumInFile, f.СorrectСhecksum));
        }

        public static IEnumerable<object[]> GetDataForInTestFilesReturnNotEqual()
        {
            return GetData(f => !Enumerable.SequenceEqual(f.СhecksumInFile, f.СorrectСhecksum));
        }

        public static IEnumerable<object[]> GetData(Func<TestFile, bool> predicate)
        {
            var testData = new List<object[]>();

            foreach (var file in Test.Files.Values)
            {
                if (predicate(file))
                {
                    testData.Add([file.Path, file.СhecksumInFile]);
                }
            }

            return testData;
        }
    }
}
