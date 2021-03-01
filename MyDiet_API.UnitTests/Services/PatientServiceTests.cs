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
    public class PatientServiceTests : IClassFixture<PatientFixture>
    {
        private IPatientService _sut;
        private Mock<IPatientRepository> _mockRepository;
        private PatientFixture _fixture;

        public PatientServiceTests(PatientFixture fixture)
        {
            _mockRepository = new Mock<IPatientRepository>();
            _sut = new PatientsService(_mockRepository.Object);
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsAllItems()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(_fixture.Items);

            // Act
            var patientResult = await _sut.GetAllAsync();

            // Assert
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
            patientResult.Should().NotBeNullOrEmpty();
            patientResult.Should().BeEquivalentTo(_fixture.Items);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsPatient()
        {
            // Arrange
            var patient = _fixture.Items.FirstOrDefault();
            var patientId = patient.Id;
            _mockRepository.Setup(r => r.GetAsync(patientId)).ReturnsAsync(patient);

            // Act
            var patientResult = await _sut.GetAsync(patientId);

            // Assert
            _mockRepository.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Once);
            patientResult.Should().NotBeNull();
            patientResult.Should().BeEquivalentTo(patient);
        }

        [Fact]
        public async Task Get_WithNotExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var patientResult = await _sut.GetAsync(It.IsAny<int>());

            // Assert
            _mockRepository.Verify(r => r.GetAsync(It.IsAny<int>()), Times.Once);
            patientResult.Should().BeNull();
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsPatient()
        {
            // Arrange
            var patientDto = new PatientDto()
            {
                Name = "Patient 1",
                Surname = "Patient 1",
                Age = 25,
                Email = "patient@patient.com",
                Gender = "male",
                FiscalCode = "PatientFiscalCode",
                Phone = "45863259",
                Weight = 70
            };
            var expectedPatientDto = new PatientDto()
            {
                Id = 1,
                Name = patientDto.Name,
                Surname = patientDto.Surname,
                Age = patientDto.Age,
                Email = patientDto.Email,
                Gender = patientDto.Gender,
                FiscalCode = patientDto.FiscalCode,
                Phone = patientDto.Phone,
                Weight = patientDto.Weight
            };
            _mockRepository.Setup(r => r.CreateAsync(patientDto)).ReturnsAsync(expectedPatientDto);

            // Act
            var patientResult = await _sut.CreateAsync(patientDto);

            // Assert
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<PatientDto>()), Times.Once);
            patientResult.Should().NotBeNull();
            patientResult.Should().BeEquivalentTo(expectedPatientDto);
        }

        [Fact]
        public async Task Update_WhenCalled_ReturnsPatient()
        {
            // Arrange
            var patientDto = _fixture.Items.FirstOrDefault();
            var patientId = patientDto.Id;
            var expectedPatientDto = new PatientDto()
            {
                Id = patientDto.Id,
                Name = "Patient 11",
                Surname = patientDto.Surname,
                Age = patientDto.Age,
                Email = patientDto.Email,
                Gender = patientDto.Gender,
                FiscalCode = patientDto.FiscalCode,
                Phone = patientDto.Phone,
                Weight = patientDto.Weight
            };
            _mockRepository.Setup(r => r.UpdateAsync(patientId, patientDto)).ReturnsAsync(expectedPatientDto);

            // Act
            var patientResult = await _sut.UpdateAsync(patientId, patientDto);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<PatientDto>()), Times.Once);
            patientResult.Should().NotBeNull();
            patientResult.Should().BeEquivalentTo(expectedPatientDto);
        }

        [Fact]
        public async Task Update_WithNotExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<PatientDto>())).ReturnsAsync(() => null);

            // Act
            var patientResult = await _sut.UpdateAsync(It.IsAny<int>(), It.IsAny<PatientDto>());

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<PatientDto>()), Times.Once);
            patientResult.Should().BeNull();
        }

        [Fact]
        public async Task Delete_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var patientId = _fixture.Items.FirstOrDefault().Id;
            _mockRepository.Setup(r => r.DeleteAsync(patientId)).ReturnsAsync(true);

            // Act
            var patientResult = await _sut.DeleteAsync(patientId);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
            patientResult.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_WithNotExistingId_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var patientResult = await _sut.DeleteAsync(It.IsAny<int>());

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
            patientResult.Should().BeFalse();
        }
    }
}
