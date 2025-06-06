using Chat.Infrastructure.Abstractions.Auth;
using Chat.Infrastructure.ServiceRegistration.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Chat.Infrastructure.Auth;

public class TokenStorage(IDistributedCache redisCache, IOptions<JwtOptions> options) :
    ITokenStorage
{
    private const string keyFormat = "refresh-token:{0}";

    private readonly IDistributedCache redisCache = redisCache;
    private readonly TimeSpan tokenLifetime = TimeSpan.FromHours(options.Value.RefreshTokenExpirationHours);

    public async Task<(bool success, long? id)> GetUserIdAsync(string token, CancellationToken cancellationToken)
    {
        var key = string.Format(keyFormat, token);
        var storedId = await redisCache.GetStringAsync(key, cancellationToken);

        if (long.TryParse(storedId, out var userId))
            return (true, userId);

        return (false, null);
    }

    public async Task SetTokenAsync(string token, long userId, CancellationToken cancellationToken)
    {
        var key = string.Format(keyFormat, token);
        await redisCache.SetStringAsync(key, userId.ToString(), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = tokenLifetime
        }, cancellationToken);
    }

    public async Task DeleteTokenAsync(string token, CancellationToken cancellationToken)
    {
        var key = string.Format(keyFormat, token);
        await redisCache.RemoveAsync(key, cancellationToken);
    }
}