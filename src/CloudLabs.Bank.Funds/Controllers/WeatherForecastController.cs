using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace CloudLabs.Bank.Funds.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ServiceBusSender sender;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ServiceBusSender _sender, ILogger<WeatherForecastController> logger)
    {
        sender = _sender;
        _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> TestGet()
    {
        await sender.SendMessageAsync(new ServiceBusMessage("Test message from Funds service"));
        return Ok(Task.FromResult(""));
    }

}