using MyDiet_API.Shared.Dtos;

namespace MyDiet_API.UnitTests.Fixture
{
    public class ProductCategoryFixture : BaseDBFixture<ProductCategoryDto>
    {
        public ProductCategoryFixture() : base()
        {
            Populate();
        }

        private void Populate()
        {
            for(int i = 0; i < 30; i++)
            {
                Items.Add(new ProductCategoryDto
                {
                    Id = i + 1,
                    Description = $"ProductCategoryDto {i + 1}"
                });
            }
        }
    }
}
