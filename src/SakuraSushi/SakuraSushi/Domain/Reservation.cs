namespace SakuraSushi.Domain
{
    public enum SeatType { Table, SushiBar }

    public class Reservation
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int PartySize { get; private set; }
        public DateTimeOffset At { get; private set; }
        public SeatType SeatType { get; private set; }
        public string Phone { get; private set; } = string.Empty;
        public DateTimeOffset CreatedAt { get;private set; }

        private Reservation() { }
        public static Reservation Create(string name, int partySize, DateTimeOffset at, SeatType seatType, string phone)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name Required!");
            if (partySize < 1 || partySize > 10) throw new ArgumentOutOfRangeException(nameof(partySize));
            if (at <= DateTimeOffset.UtcNow) throw new ArgumentException("Must be in the future");
            if (string.IsNullOrEmpty(phone)) throw new ArgumentException("Phone Required!");
            return new Reservation
            {
                Name = name.Trim(),
                PartySize = partySize,
                At = at,
                SeatType = seatType,
                Phone = phone.Trim(),
                CreatedAt = DateTimeOffset.Now
            };
        }

        public void Update(string name, int partySize, DateTimeOffset at, SeatType seatType, string phone)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name Required!");
            if (partySize < 1 || partySize > 10) throw new ArgumentOutOfRangeException(nameof(partySize));
            if (at <= DateTimeOffset.UtcNow) throw new ArgumentException("Must be in the future");
            if (string.IsNullOrEmpty(phone)) throw new ArgumentException("Phone Required!");
            Name = name.Trim();
            PartySize = partySize;
            At = at;
            SeatType = seatType;
            Phone = phone.Trim();
        }
    }
}
