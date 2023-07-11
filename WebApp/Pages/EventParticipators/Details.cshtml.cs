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
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

      public EventParticipator EventParticipator { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.EventParticipators == null)
            {
                return NotFound();
            }

            var eventparticipator = await _context.EventParticipators.FirstOrDefaultAsync(m => m.Id == id);
            if (eventparticipator == null)
            {
                return NotFound();
            }
            else 
            {
                EventParticipator = eventparticipator;
            }
            return Page();
        }
    }
}
