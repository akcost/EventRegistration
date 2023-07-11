using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.EventParticipators
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EventParticipator EventParticipator { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.EventParticipators == null)
            {
                return NotFound();
            }

            var eventparticipator =  await _context.EventParticipators.FirstOrDefaultAsync(m => m.Id == id);
            if (eventparticipator == null)
            {
                return NotFound();
            }
            EventParticipator = eventparticipator;
           ViewData["EventInfoId"] = new SelectList(_context.EventInfos, "Id", "EventName");
           ViewData["ParticipatorId"] = new SelectList(_context.Participators, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(EventParticipator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventParticipatorExists(EventParticipator.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EventParticipatorExists(int id)
        {
          return (_context.EventParticipators?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
