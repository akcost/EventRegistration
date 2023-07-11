using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages.Participators
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
        ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "CompanyName");
        ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "PersonFirstName");
            return Page();
        }

        [BindProperty]
        public Participator Participator { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Participators == null || Participator == null)
            {
                return Page();
            }

            _context.Participators.Add(Participator);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
