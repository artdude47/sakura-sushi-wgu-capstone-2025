using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SakuraSushi.Data;

namespace SakuraSushi.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppDbContext _db;
        public MenuController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Search(string q)
        {
            var query = _db.MenuItems.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var s = q.Trim().ToLower();
                query = query.Where(m => m.Name.ToLower().Contains(s) || m.Description.ToLower().Contains(s));
            }

            var items = await query.OrderBy(m => m.Name).ToListAsync();
            ViewBag.Query = q;
            return View(items);
        }
    }
}
