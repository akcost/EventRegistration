using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Participators
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Participator> Participator { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Participators != null)
            {
                Participator = await _context.Participators
                .Include(p => p.Company)
                .Include(p => p.Person).ToListAsync();
            }
        }
    }
}
