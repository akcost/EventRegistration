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
            var eventInfo = await _context.EventInfos.FindAsync(id);

            if (eventInfo != null)
            {
                // Find all EventParticipators associated with the EventInfo
                var eventParticipators = _context.EventParticipators
                    .Include(ep => ep.Participator )
                    .Include(ep => ep.Participator!.Company)
                    .Include(ep => ep.Participator!.Person)
                    .Where(ep => ep.EventInfoId == id)
                    .ToList();
                
                // Delete the EventParticipators, Participators and associated Person/Company records
                foreach (var eventParticipator in eventParticipators)
                {
                    
                    if (eventParticipator.Participator!.Person != null)
                    {
                        _context.Persons.Remove(eventParticipator.Participator.Person);
                    }
                    else if (eventParticipator.Participator.Company != null)
                    {
                        _context.Companies.Remove(eventParticipator.Participator.Company);
                    }
                    _context.Participators.Remove(eventParticipator.Participator);
                    _context.EventParticipators.Remove(eventParticipator);
                }
                
                _context.EventInfos.Remove(eventInfo);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
