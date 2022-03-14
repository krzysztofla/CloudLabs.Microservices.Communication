using Azure.Messaging.ServiceBus;
using CloudLabs.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CloudLabs.Store.Cart.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly ServiceBusSender _sender;

    private readonly ServiceBusClient _client;


    private readonly ILogger<CartController> _logger;

    public CartController(ServiceBusClient client, ILogger<CartController> logger)
    {
        _logger = logger;
        _client = client;
        _sender = _client.CreateSender("CloudLabs.Bank.Orders"); ;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder()
    {

        await _sender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(new CreateOrder(007, "Krzysztof", 1230))));

        return Ok();
    }
}