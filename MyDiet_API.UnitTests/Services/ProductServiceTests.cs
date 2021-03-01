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
    public class ProductServiceTests : IClassFixture<ProductFixture>
    {
        private IProductService _sut;
        private Mock<IProductRepository> _mockRepository;
        private ProductFixture _fixture;

        public ProductServiceTests(ProductFixture fixture)
        {
            _mockRepository = new Mock<IProductRepository>();
            _sut = new ProductService(_mockRepository.Object);
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsAllItems()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(_fixture.Items);

            // Act
            var productResult = await _sut.GetAllAsync();

            // Assert
            productResult.Should().NotBeNullOrEmpty();
            productResult.Should().BeEquivalentTo(_fixture.Items);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsProduct()
        {
            // Arrange
            var productDto = _fixture.Items.FirstOrDefault();
            var productId = productDto.Id;
            _mockRepository.Setup(r => r.GetAsync(productId)).ReturnsAsync(productDto);

            // Act
            var productResult = await _sut.GetAsync(productId);

            // Assert
            _mockRepository.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Once);
            productResult.Should().NotBeNull();
            productResult.Should().BeEquivalentTo(productDto);
        }

        [Fact]
        public async Task Get_WithNotExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var productResult = await _sut.GetAsync(It.IsAny<int>());

            // Assert
            _mockRepository.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Once);
            productResult.Should().BeNull();
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsProduct()
        {
            // Arrange
            var productDto = new ProductDto()
            {
                Name = "ProductDto 1",
                Description = "ProductDtoDescription 1",
                ProductCategory = new ProductCategoryDto
                {
                    Id = 1,
                    Description = "ProductCategoryDto 1"
                }
            };
            var expectedProductDto = new ProductDto()
            {
                Id = 1,
                Description = "ProductDtoDescription 1",
                ProductCategory = new ProductCategoryDto
                {
                    Id = 1,
                    Description = "ProductCategoryDto 1"
                }
            };
            _mockRepository.Setup(r => r.CreateAsync(productDto)).ReturnsAsync(expectedProductDto);

            // Act
            var productResult = await _sut.CreateAsync(productDto);

            // Assert
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<ProductDto>()), Times.Once);
            productResult.Should().NotBeNull();
            productResult.Should().BeEquivalentTo(expectedProductDto);
        }

        [Fact]
        public async Task Update_WhenCalled_ReturnsProduct()
        {
            // Arrange
            var productDto = _fixture.Items.FirstOrDefault();
            var expectedProductDto = new ProductDto()
            {
                Id = productDto.Id,
                Name = "New name",
                Description = "New Description",
                ProductCategory = productDto.ProductCategory
            };
            _mockRepository.Setup(r => r.UpdateAsync(productDto.Id, productDto)).ReturnsAsync(expectedProductDto);

            // Act
            var productResult = await _sut.UpdateAsync(productDto.Id, productDto);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<ProductDto>()), Times.Once);
            productResult.Should().NotBeNull();
            productResult.Should().BeEquivalentTo(expectedProductDto);
        }

        [Fact]
        public async Task Update_WithNotExistingId_RetrunsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<ProductDto>())).ReturnsAsync(() => null);

            // Act
            var productResult = await _sut.UpdateAsync(It.IsAny<int>(), It.IsAny<ProductDto>());

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<ProductDto>()), Times.Once);
            productResult.Should().BeNull();
        }

        [Fact]
        public async Task Delete_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var productId = _fixture.Items.FirstOrDefault().Id;
            _mockRepository.Setup(r => r.DeleteAsync(productId)).ReturnsAsync(true);

            // Act
            var productResult = await _sut.DeleteAsync(productId);

            // Assert
            productResult.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_WithNotExistingId_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var productResult = await _sut.DeleteAsync(It.IsAny<int>());

            // Assert
            productResult.Should().BeFalse();
        }
    }
}
