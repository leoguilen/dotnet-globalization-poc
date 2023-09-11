namespace GlobalizationDemo.Models;

internal record GreetingRequest
{
    [FromRoute]
    public required string Name { get; init; }

    [FromServices]
    public required IStringLocalizerFactory LocalizerFactory { get; init; }
}