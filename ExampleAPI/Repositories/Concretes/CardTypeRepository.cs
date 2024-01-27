using ExampleAPI.Contexts;
using ExampleAPI.Core;
using ExampleAPI.Entities;
using ExampleAPI.Repositories.Abstracts;

namespace ExampleAPI.Repositories.Concretes;

public class CardTypeRepository : BaseRepository<CardType>, ICardTypeRepository
{
    public CardTypeRepository(ExampleDbContext context) : base(context)
    {
    
    }
}