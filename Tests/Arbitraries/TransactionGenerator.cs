using FluentAssertions.Execution;
using FsCheck;
using FsCheck.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletPropertyTesting.Domain;
namespace WalletPropertyTesting.Tests.Arbitraries
{
    public static class TransactionGenerator
    {
        public static Arbitrary<Transaction> Transaction()
        {
            var gen =
                from money in MoneyGenerator.Money().Generator
                from type in Gen.Elements(TransactionType.Deposit, TransactionType.Withdraw)
                select type == TransactionType.Deposit ? Domain.Transaction.Deposit(money) : Domain.Transaction.Withdraw(money);
            return Arb.From(gen);
        }
    }

}
