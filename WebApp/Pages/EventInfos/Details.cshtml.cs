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
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public EventInfo? EventInfo { get; set; } = default!;
        public List<Participator?> Participators { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EventInfo = await _context.EventInfos.FirstOrDefaultAsync(m => m.Id == id);

            if (EventInfo == null)
            {
                return NotFound();
            }

            Participators = await _context.EventParticipators
                .Include(ep => ep.Participator)
                .ThenInclude(p => p!.Company)
                .Include(ep => ep.Participator)
                .ThenInclude(p => p!.Person)
                .Where(ep => ep.EventInfoId == id)
                .Select(ep => ep.Participator)
                .ToListAsync();
            
            Console.WriteLine("lengtgh: " + Participators.Count);

            return Page();
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(int participatorId, int eventInfoId)
        {
            var participator = await _context.Participators
                .Include(p => p.Person)
                .Include(p => p.Company)
                .FirstOrDefaultAsync(p => p.Id == participatorId);

            if (participator == null)
            {
                return NotFound();
            }
            
            if (participator.Person != null)
            {
                _context.Persons.Remove(participator.Person);
            }

            if (participator.Company != null)
            {
                _context.Companies.Remove(participator.Company);
            }

            _context.Participators.Remove(participator);
            await _context.SaveChangesAsync();

            return RedirectToPage("/EventInfos/Details", new { id = eventInfoId });
        }
    }
}