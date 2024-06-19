using Xunit;
using Moq;
using JediApi.Repositories;
using JediApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JediApi.Models;

namespace JediApi.Tests.Services
{
    public class JediServiceTests
    {
        private readonly JediService _service;
        private readonly Mock<IJediRepository> _repositoryMock;

        public JediServiceTests()
        {
            _repositoryMock = new Mock<IJediRepository>();
            _service = new JediService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetById_Success()
        {
            // Arrange
            var jediId = 1;
            var expectedJedi = new Jedi { Id = jediId, Name = "Luke Skywalker" };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(jediId)).ReturnsAsync(expectedJedi);

            // Act
            var result = await _service.GetByIdAsync(jediId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedJedi.Id, result.Id);
            Assert.Equal(expectedJedi.Name, result.Name);
        }

        [Fact]
        public async Task GetById_NotFound()
        {
            // Arrange
            var jediId = 1;
            _repositoryMock.Setup(repo => repo.GetByIdAsync(jediId)).ReturnsAsync((Jedi)null);

            // Act
            var result = await _service.GetByIdAsync(jediId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAll()
        {
            // Arrange
            var expectedJedis = new List<Jedi>
            {
                new Jedi { Id = 1, Name = "Luke Skywalker" },
                new Jedi { Id = 2, Name = "Obi-Wan Kenobi" }
            };
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedJedis);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedJedis.Count, result.Count());
            Assert.Contains(result, jedi => jedi.Name == "Luke Skywalker");
            Assert.Contains(result, jedi => jedi.Name == "Obi-Wan Kenobi");
        }
    }
}