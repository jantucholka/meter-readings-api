using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MeterReading.Logic.Facades;
using MeterReadings.Repository;
using MeterReadings.Schema;
using Moq;
using NUnit.Framework;

namespace MeterReadings.Tests
{
    [TestFixture]
    public class AccountFacadeTests
    {
        private AccountFacade _sut;
        private IAccountRepository _mockRepository;

        private readonly Account _account = new Account()
        {
            AccountId = 1234,
            FirstName = "Frank",
            LastName = "Test"
        };

        [SetUp]
        public void Setup()
        {
            _mockRepository = Mock.Of<IAccountRepository>(MockBehavior.Strict);
            _sut = new AccountFacade(_mockRepository);
        }

        [Test]
        public async Task AddAccount_ReturnsAccountId_WhenAccountHasBeenAdded()
        {
            // Arrange
            Mock.Get(_mockRepository).Setup(x => x.AddAccounts(It.IsAny<IEnumerable<Account>>()))
                .ReturnsAsync(() => new List<Account>(1) { new Account() { AccountId = 1 } });

            // Act
            var result = await _sut.AddAccount(_account);

            // Assert
            result.Should().Be(1);
        }

        [Test]
        public async Task GetAccounts_ReturnsAccounts()
        {
            // Arrange
            Mock.Get(_mockRepository).Setup(x => x.GetAccounts())
                .ReturnsAsync(() => new List<Account>(1)
                {
                    _account
                });

            //Act
            var result = await _sut.GetAccounts();

            //Assert
            result.Should().Contain(_account);
        }

        [Test]
        public async Task DeleteAccount_DeletesAccount()
        {
            var AccountId = 1;

            // Arrange
            Mock.Get(_mockRepository).Setup(x => x.DeleteAccount(AccountId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Act
            await _sut.DeleteAccount(AccountId);

            //Assert
            Mock.Get(_mockRepository).Verify();
        }

        [Test]
        public async Task GetAccount_ReturnsAccount()
        {
            // Arrange
            var AccountId = 1;

            Mock.Get(_mockRepository).Setup(x => x.GetAccount(AccountId))
                .ReturnsAsync(() => _account);

            //Act
            var result = await _sut.GetAccount(AccountId);

            //Assert
            result.Should().Be(_account);
        }
    }
}
