namespace Domain;

public class EventParticipator
{
    public int Id { get; set; }
    
    public int EventInfoId { get; set; }
    public EventInfo? EventInfo { get; set; }

    public int ParticipatorId { get; set; }
    public Participator? Participator { get; set; }
}