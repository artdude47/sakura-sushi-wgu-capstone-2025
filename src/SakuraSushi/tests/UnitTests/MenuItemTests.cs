using System;
using SakuraSushi.Domain;
using Xunit;
using static SakuraSushi.Domain.MenuItem;

namespace UnitTests
{
    public class MenuItemTests
    {
        [Fact]
        public void UpdateDetails_DisallowsNegativePrice()
        {
            var m = new Nigiri("Salmon", "Fresh Salmon", 3.50m);
            Assert.Throws<ArgumentOutOfRangeException>(() => m.UpdateDetails("Salmon", "Fresh Salmon", -1.00m, null));
        }

        [Fact]
        public void UpdateDetails_ChangesFields()
        {
            var m = new Roll("California", "Crab and Avocado", 8.00m, null);
            m.UpdateDetails("Spicy California", "Crab, Avocado, and Spicy Mayo", 9.00m, null);

            Assert.Equal("Spicy California", m.Name);
            Assert.Equal("Crab, Avocado, and Spicy Mayo", m.Description);
            Assert.Equal(9.00m, m.Price);
        }
    }
}
