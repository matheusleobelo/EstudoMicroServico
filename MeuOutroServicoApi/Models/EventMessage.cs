using System;

namespace MeuOutroServicoApi.Models;

public class EventMessage
{
    public string Message { get; set; } = default!;
    public string Origin { get; set; } = default!;
    public DateTime Timestamp { get; set; }
}
