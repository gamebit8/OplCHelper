using ChecksumCorrector.Core;
using UnitTests.Consts;


namespace UnitTests
{
    public class ChecksumService_GetChecksumAlgorithmFromOriginalFileAsync : IClassFixture<ServicesFixture>
    {
        private static ChecksumService checksumService;
        public ChecksumService_GetChecksumAlgorithmFromOriginalFileAsync(ServicesFixture servicesFixture)
        {
            checksumService = servicesFixture.ChecksumService;
        }

        [Theory]
        [MemberData(nameof(GetData), DisableDiscoveryEnumeration = true)]
        public async void ChecksumService_GetChecksumAlgorithmFromOriginalFileAsync_InTestFilesWithAlgorithm_ReturnEqual(string filePath, ChecksumAlgorithm expect)
        {
            var result = await checksumService.GetChecksumAlgorithmFromOriginalFileAsync(filePath);

            Assert.Equal(expect, result);
        }


        public static IEnumerable<object[]> GetData()
        {
            var testData = new List<object[]>();

            testData.Add([Test.Files[TestData.TestFileTypes.SumOfBigEndian16BitNotOrig].Path, ChecksumAlgorithm.SumOfBigEndian16BitNot]);
            testData.Add([Test.Files[TestData.TestFileTypes.SumOfBigEndian16BitNotPlusOneOrig].Path, ChecksumAlgorithm.SumOfBigEndian16BitNotPlusOne]);

            return testData;
        }
    }
}
