using FluentAssertions;
using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletPropertyTesting.Domain;
using WalletPropertyTesting.Tests.Arbitraries;

namespace WalletPropertyTesting.Tests.Properties
{

    public class WalletProperties
    {
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Wallet_DepositThenWithdrawSame_BalanceUnchanged(Money[] seed, Money m)
        {
            Wallet wallet = new Wallet();
            foreach (var i in seed)
            {
                wallet.Deposit(i);
            }
            var balance = wallet.Balance;
            wallet.Deposit(m);
            wallet.Withdraw(m);
            var result = wallet.Balance;
            result.Amount.Should().Be(balance.Amount);
        }

        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public Property Wallet_AfterWithdraw_BalanceNotAboveTotalDeposits(Money[] deposits, Money w)
        {
            Wallet wallet = new Wallet();
            foreach (var i in deposits)
            {
                wallet.Deposit(i);
            }
            bool condition = w.Amount <= wallet.Balance.Amount;
            decimal totalDeposited = deposits.Sum(d => d.Amount);
            Action action = () =>
            {
                wallet.Withdraw(w);
                wallet.Balance.Amount.Should().BeLessOrEqualTo(totalDeposited);
            };
            return action.When(condition);
        }

        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Wallet_Deposit_BalanceIncreaseEqualsDeposited(Money[] seed, Money m)
        {
            Wallet wallet = new Wallet();
            foreach (var i in seed)
            {
                wallet.Deposit(i);
            }
            var balanceBefore = wallet.Balance;
            wallet.Deposit(m);
            var balanceAfter = wallet.Balance;
            var result = balanceAfter.Amount - balanceBefore.Amount;

            result.Should().Be(m.Amount);
        }
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Wallet_TwoDepositsVsOneCombined_SameBalance(Money a, Money b)
        {
            Wallet wallet1 = new Wallet();
            Wallet wallet2 = new Wallet();
            wallet1.Deposit(a);
            wallet1.Deposit(b);
            var balanceWallet1 = wallet1.Balance;
            wallet2.Deposit(a + b);
            var balanceWallet2 = wallet2.Balance;
            balanceWallet1.Amount.Should().Be(balanceWallet2.Amount);
        }

        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Wallet_WithdrawMoreThanBalance_Throws(Money[] deposits, PositiveInt extra) 
        {
            Wallet wallet = new Wallet();
            foreach (var i in deposits)
            {
                wallet.Deposit(i);
            }
            var sum = wallet.Balance.Amount + (extra.Get / 100m);
            Money w = new Money(sum);
            Action act = () => { wallet.Withdraw(w); };
            act.Should().Throw<InvalidOperationException>();
        }

        [Property(Arbitrary = new[] { typeof(MoneyGenerator), typeof(TransactionGenerator) })]
        public void Transaction_Ids_AreUnique(Transaction[] txs)
        {
            var ids = txs.Select(t => t.Id);
            var uniqueIds = ids.Distinct();
            uniqueIds.Count().Should().Be(txs.Length);
        }
    }

}
