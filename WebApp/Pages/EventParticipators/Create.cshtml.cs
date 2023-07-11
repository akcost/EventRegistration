using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages.EventParticipators
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["EventInfoId"] = new SelectList(_context.EventInfos, "Id", "EventName");
        ViewData["ParticipatorId"] = new SelectList(_context.Participators, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public EventParticipator EventParticipator { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.EventParticipators == null || EventParticipator == null)
            {
                return Page();
            }

            _context.EventParticipators.Add(EventParticipator);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
