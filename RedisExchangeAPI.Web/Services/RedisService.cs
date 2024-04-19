using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services;

public class RedisService
{
    private readonly string _redisHost;
    private readonly string _redisPort;
    private ConnectionMultiplexer _redis;

    public IDatabase RedisDatabase { get; private set; }

    public RedisService(IConfiguration configuration)
    {
        _redisHost = configuration["Redis:Host"];
        _redisPort = configuration["Redis:Port"];
    }

    public void Connect()
    {
        var configString = $"{_redisHost}:{_redisPort}";
        _redis = ConnectionMultiplexer.Connect(configString);
        RedisDatabase = _redis.GetDatabase();
    }

    public IDatabase GetRedisDb(int redisDb)
    {
        return _redis.GetDatabase(redisDb);
    }
}
