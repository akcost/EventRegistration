using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly DAL.AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<EventInfo> EventInfos { get;set; } = default!;
    public IList<EventInfo> FutureEvents { get;set; } = default!;
    public IList<EventInfo> PastEvents { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.EventInfos != null)
        {
            EventInfos =  await _context.EventInfos.ToListAsync();

            var currentDate = DateTime.Now;
            
            PastEvents = EventInfos
                .Where(e => e.EventDateTime < currentDate)
                .OrderBy(e => e.EventDateTime)
                .ToList();

            FutureEvents = EventInfos
                .Where(e => e.EventDateTime >= currentDate)
                .OrderBy(e => e.EventDateTime)
                .ToList();
        }
    }
}