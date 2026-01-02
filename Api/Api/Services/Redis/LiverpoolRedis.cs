using Api.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Api.Services.Redis;

public class LiverpoolRedis(IConnectionMultiplexer connectionMultiplexer, 
    IOptions<AppConfiguration> options) : ILiverpoolRedis
{
    public IDatabase GetDatabase()
    {
        return connectionMultiplexer.GetDatabase(options.Value.Redis.Database);
    }
}