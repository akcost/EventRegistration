namespace Domain;

public class Participator
{
    public int Id { get; set; }
    
    public int? PersonId { get; set; }
    public Person? Person { get; set; }
    public int? CompanyId { get; set; }
    public Company? Company { get; set; }

    public PaymentType? PaymentType { get; set; }

}