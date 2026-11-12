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
    // Зразки з проєкту лекції: FuelAmountGen, FuelTankGen, VehicleGen.
    // Гаманець (Wallet) генератора не потребує: створіть new Wallet() у тесті і поповніть його
    // сумами з масиву Money[] — FsCheck побудує масив сам, як Vehicle[] у лекції.
    public static class MoneyGenerator
    {
        // Money: Amount >= 0 (конструктор кидає ArgumentException для від'ємних).
        // Генеруйте суми з двома знаками після коми, напр. Gen.Choose(0, 100_000).Select(c => c / 100m);
        // нуль — важливий граничний випадок.
        public static Arbitrary<Money> Money()
        {
            var randomGen = Gen.Choose(0, 100_000).Select(c => c / 100m);
            var amountGen = Gen.OneOf(Gen.Constant(0m), Gen.Constant(0.01m), randomGen, randomGen, randomGen, randomGen);
            var gen = from amount in amountGen select new Money(amount);
            return Arb.From(gen);
        }
    }

}
