using FluentAssertions;
using Moq;
using MyDiet_API.Business.IRepository;
using MyDiet_API.Services;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using MyDiet_API.Shared.Models;
using MyDiet_API.UnitTests.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyDiet_API.UnitTests.Services
{
    public class DietServiceTests : IClassFixture<DietFixture>
    {
        private readonly IDietService _sut;
        private readonly Mock<IDietRepository> _mockRepository;
        private readonly DietFixture _fixture;

        public DietServiceTests(DietFixture fixture)
        {
            _mockRepository = new Mock<IDietRepository>();
            _sut = new DietService(_mockRepository.Object);
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsAllItems()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(_fixture.Items);

            // Act
            var dietResult = await _sut.GetAllAsync();

            // Assert
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
            dietResult.Should().NotBeNullOrEmpty();
            dietResult.Should().BeEquivalentTo(_fixture.Items);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsDiet()
        {
            // Arrange
            var dietDto = _fixture.Items.FirstOrDefault();
            var dietId = dietDto.Id;
            _mockRepository.Setup(r => r.GetAsync(dietId)).ReturnsAsync(dietDto);

            // Act
            var dietResult = await _sut.GetAsync(dietId);

            // Assert
            _mockRepository.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Once);
            dietResult.Should().NotBeNull();
            dietResult.Should().BeEquivalentTo(dietDto);
        }

        [Fact]
        public async Task Get_WithNotExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var dietResult = await _sut.GetAsync(It.IsAny<int>());

            // Assert
            _mockRepository.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Once);
            dietResult.Should().BeNull();
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsDiet()
        {
            // Arrange
            var dietDto = new DietDto
            {
                Name = "Diet 1",
                Description = "Diet 1"
            };
            var expectedDietDto = new DietDto
            {
                Id = 1,
                Name = dietDto.Name,
                Description = dietDto.Description
            };
            _mockRepository.Setup(r => r.CreateAsync(dietDto)).ReturnsAsync(expectedDietDto);

            // Act
            var dietResult = await _sut.CreateAsync(dietDto);

            // Assert
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<DietDto>()), Times.Once);
            dietResult.Should().NotBeNull();
            dietResult.Should().BeEquivalentTo(expectedDietDto);
        }

        [Fact]
        public async Task Update_WhenCalled_ReturnsDiet()
        {
            // Arrange
            var dietDto = _fixture.Items.FirstOrDefault();
            var dietId = dietDto.Id;
            var expectedDietDto = new DietDto
            {
                Id = dietDto.Id,
                Name = "Diet 11",
                Description = dietDto.Description
            };
            _mockRepository.Setup(r => r.UpdateAsync(dietId, dietDto)).ReturnsAsync(expectedDietDto);

            // Act
            var dietResult = await _sut.UpdateAsync(dietId, dietDto);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<DietDto>()), Times.Once);
            dietResult.Should().NotBeNull();
            dietResult.Should().BeEquivalentTo(expectedDietDto);
        }

        [Fact]
        public async Task Update_WithNotExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<DietDto>())).ReturnsAsync(() => null);

            // Act
            var dietResult = await _sut.UpdateAsync(It.IsAny<int>(), It.IsAny<DietDto>());

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<DietDto>()), Times.Once);
            dietResult.Should().BeNull();
        }

        [Fact]
        public async Task Delete_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var dietId = _fixture.Items.FirstOrDefault().Id;
            _mockRepository.Setup(r => r.DeleteAsync(dietId)).ReturnsAsync(true);

            // Act
            var dietResult = await _sut.DeleteAsync(dietId);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
            dietResult.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_WithNotExistingId_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var dietResult = await _sut.DeleteAsync(It.IsAny<int>());

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
            dietResult.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllDietMeals_WhenCalled_ReturnsAllDietMeals()
        {
            // Arrange
            var dietDto = _fixture.Items.FirstOrDefault();
            var expectedDietDto = new DietDto()
            {
                Id = dietDto.Id,
                Name = dietDto.Name,
                Description = dietDto.Description,
                DietMeal = new List<DietMeal>
                {
                    new DietMeal
                    {
                        DietId = dietDto.Id,
                        MealId = 1
                    }
                }
            };
            _mockRepository.Setup(r => r.GetAllDietMealsAsync(dietDto.Id)).ReturnsAsync(expectedDietDto);

            // Act
            var dietResult = await _sut.GetAllDietMeals(dietDto.Id);

            // Assert
            _mockRepository.Verify(r => r.GetAllDietMealsAsync(It.IsAny<int>()), Times.Once);
            dietResult.Should().NotBeNull();
            dietResult.Should().BeEquivalentTo(expectedDietDto);
        }
    }
}
