using ExampleAPI.Contexts;
using ExampleAPI.Core;
using ExampleAPI.Entities;
using ExampleAPI.Repositories.Abstracts;

namespace ExampleAPI.Repositories.Concretes;

public class CardRepository : BaseRepository<Card>, ICardRepository
{
    public CardRepository(ExampleDbContext context) : base(context)
    {
    
    }
}