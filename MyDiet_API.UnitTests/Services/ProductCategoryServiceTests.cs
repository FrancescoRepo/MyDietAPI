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
    public class ProductCategoryServiceTests : IClassFixture<ProductCategoryFixture>
    {
        private IProductCategoryService _sut;
        private Mock<IProductCategoryRepository> _mockRepository;
        private ProductCategoryFixture _fixture;

        public ProductCategoryServiceTests(ProductCategoryFixture fixture)
        {
            _mockRepository = new Mock<IProductCategoryRepository>();
            _sut = new ProductCategoriesService(_mockRepository.Object);
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsAllItems()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(_fixture.Items);

            // Act
            var productCategoriesResult = await _sut.GetAllAsync();

            // Assert
            productCategoriesResult.Should().NotBeNullOrEmpty();
            productCategoriesResult.Should().BeEquivalentTo(_fixture.Items);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsProductCategory()
        {
            // Arrange
            int productCategoryId = 1;
            var productCategoryDto = _fixture.Items.Find(pc => pc.Id == productCategoryId);

            _mockRepository.Setup(r => r.GetAsync(productCategoryId)).ReturnsAsync(productCategoryDto);

            // Act
            var productCategoryResult = await _sut.GetAsync(productCategoryId);

            // Assert
            productCategoryResult.Should().NotBeNull();
            productCategoryResult.Should().BeEquivalentTo(productCategoryDto);
        }

        [Fact]
        public async Task Get_WithNotExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var productCategoryResult = await _sut.GetAsync(10);

            //Assert
            productCategoryResult.Should().BeNull();
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsNewProductCategory()
        {
            // Arrange
            var productCategoryDto = new ProductCategoryDto()
            {
                Description = "ProductCategoryDto 1"
            };
            var expectedProductCategoryDto = new ProductCategoryDto()
            {
                Id = 1,
                Description = "ProductCategoryDto 1"
            };
            _mockRepository.Setup(r => r.CreateAsync(productCategoryDto)).ReturnsAsync(expectedProductCategoryDto);

            // Act
            var productCategoryResult = await _sut.CreateAsync(productCategoryDto);

            // Assert
            productCategoryResult.Should().NotBeNull();
            productCategoryResult.Should().BeEquivalentTo(expectedProductCategoryDto);
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<ProductCategoryDto>()), Times.Once);
        }

        [Fact]
        public async Task Update_WhenCalled_ReturnsUpdatedProductCategory()
        {
            // Arrange
            var productCategory = _fixture.Items.FirstOrDefault();
            var expectedProductCategory = new ProductCategoryDto()
            {
                Id = productCategory.Id,
                Description = "ProductCategoryDto 11"
            };
            _mockRepository.Setup(r => r.UpdateAsync(productCategory.Id, productCategory)).ReturnsAsync(expectedProductCategory);

            // Act
            var productCategoryResult = await _sut.UpdateAsync(productCategory.Id, productCategory);

            // Assert
            productCategoryResult.Should().NotBeNull();
            productCategoryResult.Should().BeEquivalentTo(expectedProductCategory);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<ProductCategoryDto>()), Times.Once);
        }

        [Fact]
        public async Task Update_WithNotExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<ProductCategoryDto>())).ReturnsAsync(() => null);

            // Act
            var productCategoryResult = await _sut.UpdateAsync(It.IsAny<int>(), It.IsAny<ProductCategoryDto>());

            //Assert
            productCategoryResult.Should().BeNull();
        }

        [Fact]
        public async Task Delete_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var productCategoryId = _fixture.Items.FirstOrDefault().Id;
            _mockRepository.Setup(r => r.DeleteAsync(productCategoryId)).ReturnsAsync(true);

            // Act
            var productCategoryResult = await _sut.DeleteAsync(productCategoryId);

            // Assert
            productCategoryResult.Should().Be(true);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNotExistingId_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var productCategoryResult = await _sut.DeleteAsync(It.IsAny<int>());

            // Assert
            productCategoryResult.Should().Be(false);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
