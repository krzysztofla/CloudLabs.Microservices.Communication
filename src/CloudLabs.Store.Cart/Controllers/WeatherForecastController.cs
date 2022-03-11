using Azure.Messaging.ServiceBus;
using CloudLabs.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CloudLabs.Store.Cart.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ServiceBusSender _sender;


    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ServiceBusSender sender, ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder()
    {

        await _sender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(new CreateOrder(007, "Krzysztof", 1230))));

        return Ok();
    }
}