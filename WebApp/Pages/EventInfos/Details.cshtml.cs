using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

            return Page();
        }
        
        [BindProperty] public Person Person { get; set; } = default!;
        [BindProperty] public Company Company { get; set; } = default!;
        [BindProperty] public PaymentType PaymentType { get; set; }

        public Boolean IsPerson { get; set; } = true;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int eventInfoId)
        {

            EventInfo = await _context.EventInfos.FirstOrDefaultAsync(m => m.Id == eventInfoId);

            Console.WriteLine();
            Participator participator;
            // Check if Person info was submitted or Company info
            if (Person?.PersonFirstName == null)
            {
                _context.Companies.Add(Company!);
                await _context.SaveChangesAsync();

                participator = new Participator()
                {
                    Company = Company,
                    CompanyId = Company!.Id,
                    PaymentType = PaymentType,
                };
            }
            else
            {
                _context.Persons.Add(Person);
                await _context.SaveChangesAsync();

                participator = new Participator()
                {
                    Person = Person,
                    PersonId = Person.Id,
                    PaymentType = PaymentType,
                };
            }
            
            _context.Participators.Add(participator);
            await _context.SaveChangesAsync();

            EventParticipator eventParticipator = new EventParticipator()
            {
                EventInfo = EventInfo,
                EventInfoId = EventInfo!.Id,
                Participator = participator,
                ParticipatorId = participator.Id,
            };
            _context.EventParticipators.Add(eventParticipator);
            await _context.SaveChangesAsync();


            return RedirectToPage("/EventInfos/Details", new { id = eventInfoId });

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