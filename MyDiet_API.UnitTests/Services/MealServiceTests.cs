using FluentAssertions;
using Moq;
using MyDiet_API.Business.IRepository;
using MyDiet_API.Services;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using MyDiet_API.UnitTests.Fixture;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyDiet_API.UnitTests.Services
{
    public class MealServiceTests : IClassFixture<MealFixture>
    {
        private IMealService _sut;
        private Mock<IMealRepository> _mockRepository;
        private MealFixture _fixture;

        public MealServiceTests(MealFixture fixture)
        {
            _mockRepository = new Mock<IMealRepository>();
            _sut = new MealService(_mockRepository.Object);
            _fixture = fixture;
        }

        #region Meal
        [Fact]
        public async Task GetAll_WhenCalled_ReturnsAllItems()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(_fixture.Items);

            // Act
            var mealResult = await _sut.GetAllAsync();

            // Assert
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
            mealResult.Should().NotBeNullOrEmpty();
            mealResult.Should().BeEquivalentTo(_fixture.Items);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsMeal()
        {
            // Arrange
            var meal = _fixture.Items.FirstOrDefault();
            var mealId = meal.Id;
            _mockRepository.Setup(r => r.GetAsync(mealId)).ReturnsAsync(meal);

            // Act
            var mealResult = await _sut.GetAsync(mealId);

            // Assert
            _mockRepository.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Once);
            mealResult.Should().NotBeNull();
            mealResult.Should().BeEquivalentTo(meal);
        }

        [Fact]
        public async Task Get_WithNotExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var mealResult = await _sut.GetAsync(It.IsAny<int>());

            // Assert
            _mockRepository.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Once);
            mealResult.Should().BeNull();
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsMeal()
        {
            // Arrange
            var mealDto = new MealDto
            {
                Name = "MealDto 1"
            };
            var expectedMealDto = new MealDto
            {
                Id = 1,
                Name = "MealDto 1"
            };
            _mockRepository.Setup(r => r.CreateAsync(mealDto)).ReturnsAsync(expectedMealDto);

            // Act
            var mealResult = await _sut.CreateAsync(mealDto);

            // Assert
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<MealDto>()), Times.Once);
            mealResult.Should().NotBeNull();
            mealResult.Should().BeEquivalentTo(expectedMealDto);
        }

        [Fact]
        public async Task Update_WhenCalled_ReturnsMeal()
        {
            // Arrange
            var mealDto = _fixture.Items.FirstOrDefault();
            var expectedMealDto = new MealDto
            {
                Id = mealDto.Id,
                Name = "MealDto 11"
            };
            _mockRepository.Setup(r => r.UpdateAsync(mealDto.Id, mealDto)).ReturnsAsync(expectedMealDto);

            // Act
            var mealResult = await _sut.UpdateAsync(mealDto.Id, mealDto);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<MealDto>()), Times.Once);
            mealResult.Should().NotBeNull();
            mealResult.Should().BeEquivalentTo(expectedMealDto);
        }

        [Fact]
        public async Task Update_WithNotExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<MealDto>())).ReturnsAsync(() => null);

            // Act
            var mealResult = await _sut.UpdateAsync(It.IsAny<int>(), It.IsAny<MealDto>());

            // Assert
            mealResult.Should().BeNull();
        }

        [Fact]
        public async Task Delete_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var mealId = _fixture.Items.FirstOrDefault().Id;
            _mockRepository.Setup(r => r.DeleteAsync(mealId)).ReturnsAsync(true);

            // Act
            var mealResult = await _sut.DeleteAsync(mealId);

            // Assert
            mealResult.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_WithNotExistingId_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var mealResult = await _sut.DeleteAsync(It.IsAny<int>());

            // Assert
            mealResult.Should().BeFalse();
        }

        #endregion

        #region MealDiet
        [Fact]
        public async Task AddMealToDiet_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var mealId = _fixture.Items.FirstOrDefault().Id;
            var dietId = 1;
            _mockRepository.Setup(r => r.AddMealToDietAsync(dietId, mealId)).ReturnsAsync(true);

            // Act
            var mealResult = await _sut.AddMealToDiet(dietId, mealId);

            // Assert
            mealResult.Should().BeTrue();
        }

        [Fact]
        public async Task AddMealToDiet_WithNotExistingIds_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.AddMealToDietAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var mealResult = await _sut.AddMealToDiet(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            mealResult.Should().BeFalse();
        }

        [Fact]
        public async Task RemoveMealFromDiet_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var mealId = _fixture.Items.FirstOrDefault().Id;
            var dietId = 1;
            _mockRepository.Setup(r => r.RemoveMealFromDietAsync(dietId, mealId)).ReturnsAsync(true);

            // Act
            var mealResult = await _sut.RemoveMealFromDiet(dietId, mealId);

            // Assert
            mealResult.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveMealFromDiet_WithNotExistingIds_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.RemoveMealFromDietAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var mealResult = await _sut.RemoveMealFromDiet(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            mealResult.Should().BeFalse();
        }

        [Fact]
        public async Task AddProductToMeal_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var mealId = _fixture.Items.FirstOrDefault().Id;
            var productId = 1;
            _mockRepository.Setup(r => r.AddProductToMealAsync(mealId, productId)).ReturnsAsync(true);

            // Act
            var mealResult = await _sut.AddProductToMeal(mealId, productId);

            // Assert
            mealResult.Should().BeTrue();
        }

        [Fact]
        public async Task AddProductToMeal_WithNotExistingIds_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.AddProductToMealAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var mealResult = await _sut.AddProductToMeal(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            mealResult.Should().BeFalse();
        }

        [Fact]
        public async Task RemoveProductFromMeal_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var mealId = _fixture.Items.FirstOrDefault().Id;
            var productId = 1;
            _mockRepository.Setup(r => r.RemoveProductFromMealAsync(mealId, productId)).ReturnsAsync(true);

            // Act
            var mealResult = await _sut.RemoveProductFromMeal(mealId, productId);

            // Assert
            mealResult.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveProductFromMeal_WithNotExistingIds_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.RemoveProductFromMealAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var mealResult = await _sut.RemoveProductFromMeal(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            mealResult.Should().BeFalse();
        }
        #endregion
    }
}
