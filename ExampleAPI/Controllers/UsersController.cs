using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleAPI.DTOs;
using ExampleAPI.Entities;
using ExampleAPI.Repositories.Abstracts;
using ExampleAPI.Repositories.Concretes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurkeyServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExampleAPI.Controllers;

[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountTransactionRepository _accountTransactionRepository;
    private readonly ICardRepository _cardRepository;

    public UsersController(
        IUserRepository userRepository,
        IAccountTransactionRepository accountTransactionRepository,
        ICardRepository cardRepository
    )
    {
        _userRepository = userRepository;
        _accountTransactionRepository = accountTransactionRepository;
        _cardRepository = cardRepository;
    }

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        return Ok(_userRepository.GetAll());
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var user = _userRepository.Get(user => user.Id == id);

        using (KPSPublicSoapClient soapClient = new(endpointConfiguration: KPSPublicSoapClient.EndpointConfiguration.KPSPublicSoap12))
        {
            if (user != null)
            {
                var result = await soapClient.TCKimlikNoDogrulaAsync(
                    long.Parse(user.IdentificationNumber),
                    user.FirstName,
                    user.LastName,
                    user.BirthYear
                );

                if (result.Body.TCKimlikNoDogrulaResult)
                {
                    return Ok(user);
                }
            }
        }

        return BadRequest("ID information is invalid.");
    }

    [HttpGet("GetAllWithBalanceTransactions")]
    public IActionResult GetAllWithBalanceTransactions()
    {
        return Ok(_userRepository.GetAll(
            include: user => user.Include(u => u.AccountTransactions)
        ));
    }

    [HttpGet("GetAllWithOrders")]
    public IActionResult GetAllWithOrders()
    {
        return Ok(_userRepository.GetAll(
            include: user => user
                    .Include(u => u.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.ProductTransaction)
                    .Include(u => u.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.Category)
            ));
    }

    [HttpGet("GetAllWithAllDetails")]
    public IActionResult GetAllWithAllDetails()
    {
        return Ok(_userRepository.GetAll(
            include: user => user
                    .Include(u => u.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.ProductTransaction)
                    .Include(u => u.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.Category)
                    .Include(u => u.AccountTransactions)
            ));
    }

    [HttpGet("GetAllWithCards")]
    public IActionResult GetAllWithCards()
    {
        return Ok(_userRepository.GetAll(
            include: user => user.Include(u => u.Cards)
        ));
    }

    [HttpPost("Add")]
    public IActionResult Add([FromBody] User user)
    {
        return Ok(_userRepository.Add(user));
    }

    [HttpPost("AddCard")]
    public IActionResult AddCard(AddCardDto addCardDto)
    {
        Card card = new()
        {
            UserId = addCardDto.UserId,
            CardTypeId = addCardDto.CardTypeId,
            CardUID = addCardDto.CardUID,
            Limit = addCardDto.Limit
        };

        return Ok(_cardRepository.Add(card));
    }

    [HttpPost("AddBalance")]
    public IActionResult Add([FromBody] AccountTransaction accountTransaction)
    {
        return Ok(_accountTransactionRepository.Add(accountTransaction));
    }

    [HttpPut("Update")]
    public IActionResult Update([FromBody] User user)
    {
        return Ok(_userRepository.Update(user));
    }

    [HttpDelete("DeleteById/{id}")]
    public IActionResult Delete(Guid id)
    {
        var user = _userRepository.Get(user => user.Id == id);
        if (user == null) return BadRequest("User not found");
        return Ok(_userRepository.Delete(user));
    }
}

