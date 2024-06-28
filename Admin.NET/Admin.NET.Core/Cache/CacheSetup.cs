// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Microsoft.Extensions.DependencyInjection.Extensions;
using NewLife.Caching.Services;

namespace Admin.NET.Core;

public static class CacheSetup
{
    /// <summary>
    /// 缓存注册（新生命Redis组件）
    /// </summary>
    /// <param name="services"></param>
    public static void AddCache(this IServiceCollection services)
    {
        ICache cache = Cache.Default;

        var cacheOptions = App.GetConfig<CacheOptions>("Cache", true);
        if (cacheOptions.CacheType == CacheTypeEnum.Redis.ToString())
        {
            var redis = new FullRedis(new RedisOptions
            {
                Configuration = cacheOptions.Redis.Configuration,
                Prefix = cacheOptions.Redis.Prefix
            });
            if (cacheOptions.Redis.MaxMessageSize > 0)
                redis.MaxMessageSize = cacheOptions.Redis.MaxMessageSize;

            // 注入Redis缓存提供者
            services.AddSingleton<ICacheProvider>(p => new RedisCacheProvider(p) { Cache = redis });
        }

        services.AddSingleton(cache);

        // 内存缓存兜底。在没有配置Redis时，使用内存缓存，逻辑代码无需修改
        services.TryAddSingleton<ICacheProvider, CacheProvider>();
    }
}