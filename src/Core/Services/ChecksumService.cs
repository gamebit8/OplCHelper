using System.Linq;

namespace ChecksumCorrector.Core
{
    public class ChecksumService : IChecksumService
    {
        private readonly ILogger<ChecksumService> _logger;
        private const uint _checksumLengthBytes = 2;
        private const uint _readBufferLengthInBytes = 1024;

        public ChecksumService(ILogger<ChecksumService> logger) => _logger = logger;

        public async Task<byte[]> GetOriginalChecksumAsync(string path)
        {
            var checksum = new byte[_checksumLengthBytes];

            using FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            await fs.ReadAsync(checksum);

            return checksum;
        }

        public async Task<byte[]> GetCorrectedChecksumAsync(string path)
        {
            byte[] checksum = new byte[2];

            var algorithm = await TryGetChecksumAlgorithmFromModifiedFileAsync(path);

            checksum = algorithm == ChecksumAlgorithm.SumOfBigEndian16BitNot
                ? await GetChecksumSumOfBigEndian16BitAlgorithmAsync(path)
                : await GetChecksumSumOfBigEndian16BitPlusOneAsync(path);

            return checksum;
        }

        public async Task<ChecksumAlgorithm> GetChecksumAlgorithmFromOriginalFileAsync(string path)
        {
            if (!IsCalibrationFile(path))
                return ChecksumAlgorithm.Unknown;

            var sum = await GetSumOfBigEndianAsync(path, 0);

            var sumInHex = Convert.ToString(sum, 16);
            var lastTwoByteChecksum = sumInHex.Substring(sumInHex.Length - (int)(4));

            return lastTwoByteChecksum switch
            {
                "ffff" => ChecksumAlgorithm.SumOfBigEndian16BitNot,
                "0000" => ChecksumAlgorithm.SumOfBigEndian16BitNotPlusOne,
                _ => ChecksumAlgorithm.Unknown
            };
        }

        private bool IsCalibrationFile(string path)
        {
            var fileExtension = Path.GetExtension(path);

            if (fileExtension == "" || fileExtension == "bin")
                return true;

            return false;
        }

        private async Task<ChecksumAlgorithm> TryGetChecksumAlgorithmFromModifiedFileAsync(string path)
        {
            var lengthBuffer = 1;

            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var buffer = new byte[lengthBuffer];
            fs.Seek(-lengthBuffer, SeekOrigin.End);
            await fs.ReadAsync(buffer);

            if (buffer[0] == 255)
                return ChecksumAlgorithm.SumOfBigEndian16BitNot;

            if (buffer[0] == 0) 
                return ChecksumAlgorithm.SumOfBigEndian16BitNotPlusOne;

            return ChecksumAlgorithm.Unknown;
        }

        public async Task<byte[]> GetChecksumSumOfBigEndian16BitAlgorithmAsync(string path)
        {
            var sum = await GetSumOfBigEndianAsync(path, _checksumLengthBytes);
            return GetChecksumSumOfBigEndian16BitAlgorithFromSum(sum);
        }

        public async Task<byte[]> GetChecksumSumOfBigEndian16BitPlusOneAsync(string path)
        {
            var sum = await GetSumOfBigEndianAsync(path, _checksumLengthBytes);
            return GetChecksumSumOfBigEndian16BitAlgorithPlusOneFromSum(sum);
        }

        private async Task<uint> GetSumOfBigEndianAsync(string path, long offset = 0)
        {
            uint sum = 0;
            var buffer = new byte[_readBufferLengthInBytes];

            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Seek(offset, SeekOrigin.Begin);

                while (await fs.ReadAsync(buffer) > 0)
                {
                    sum += GetSumOfBigEndian(buffer);
                    Array.Clear(buffer);
                }
            }

            return sum;
        }

        private static uint GetSumOfBigEndian(byte[] data)
        {
            uint sum = 0;

            for (int i = 0; i < data.Length; i += 2)
            {
                var bigEndianPair = new byte[] { data[i + 1], data[i] };
                sum += BitConverter.ToUInt16(bigEndianPair);
            }

            return sum;
        }

        private static byte[] GetChecksumSumOfBigEndian16BitAlgorithFromSum(uint sum)
        {
            byte[] tempChecksum = new byte[_checksumLengthBytes];

            var sumInHex = Convert.ToString(sum, 16);
            var lastTwoByteChecksum = sumInHex.Substring(sumInHex.Length - (int)(_checksumLengthBytes * 2));
            var lastTwoByteChecksumInUint = Convert.ToUInt16(lastTwoByteChecksum, 16);

            uint checksum = 65535 ^ (uint)lastTwoByteChecksumInUint;

            var checksumInByteArray = BitConverter.GetBytes(checksum);

            tempChecksum[0] = checksumInByteArray[1];
            tempChecksum[1] = checksumInByteArray[0];

            return tempChecksum;
        }

        private static byte[] GetChecksumSumOfBigEndian16BitAlgorithPlusOneFromSum(uint sum)
        {
            byte[] tempChecksum = new byte[_checksumLengthBytes];

            var checksumInHex = Convert.ToString(sum, 16);
            var lastTwoByteChecksum = checksumInHex.Substring(checksumInHex.Length - (int)(_checksumLengthBytes * 2));
            var lastByteChecksumAfterXor = ~Convert.ToUInt16(lastTwoByteChecksum, 16);
            var lastByteChecksumAfterXorInByteArray = BitConverter.GetBytes(lastByteChecksumAfterXor + 1);

            tempChecksum[0] = lastByteChecksumAfterXorInByteArray[1];
            tempChecksum[1] = lastByteChecksumAfterXorInByteArray[0];

            return tempChecksum;
        }

        public async Task<Checksum> ChecksumIsCorrectAsync(string path, ChecksumAlgorithm checksumAlgorithm = ChecksumAlgorithm.Unknown, bool createAFileWithTheCorrectChecksum = false)
        {
            if (!IsCalibrationFile(path))
                return Checksum.Unknown;

            byte[] originalChecksum = await GetOriginalChecksumAsync(path);
            byte[] correctedChecksum = checksumAlgorithm switch
                {
                    ChecksumAlgorithm.SumOfBigEndian16BitNot => await GetChecksumSumOfBigEndian16BitAlgorithmAsync(path),
                    ChecksumAlgorithm.SumOfBigEndian16BitNotPlusOne => await GetChecksumSumOfBigEndian16BitPlusOneAsync(path),
                    _ => await GetCorrectedChecksumAsync(path)
                };
           
            if (!Enumerable.SequenceEqual(originalChecksum, correctedChecksum) && createAFileWithTheCorrectChecksum)
            {
                await CreateFileWithCorrectChecksumAsync(path, correctedChecksum);
                return Checksum.Сhanged;
            }

            if(!Enumerable.SequenceEqual(originalChecksum, correctedChecksum) && !createAFileWithTheCorrectChecksum)
            {
                return Checksum.Invalid;
            }

            return Checksum.Valid;
        }

        private static async Task CreateFileWithCorrectChecksumAsync(string originalFilePath, byte[] correctChecksum)
        {
            var newPath = originalFilePath + "_mod";

            if (File.Exists(originalFilePath))
            {
                File.Copy(originalFilePath, newPath, true);
                using var fs = new FileStream(newPath, FileMode.OpenOrCreate);
                await fs.WriteAsync(correctChecksum);
            }
        }
    }
}
