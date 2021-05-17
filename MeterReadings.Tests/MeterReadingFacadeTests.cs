using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.Results;
using MeterReading.Logic;
using MeterReading.Logic.Facades;
using MeterReading.Logic.Validators;
using MeterReadings.Repository;
using MeterReadings.Schema;
using Moq;
using NUnit.Framework;

namespace MeterReadings.Tests
{
    [TestFixture]
    public class MeterReadingFacadeTests
    {
        private MeterReadingFacade _sut;
        private IMeterReadingsRepository _mockRepository;
        private IMeterReadingLenientValidator _mockValidator;
        private ICsvHelper _csvHelper;

        private readonly MeterReadingLenient _meterReading = new MeterReadingLenient()
        {
            AccountId = "2344",
            MeterReadingDateTime = "22/04/2019 09:24",
            MeterReadValue = "01002"
        };

        private readonly Collection<HttpContent> _httpContents = new Collection<HttpContent>()
        {
            new StringContent(@"AccountId,MeterReadingDateTime,MeterReadValue
2344,22/04/2019 09:24,01002")
        };

        [SetUp]
        public void Setup()
        {
            _mockRepository = Mock.Of<IMeterReadingsRepository>(MockBehavior.Strict);
            _mockValidator = Mock.Of<IMeterReadingLenientValidator>(MockBehavior.Strict);
            _csvHelper = Mock.Of<ICsvHelper>(MockBehavior.Strict);
            _sut = new MeterReadingFacade(
                _mockRepository,
                _mockValidator,
                _csvHelper);

            Mock.Get(_csvHelper).Setup(x => x.ReadCsvFromRequestIntoCollectionOfType<MeterReadingLenient>(_httpContents))
                .ReturnsAsync(new List<MeterReadingLenient>()
                {
                    _meterReading
                });
        }

        [Test]
        public async Task AddMeterReadings_ReturnsValidationFailures_WhenValidatorProducesValidationErrors()
        {
            // Arrange

            var validationResult = new ValidationResult();

            validationResult.Errors.Add(new ValidationFailure(nameof(MeterReadingLenient.AccountId), "Test error"));

            Mock.Get(_mockValidator).Setup(x => x.Validate(_meterReading))
                .Returns(() => { return validationResult; });

            Mock.Get(_mockRepository).Setup(x => x.AddReadings(It.IsAny<List<Schema.MeterReading>>()))
                .ReturnsAsync(() => new List<Schema.MeterReading>(0));

            // Act
            var result = await _sut.AddMeterReadings(_httpContents);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().Contain("AccountId: 2344, MeterReadingDateTime: 22/04/2019 09:24, MeterReadValue: 01002 - Test error");
            result.SuccesfullCount.Should().Be(0);
            result.FailedCount.Should().Be(1);
        }

        [Test]
        public async Task AddMeterReadings_ReturnsCorrectSuccessCount_WhenNoValidationAndInsertionIssuesOccur()
        {
            // Arrange
            Mock.Get(_mockValidator).Setup(x => x.Validate(_meterReading))
                .Returns(() => new ValidationResult());

            Mock.Get(_mockRepository).Setup(x => x.AddReadings(It.IsAny<List<Schema.MeterReading>>()))
                .ReturnsAsync(() => new List<Schema.MeterReading>(new List<Schema.MeterReading>(1) { _meterReading.ToMeterReading() }));

            // Act
            var result = await _sut.AddMeterReadings(_httpContents);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Count().Should().Be(0);
            result.SuccesfullCount.Should().Be(1);
            result.FailedCount.Should().Be(0);
        }

        [Test]
        public async Task AddMeterReadings_ReturnsError_WhenRepositoryIndicatesFailureToAdd()
        {
            // Arrange
            Mock.Get(_mockValidator).Setup(x => x.Validate(_meterReading))
                .Returns(() => new ValidationResult());

            Mock.Get(_mockRepository).Setup(x => x.AddReadings(It.IsAny<List<Schema.MeterReading>>()))
                .ReturnsAsync(() => new List<Schema.MeterReading>(new List<Schema.MeterReading>(0)));

            // Act
            var result = await _sut.AddMeterReadings(_httpContents);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().Contain("AccountId: 2344, MeterReadingDateTime: 22/04/2019 09:24:00, MeterReadValue: 01002 - AccountId doesn't exist or reading has already been added");
            result.SuccesfullCount.Should().Be(0);
            result.FailedCount.Should().Be(1);
        }

        [Test]
        public async Task AddMeterReading_ReturnsCorrectSuccessCount_WhenNoValidationAndInsertionIssuesOccur()
        {
            // Arrange
            Mock.Get(_mockValidator).Setup(x => x.Validate(_meterReading))
                .Returns(() => new ValidationResult());

            var reading = _meterReading.ToMeterReading();

            Mock.Get(_mockRepository).Setup(x => x.AddReadings(It.IsAny<Schema.MeterReading[]>()))
                .ReturnsAsync(() => new List<Schema.MeterReading>(new List<Schema.MeterReading>(1) { reading }));

            // Act
            var result = await _sut.AddMeterReading(reading);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task GetReadings_ReturnsReadings()
        {
            // Arrange
            var reading = _meterReading.ToMeterReading();
            Mock.Get(_mockRepository).Setup(x => x.GetReadings())
                .ReturnsAsync(() => new List<Schema.MeterReading>() { reading });

            //Act
            var result = await _sut.GetReadings();

            //Assert
            result.Should().Contain(reading);
        }

        [Test]
        public async Task DeleteReading_DeletesReading()
        {
            var readingId = Guid.NewGuid();

            // Arrange
            Mock.Get(_mockRepository).Setup(x => x.DeleteReading(readingId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Act
            await _sut.DeleteReading(readingId);

            //Assert
            Mock.Get(_mockRepository).Verify();
        }

        [Test]
        public async Task GetReading_ReturnsReading()
        {
            // Arrange
            var readingId = Guid.NewGuid();
            var reading = _meterReading.ToMeterReading();
            reading.Id = readingId;
            
            Mock.Get(_mockRepository).Setup(x => x.GetReading(readingId))
                .ReturnsAsync(() => reading)
                .Verifiable();

            //Act
            var result = await _sut.GetReading(readingId);

            //Assert
            Mock.Get(_mockRepository).Verify();
            result.Should().Be(reading);
        }
    }
}
