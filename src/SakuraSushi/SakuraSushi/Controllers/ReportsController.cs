using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SakuraSushi.Data;

[Authorize(Roles = "Owner,Staff")]
public class ReportsController : Controller
{
    private readonly AppDbContext _db;
    public ReportsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> ReservationsBySlot(DateTime? from = null, DateTime? to = null)
    {
        // establish a local-offset range from the optional dates
        var offset = DateTimeOffset.Now.Offset;

        DateTimeOffset? start = null;
        DateTimeOffset? endExclusive = null;

        if (from.HasValue)
            start = new DateTimeOffset(DateTime.SpecifyKind(from.Value.Date, DateTimeKind.Unspecified), offset);

        if (to.HasValue)
            endExclusive = new DateTimeOffset(DateTime.SpecifyKind(to.Value.Date.AddDays(1), DateTimeKind.Unspecified), offset);

        // materialize first (SQLite has translation limits on DateTimeOffset compare/sort)
        var all = await _db.Reservations.AsNoTracking().ToListAsync();

        // filter IN MEMORY to avoid translation errors
        var filtered = all.AsEnumerable();
        if (start is not null) filtered = filtered.Where(r => r.At >= start.Value);
        if (endExclusive is not null) filtered = filtered.Where(r => r.At < endExclusive.Value);

        var rows = filtered
            .Select(r => new
            {
                SortKey = r.At,
                Date = r.At.LocalDateTime.ToString("yyyy-MM-dd"),
                Time = r.At.LocalDateTime.ToString("HH:mm"),
                r.PartySize,
                r.SeatType,
                Created = r.CreatedAt.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss")
            })
            .OrderBy(r => r.SortKey)
            .ToList();

        ViewBag.Title = "Reservations by Date/Time";
        ViewBag.GeneratedAt = DateTime.Now;
        return View(rows);
    }
}
