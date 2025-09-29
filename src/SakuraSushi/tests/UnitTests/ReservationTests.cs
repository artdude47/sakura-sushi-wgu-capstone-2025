using System;
using SakuraSushi.Domain;
using Xunit;

namespace UnitTests
{
    public class ReservationTests
    {
        [Fact]
        public void Create_ValidInputs_Succeeds()
        {
            var when = DateTimeOffset.UtcNow.AddDays(1);
            var r = Reservation.Create("Test", 2, when, SeatType.SushiBar, "555-555-5555");

            Assert.Equal("Test", r.Name);
            Assert.Equal(2, r.PartySize);
            Assert.True(r.CreatedAt <= DateTimeOffset.UtcNow);
            Assert.Equal(SeatType.SushiBar, r.SeatType);
        }

        [Fact]
        public void Create_PastDate_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Reservation.Create("Test", 2, DateTimeOffset.UtcNow.AddDays(-1), SeatType.SushiBar, "555-555-5555");
            });
        }

        [Fact]
        public void Create_ChangesValues_WhenValid()
        {
            var r = Reservation.Create("Test", 2, DateTimeOffset.UtcNow.AddDays(1), SeatType.SushiBar, "555-555-5555");
            var newWhen = DateTimeOffset.UtcNow.AddDays(2);

            r.Update("Tester", 4, newWhen, SeatType.Table, "444-444-4444");

            Assert.Equal("Tester", r.Name);
            Assert.Equal(4, r.PartySize);
            Assert.Equal(SeatType.Table, r.SeatType);
            Assert.Equal("444-444-4444", r.Phone);
            Assert.Equal(newWhen, r.At);
        }
    }
}
