namespace Api.Entities.Common;

public interface ILiverpoolEntity<TDto>
{
    TDto ToDto();
}