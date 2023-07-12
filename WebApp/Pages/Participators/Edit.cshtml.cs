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

namespace WebApp.Pages.Participators
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Participator Participator { get; set; } = default!;
        [BindProperty]
        public Person Person { get; set; } = default!;
        [BindProperty]
        public Company Company { get; set; } = default!;
        
        public int? EventInfoId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? eventInfoId)
        {
            if (id == null || _context.Participators == null)
            {
                return NotFound();
            }

            if (eventInfoId != null)
            {
                EventInfoId = eventInfoId;
            }

            var participator = await _context.Participators
                .Include(p => p.Person)
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (participator == null)
            {
                return NotFound();
            }
            Console.WriteLine("Person: " + participator.Person);
            Console.WriteLine("Company: " + participator.Company);
            Participator = participator;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? eventId)
        {
            
            _context.Attach(Participator).State = EntityState.Modified;
            
            if (Participator.Person != null)
            {
                _context.Attach(Participator.Person).State = EntityState.Modified;
            }

            if (Participator.Company != null)
            {
                _context.Attach(Participator.Company).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipatorExists(Participator.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (eventId != null)
            {
                return RedirectToPage("/EventInfos/Details", new { id = eventId });
            }
            return RedirectToPage("./Index");
        }

        private bool ParticipatorExists(int id)
        {
          return (_context.Participators?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
