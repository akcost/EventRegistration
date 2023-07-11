using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.EventParticipators
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<EventParticipator> EventParticipator { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.EventParticipators != null)
            {
                EventParticipator = await _context.EventParticipators
                .Include(e => e.EventInfo)
                .Include(e => e.Participator).ToListAsync();
            }
        }
    }
}
