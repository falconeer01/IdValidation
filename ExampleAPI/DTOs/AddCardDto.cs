using ExampleAPI.Entities;

namespace ExampleAPI.DTOs;

public class AddCardDto
{
    public Guid UserId { get; set; }
    public Guid CardTypeId { get; set; }
    public long CardUID { get; set; }
    public decimal Limit { get; set; }
}
