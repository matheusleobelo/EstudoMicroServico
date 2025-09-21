using Microsoft.AspNetCore.Mvc;
using MassTransit;
using MeuOutroServicoApi.Models;

namespace MeuOutroServicoApi.Controllers;

[ApiController]
[Route("events")]
public class EventsController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventsController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost("publish")]
    public async Task<IActionResult> Publish([FromBody] EventMessage message)
    {
        message.Timestamp = DateTime.UtcNow;
        message.Origin = "MeuOutroServicoApi";

        await _publishEndpoint.Publish(message);

        return Accepted(new { status = "published", message });
    }

    [HttpGet("publish-test")]
    public async Task<IActionResult> PublishTest()
    {
        var message = new EventMessage
        {
            Origin = "MeuOutroServicoApi",
            Message = "Mensagem de teste rápida"
        };

        await _publishEndpoint.Publish(message);
        return Ok(new { status = "published via GET", message });
    }
}
