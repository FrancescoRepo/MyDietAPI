using MyDiet_API.Shared.Dtos;

namespace MyDiet_API.UnitTests.Fixture
{
    public class DietFixture : BaseDBFixture<DietDto>
    {
        public DietFixture() : base()
        {
            Populate();
        }

        private void Populate()
        {
            for(int i = 0; i < 30; i++)
            {
                Items.Add(new DietDto
                {
                    Id = i + 1,
                    Name = $"Diet {i + 1}",
                    Description = $"Diet description {i + 1}",
                });
            }
        }
    }
}
