using System.ComponentModel.DataAnnotations;

namespace Domain;

public class EventInfo
{
    public int Id { get; set; }
    public string EventName { get; set; } = default!;
    [MaxLength(1000)]
    public string? EventLocation { get; set; }
    public DateTime? EventDateTime { get; set; }
    public string? EventDescription { get; set; }
}