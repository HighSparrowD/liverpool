using StackExchange.Redis;

namespace Api.Services.Redis;

public interface ILiverpoolRedis
{
    IDatabase GetDatabase();
}