using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Company
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = default!;
    public string CompanyRegistryCode { get; set; } = default!;
    public int CompanyMemberCount { get; set; }
    [MaxLength(5000)]
    public string? CompanyDescription { get; set; }

}