using ChecksumCorrector.Core;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    public class ServicesFixture
    {     
        public ServicesFixture()
        {
            var mockILogger = new Mock<ILogger<ChecksumService>>();
            ChecksumService = new ChecksumService(mockILogger.Object);
        }
    
        public ChecksumService ChecksumService { get; private set; }
    }   
}
