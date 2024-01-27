using ExampleAPI.Entities;
using ExampleAPI.Repositories.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Controllers;

public class CardTypeController : Controller
{
    private ICardTypeRepository _cardTypeRepository;

    public CardTypeController(ICardTypeRepository cardTypeRepository)
    {
        _cardTypeRepository = cardTypeRepository;
    }

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        return Ok(_cardTypeRepository.GetAll());
    }

    [HttpGet("GetById/{id}")]
    public IActionResult Get(Guid id)
    {
        return Ok(_cardTypeRepository.Get(cardType => cardType.Id == id));
    }

    [HttpPost("Add")]
    public IActionResult Add([FromBody] CardType cardType)
    {
        return Ok(_cardTypeRepository.Add(cardType));
    }

    [HttpPut("Update")]
    public IActionResult Update([FromBody] CardType cardType)
    {
        return Ok(_cardTypeRepository.Update(cardType));
    }

    [HttpDelete("DeleteById/{id}")]
    public IActionResult Delete(Guid id)
    {
        var cardType = _cardTypeRepository.Get(cardType => cardType.Id == id);
        if (cardType == null) return BadRequest("Category not found");
        return Ok(_cardTypeRepository.Delete(cardType));
    }
}