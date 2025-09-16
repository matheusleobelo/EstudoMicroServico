using System;

namespace MeuServicoApi.Models;

public class EventMessage
{
    public string Message { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public string Origin { get; set; } = default!;
}