using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services;

public class RedisService
{
    private readonly string _redisHost;
    private readonly string _redisPort;

    private ConnectionMultiplexer _redis;

    public IDatabase redisDatabase { get; set; }

    public RedisService(IConfiguration configuration)
    {
        _redisHost = configuration["Redis:Host"];
        _redisHost = configuration["Redis:Port"];
    }

    public void Connect () 
    {
        var configSting = $"{_redisHost}:{_redisPort}";

        _redis = ConnectionMultiplexer.Connect(configSting);

    }

    public IDatabase GetRedisDb(int redisDb)
    {
        return _redis.GetDatabase(redisDb);
    }
}
