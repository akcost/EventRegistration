using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            return Page();
        }

        // [BindProperty] public Participator Participator { get; set; } = default!;
        [BindProperty] public Person Person { get; set; } = default!;
        [BindProperty] public Company Company { get; set; } = default!;
        [BindProperty] public PaymentType PaymentType { get; set; }

        public Boolean IsPerson { get; set; } = true;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            // Check if Person info was submitted or Company info
            if (Person?.PersonFirstName == null)
            {
                _context.Companies.Add(Company!);
                await _context.SaveChangesAsync();

                Participator participator = new Participator()
                {
                    Company = Company,
                    CompanyId = Company!.Id,
                    PaymentType = PaymentType,
                };
                _context.Participators.Add(participator);
            }
            else
            {
                _context.Persons.Add(Person);
                await _context.SaveChangesAsync();

                Participator participator = new Participator()
                {
                    Person = Person,
                    PersonId = Person.Id,
                    PaymentType = PaymentType,
                };
                _context.Participators.Add(participator);
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}