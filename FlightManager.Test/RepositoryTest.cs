using System.Threading.Tasks;
using FlightManager.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FlightManager.Test
{

    [TestClass]
    public class RepositoryTest
    {

        Mock<IConfiguration> _mockConfiguration;
        Mock<IConfigurationSection> _mockSection;

        Mock<ILogger<FlightRepository>> _mockFlightRepositoryLogger;
         
        readonly string _connectionString = "Server=localhost;Database=FlightManager;User ID=sa;Password=MyPassword;";

        [TestInitialize]
        public void Setup()
        {
            _mockSection = new Mock<IConfigurationSection>();
            _mockConfiguration = new Mock<IConfiguration>();

            // Mock the ConnectionStrings section
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            mockConnectionStringsSection.Setup(s => s["DefaultConnection"])
                .Returns(_connectionString);

            // Mock GetSection("ConnectionStrings") to return the connection strings section
            _mockConfiguration.Setup(c => c.GetSection("ConnectionStrings"))
                .Returns(mockConnectionStringsSection.Object);


            // Configure mock to return a valid connection string
            _mockSection.Setup(s => s.Value).Returns(_connectionString);
            _mockConfiguration.Setup(c => c.GetSection("ConnectionStrings:DefaultConnection")).Returns(_mockSection.Object);


            _mockFlightRepositoryLogger = new Mock<ILogger<FlightRepository>>();
        }

        [TestMethod]
        public async Task GetFlights_ReturnsNotNull()
        {
            // Arrange
            var repository = new FlightRepository(_mockConfiguration.Object, _mockFlightRepositoryLogger.Object);

            // Act
            var result = await repository.GetFlightsAsync();

            // Assert
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public async Task GetFlights_ReturnsNotEmpty()
        {
            // Arrange
            var repository = new FlightRepository(_mockConfiguration.Object, _mockFlightRepositoryLogger.Object);

            // Act
            var result = await repository.GetFlightsAsync();

            // Assert
            Assert.IsTrue(result.Data.Count > 0);
        }
    }
}

