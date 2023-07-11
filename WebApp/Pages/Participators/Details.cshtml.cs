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
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

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
    }
}
