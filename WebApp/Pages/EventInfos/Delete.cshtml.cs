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
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public EventInfo EventInfo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.EventInfos == null)
            {
                return NotFound();
            }

            var eventinfo = await _context.EventInfos.FirstOrDefaultAsync(m => m.Id == id);

            if (eventinfo == null)
            {
                return NotFound();
            }
            else 
            {
                EventInfo = eventinfo;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.EventInfos == null)
            {
                return NotFound();
            }
            var eventinfo = await _context.EventInfos.FindAsync(id);

            if (eventinfo != null)
            {
                EventInfo = eventinfo;
                _context.EventInfos.Remove(EventInfo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
