using ChecksumCorrector.Core;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Consts;

namespace UnitTests
{
    public class ChecksumService_GetOriginalChecksumAsync : IClassFixture<ServicesFixture>
    {
        private static ChecksumService checksumService;
        public ChecksumService_GetOriginalChecksumAsync(ServicesFixture servicesFixture)
        {
            checksumService = servicesFixture.ChecksumService;
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public async void ChecksumService_GetOriginalChecksumAsync_InTestFiles_ReturnEqual(string filePath, byte[] expect)
        {
            var result = await checksumService.GetOriginalChecksumAsync(filePath);

            Assert.Equal(expect, result);
        }

        public static IEnumerable<object[]> GetData()
        {
            var testData = new List<object[]>();    

            foreach (var file in Test.Files.Values)
            {
                testData.Add([file.Path, file.ÑhecksumInFile]);
            }

            return testData;
        }
    }
}