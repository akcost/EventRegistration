using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Person
{
    public int Id { get; set; }
    public string PersonFirstName { get; set; } = default!;
    public string PersonLastName { get; set; } = default!;
    
    [MaxLength(11)]
    public string PersonIdCode { get; set; } = default!;
    
    [MaxLength(1500)]
    public string? PersonDescription { get; set; }
}