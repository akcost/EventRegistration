using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.EventInfos;

public class AddParticipator : PageModel
{
    private readonly DAL.AppDbContext _context;

    public AddParticipator(AppDbContext context)
    {
        _context = context;
    }

    public EventInfo? EventInfo { get; set; } = default!;
    
    [BindProperty] public Person Person { get; set; } = default!;
    [BindProperty] public Company Company { get; set; } = default!;
    [BindProperty] public PaymentType PaymentType { get; set; }


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
        
        if(EventInfo.EventDateTime < DateTime.Now)
        {
            return NotFound();
        }

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int eventInfoId)
    {

        EventInfo = await _context.EventInfos.FirstOrDefaultAsync(m => m.Id == eventInfoId);

        if (EventInfo == null)
        {
            return NotFound();
        }
        else if(EventInfo.EventDateTime < DateTime.Now)
        {
            return NotFound();
        }

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


        return RedirectToPage("/Index");

    }
}