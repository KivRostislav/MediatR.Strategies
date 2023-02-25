namespace MediatR.Strategies.Tests;

public class Logger
{
    public readonly List<string> Logs;

    public Logger() => Logs = new();
}
