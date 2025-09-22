using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SakuraSushi.Data;
using SakuraSushi.Domain;
using SakuraSushi.ViewModels;
using static SakuraSushi.Domain.MenuItem;

namespace SakuraSushi.Controllers
{
    [Authorize(Roles = "Owner,Staff")]
    public class MenuItemsController : Controller
    {
        private readonly AppDbContext _db;
        public MenuItemsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var items = await _db.MenuItems.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
            return View(items);
        }

        public IActionResult Create() => View(new MenuItemVm());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItemVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            MenuItem entity = vm.ItemType switch
            {
                MenuItemType.Nigiri => new Nigiri(vm.Name, vm.Description, vm.Price, vm.ImageUrl),
                MenuItemType.Sashimi => new Sashimi(vm.Name, vm.Description, vm.Price, vm.ImageUrl),
                MenuItemType.Roll => new Roll(vm.Name, vm.Description, vm.Price, vm.ImageUrl),
                _ => throw new InvalidOperationException("Unknown MenuItemType")
            };
            _db.Add(entity);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var m = await _db.MenuItems.FindAsync(id);
            if (m is null) return NotFound();

            var vm = new MenuItemVm
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                ItemType = m switch
                {
                    Nigiri => MenuItemType.Nigiri,
                    Sashimi => MenuItemType.Sashimi,
                    Roll => MenuItemType.Roll,
                    _ => throw new InvalidOperationException("Unknown MenuItem subtype")
                }
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]    
        public async Task<IActionResult> Edit(int id, MenuItemVm vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var m = await _db.MenuItems.FindAsync(id);
            if (m is null) return NotFound();

            m.UpdateDetails(vm.Name, vm.Description, vm.Price, vm.ImageUrl);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.MenuItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var m = await _db.MenuItems.FindAsync(id);
            if (m == null) return NotFound();
            _db.MenuItems.Remove(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
