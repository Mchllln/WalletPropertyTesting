using FluentAssertions;
using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletPropertyTesting.Domain;
using WalletPropertyTesting.Tests.Arbitraries;

namespace WalletPropertyTesting.Tests.Properties
{

    public class RepositoryProperties
    {
        [Property]
        public void Repository_GetUnknownId_ThrowsKeyNotFound(Guid id)
        {
            var repo = new InMemoryWalletRepository();
            Action act = () => repo.Get(id); 
            act.Should().Throw<KeyNotFoundException>();
        }

        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Repository_SaveTwice_DoesNotDuplicateWallet(Money[] seed)
        {
            var repo = new InMemoryWalletRepository();
            Wallet wallet = new Wallet();
            foreach (var i in seed)
            {
                wallet.Deposit(i);
            }
            repo.Save(wallet);
            int countBefore = repo.All().Count(); 
            repo.Save(wallet);
            int countAfter = repo.All().Count();  
            countAfter.Should().Be(countBefore);  
        }

        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Service_Deposit_SameResultAsDirectWalletDeposit(Money m)
        {
            Wallet directWallet = new Wallet();
            directWallet.Deposit(m);
            var repo = new InMemoryWalletRepository();
            var service = new WalletService(repo);
            Wallet serviceWallet = service.CreateWallet();
            Guid id = serviceWallet.Id;
            service.Deposit(id, m.Amount);
            Money serviceBalance = service.GetBalance(id);
            serviceBalance.Amount.Should().Be(directWallet.Balance.Amount);
        }

    }
}
