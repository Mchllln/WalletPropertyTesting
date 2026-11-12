using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletPropertyTesting.Domain;
using FsCheck.Fluent;

namespace WalletPropertyTesting.Tests.Arbitraries
{
    public static class MoneyGenerator
    {
        public static Arbitrary<Money> Money()
        {
            var randomGen = Gen.Choose(0, 100_000).Select(c => c / 100m);
            var amountGen = Gen.OneOf(Gen.Constant(0m), Gen.Constant(0.01m), randomGen, randomGen, randomGen, randomGen);
            var gen = from amount in amountGen select new Money(amount);
            return Arb.From(gen);
        }
    }

}
