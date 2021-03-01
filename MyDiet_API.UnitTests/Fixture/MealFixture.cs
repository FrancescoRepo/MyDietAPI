using MyDiet_API.Shared.Dtos;

namespace MyDiet_API.UnitTests.Fixture
{
    public class MealFixture : BaseDBFixture<MealDto>
    {
        public MealFixture() : base()
        {
            Populate();
        }

        private void Populate()
        {
            for(int i = 0; i < 30; i++)
            {
                Items.Add(new MealDto
                {
                    Id = i + 1,
                    Description = $"MealDto {i + 1}"
                });
            }
        }
    }
}
