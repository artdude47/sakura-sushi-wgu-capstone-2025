using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SakuraSushi.Data;
using SakuraSushi.Domain;
using System.ComponentModel.DataAnnotations;

namespace SakuraSushi.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _db;
        public BookingController(AppDbContext db) => _db = db;

        public IActionResult Create() => View(new BookingVm());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            try
            {
                var dto = new DateTimeOffset(vm.AtLocal, DateTimeOffset.Now.Offset);
                var entity = Reservation.Create(vm.Name, vm.PartySize, dto, vm.SeatType, vm.Phone);
                _db.Add(entity);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Thanks), new { id = entity.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> Thanks(int id)
        {
            var r = await _db.Reservations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (r == null) return NotFound();
            return View(r);
        }
    }

    public class BookingVm
    {
        [Required, StringLength(80)]
        public string Name { get; set; } = "";

        [Range(1, 10)]
        public int PartySize { get; set; } = 2;

        [Required, DataType(DataType.DateTime)]
        public DateTime AtLocal { get; set; } = DateTime.Now.AddHours(24);

        [Required]
        public SeatType SeatType { get; set; } = SeatType.Table;

        [Phone, StringLength(24)]
        public string Phone { get; set; } = "";
    }
}
