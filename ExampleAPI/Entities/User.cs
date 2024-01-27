using System;
using System.ComponentModel.DataAnnotations;
using ExampleAPI.Core;

namespace ExampleAPI.Entities;

public class User : Entity<Guid>
{
	[MaxLength(150)]
	public required string FirstName { get; set; }
	[MaxLength(150)]
	public required string LastName { get; set; }
	public short BirthYear { get; set; }
	[MaxLength(20)]
	public required string IdentificationNumber { get; set; }
	[MaxLength(11)]
	public string? CarPlate { get; set; }
	public virtual ICollection<Order> Orders { get; set; }
	public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }
	public virtual ICollection<Card> Cards { get; set; }
	public User()
	{
		Orders = new HashSet<Order>();
		AccountTransactions = new HashSet<AccountTransaction>();
		Cards = new HashSet<Card>();
	}
}
