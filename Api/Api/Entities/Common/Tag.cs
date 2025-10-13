namespace Api.Entities.Common;

public record Tag
{
    public required long Id { get; set; }

    public required string Text { get; set; }
}