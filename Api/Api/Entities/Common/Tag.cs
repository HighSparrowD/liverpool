namespace Api.Entities.Common;

public record Tag : ILiverpoolEntity<Models.Common.Tag>
{
    public required long Id { get; set; }

    public required string Text { get; set; }
    
    public Models.Common.Tag ToDto()
    {
        return new Models.Common.Tag
        {
            Id = this.Id,
            Text = this.Text,
        };
    }
}