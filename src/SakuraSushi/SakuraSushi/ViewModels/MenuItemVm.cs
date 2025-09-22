using System.ComponentModel.DataAnnotations;

namespace SakuraSushi.ViewModels
{
    public enum MenuItemType
    {
        Nigiri,
        Sashimi,
        Roll
    }

    public class MenuItemVm
    {
        public int Id { get; set; }
        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(500)]
        public string Description { get; set; } = string.Empty;
        [Range(0.01, 1000)]
        public decimal Price { get; set; }
        [Url, StringLength(400)]
        public string? ImageUrl { get; set; }
        [Required]
        public MenuItemType ItemType { get; set; }
    }
}
