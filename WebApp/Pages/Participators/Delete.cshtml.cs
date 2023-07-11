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
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Participator Participator { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Participators == null)
            {
                return NotFound();
            }

            var participator = await _context.Participators.FirstOrDefaultAsync(m => m.Id == id);

            if (participator == null)
            {
                return NotFound();
            }
            else 
            {
                Participator = participator;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Participators == null)
            {
                return NotFound();
            }
            var participator = await _context.Participators.FindAsync(id);

            if (participator != null)
            {
                Participator = participator;
                _context.Participators.Remove(Participator);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
