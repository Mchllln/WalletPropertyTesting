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
using static System.Collections.Specialized.BitVector32;

namespace WalletPropertyTesting.Tests.Properties
{

    public class MoneyProperties
    {
        [Property]
        public void Money_NegativeAmount_ThrowsArgumentException(PositiveInt cents)
        {
            Action action = () => new Money(-cents.Get/100m) ;
            action.Should().Throw<ArgumentException>();
        }

        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Money_AddThenSubtract_ReturnsOriginal(Money a, Money b)
        {
            Money result = (a + b) - b;
            result.Amount.Should().Be(a.Amount);
        }

        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public Property Money_SubtractMoreThanHave_Throws(Money a, Money b)
        {
            bool condition = a.Amount < b.Amount;
            Action action = () =>
            {
                Action act = () => { var result = a - b; };
                act.Should().Throw<InvalidOperationException>();
            };
            return action.When(condition);
        }
    }
    

}
