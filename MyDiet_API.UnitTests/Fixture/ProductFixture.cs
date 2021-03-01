using MyDiet_API.Shared.Dtos;

namespace MyDiet_API.UnitTests.Fixture
{
    public class ProductFixture : BaseDBFixture<ProductDto>
    {
        public ProductFixture() : base()
        {
            Populate();
        }

        private void Populate()
        {
            for(int i = 0; i < 30; i++)
            {
                Items.Add(new ProductDto
                {
                    Id = i + 1,
                    Name = $"ProductDto {i + 1}",
                    Description = $"ProductDto Descr {i + 1}",
                    ProductCategory = new ProductCategoryDto
                    {
                        Id = i + 1,
                        Description = $"ProductCategoryDto {i + 1}"
                    }
                }); ;
            }
        }
    }
}
