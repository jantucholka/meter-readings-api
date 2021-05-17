using System.Collections;
using System.Threading.Tasks;
using FluentAssertions;
using MeterReading.Logic.Validators;
using MeterReadings.Schema;
using NUnit.Framework;

namespace MeterReadings.Tests
{
    [TestFixture]
    public class MeterReadingLenientValidatorTests
    {
        private MeterReadingLenientValidator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new MeterReadingLenientValidator();
        }

        [TestCaseSource(nameof(ValidationFailureTestCases))]
        public async Task Validate_ReturnsError(MeterReadingLenient meterReading)
        {
            var result = _sut.Validate(meterReading);
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [TestCaseSource(nameof(ValidationSuccessTestCases))]
        public async Task Validate_ReturnsNoErrors(MeterReadingLenient meterReading)
        {
            var result = _sut.Validate(meterReading);
            result.Errors.Count.Should().Be(0);
        }

        static IEnumerable ValidationFailureTestCases()
        {
            yield return new TestCaseData(new MeterReadingLenient() { AccountId = null, MeterReadingDateTime = "22/04/2019  09:24", MeterReadValue = "00123" });
            yield return new TestCaseData(new MeterReadingLenient() { AccountId = "12345", MeterReadingDateTime = null, MeterReadValue = "00123" });
            yield return new TestCaseData(new MeterReadingLenient() { AccountId = null, MeterReadingDateTime = "22/04/2019  09:24", MeterReadValue = null });
            yield return new TestCaseData(new MeterReadingLenient() { AccountId = "abc", MeterReadingDateTime = "22/04/2019  09:24", MeterReadValue = "00123" });
            yield return new TestCaseData(new MeterReadingLenient() { AccountId = "12345", MeterReadingDateTime = "abc", MeterReadValue = "00123" });
            yield return new TestCaseData(new MeterReadingLenient() { AccountId = "12345", MeterReadingDateTime = "22/04/2019  09:24", MeterReadValue = "abc" });
            yield return new TestCaseData(new MeterReadingLenient() { AccountId = "12345", MeterReadingDateTime = "22/04/2019  09:24", MeterReadValue = "0012" });
        }

        static IEnumerable ValidationSuccessTestCases()
        {
            yield return new TestCaseData(new MeterReadingLenient() { AccountId = "12345", MeterReadingDateTime = "22/04/2019  09:24", MeterReadValue = "00123" });
        }
    }
}
