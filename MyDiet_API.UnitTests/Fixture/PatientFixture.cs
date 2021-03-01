using MyDiet_API.Shared.Dtos;

namespace MyDiet_API.UnitTests.Fixture
{
    public class PatientFixture : BaseDBFixture<PatientDto>
    {
        public PatientFixture() : base()
        {
            Populate();
        }

        private void Populate()
        {
            for (int i = 0; i < 30; i++)
            {
                Items.Add(new PatientDto
                {
                    Id = i + 1,
                    Name = $"Patient {i + 1}",
                    Surname = $"Patient {i + 1}",
                    Age = (i + 1) * 10,
                    Email = $"patient@patient{i + 1}.com",
                    Gender = $"male",
                    FiscalCode = $"PatientFiscalCode{i +1}",
                    Phone = $"{i + 1}45863259"
                }) ;
            }
        }
    }
}
