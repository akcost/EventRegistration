using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.EventInfos
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<EventInfo> EventInfo { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.EventInfos != null)
            {
                EventInfo = await _context.EventInfos.ToListAsync();
            }
        }
    }
}
