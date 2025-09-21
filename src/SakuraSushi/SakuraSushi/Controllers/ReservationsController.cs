using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SakuraSushi.Data;
using SakuraSushi.Domain;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace SakuraSushi.Controllers
{
    public class ReservationVm
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;
        [Range(1, 10)]
        public int PartySize { get; set; }
        [Required]
        public DateTime AtLocal { get; set; }
        [Required]
        public string SeatType { get; set; } = string.Empty;
        [Phone, StringLength(24)]
        public string Phone { get; set; } = string.Empty;

        public DateTimeOffset ToOffset() => new DateTimeOffset(AtLocal, DateTimeOffset.Now.Offset);
        public static ReservationVm FromEntity(Reservation r) => new()
        {
            Id = r.Id,
            Name = r.Name,
            PartySize = r.PartySize,
            AtLocal = r.At.ToLocalTime().DateTime,
            SeatType = r.SeatType.ToString(),
            Phone = r.Phone
        };
    }

    [Authorize(Roles = "Owner,Staff")]
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservations.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                var entity = Reservation.Create(vm.Name, vm.PartySize, vm.ToOffset(), Enum.Parse<SeatType>(vm.SeatType), vm.Phone);
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var r = await _context.Reservations.FindAsync(id);
            if (r == null) return NotFound();
            return View(ReservationVm.FromEntity(r));
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationVm vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var r = await _context.Reservations.FindAsync(id);
            if (r == null) return NotFound();

            try
            {
                r.Update(vm.Name, vm.PartySize, vm.ToOffset(), Enum.Parse<SeatType>(vm.SeatType), vm.Phone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException) when (!ReservationExists(vm.Id))
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
