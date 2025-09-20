namespace SakuraSushi.Domain
{
    public abstract class MenuItem
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public string? ImagePath { get; private set; }

        protected MenuItem() { }
        protected MenuItem(string name, string desc, decimal price)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name Required!");
            if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));
            Name = name.Trim();
            Description = desc.Trim();
            Price = price;
        }

        public virtual string GetDisplayName() => Name;
        public void UpdateDetails(string name, string desc, decimal price)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name Required!");
            if (price <= 0) throw new ArgumentOutOfRangeException(nameof (price));
            Name = name.Trim();
            Description = desc.Trim();
            Price = price;
        }
        public void SetImage(string path) => ImagePath = path;

        public sealed class Nigiri : MenuItem 
        {
            private Nigiri() { }
            public Nigiri(string name, string desc, decimal price) : base(name, desc, price) { }
            public override string GetDisplayName()
            {
                return base.GetDisplayName() + " (Nigiri)";
            }
        }

        public sealed class Sashimi : MenuItem
        {
            private Sashimi() { }
            public Sashimi(string name, string desc, decimal price) : base(name, desc, price) { }
            public override string GetDisplayName()
            {
                return base.GetDisplayName() + " (Sashimi)";
            }
        }

        public sealed class Roll : MenuItem
        {
            private Roll() { }
            public Roll(string name, string desc, decimal price) : base(name, desc, price) { }
            public override string GetDisplayName()
            {
                return base.GetDisplayName() + " (Roll)";
            }
        }
    }
}
