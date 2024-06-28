// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using NewLife.Caching.Queues;

namespace Admin.NET.Core;

/// <summary>
/// Redis 消息队列
/// </summary>
public static class RedisQueue
{
    //private static ICache _cache = App.GetService<ICache>();
    private static ICacheProvider _cacheProvider = App.GetService<ICacheProvider>();

    /// <summary>创建Redis消息队列。默认消费一次，指定消费者group时使用STREAM结构，支持多消费组共享消息</summary>
    /// <remarks>
    /// 使用队列时，可根据是否设置消费组来决定使用简单队列还是完整队列。 简单队列（如RedisQueue）可用作命令队列，Topic很多，但几乎没有消息。 完整队列（如RedisStream）可用作消息队列，Topic很少，但消息很多，并且支持多消费组。
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="topic">主题</param>
    /// <param name="group">消费组。未指定消费组时使用简单队列（如RedisQueue），指定消费组时使用完整队列（如RedisStream）</param>
    /// <returns></returns>
    public static IProducerConsumer<T> GetQueue<T>(String topic, String group = null)
    {
        // 队列需要单列
        var key = $"myStream:{topic}";
        if (_cacheProvider.InnerCache.TryGetValue<IProducerConsumer<T>>(key, out var queue)) return queue;

        queue = _cacheProvider.GetQueue<T>(topic, group);
        _cacheProvider.Cache.Set(key, queue);

        return queue;
    }

    /// <summary>
    /// 获取可信队列，需要确认
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="topic"></param>
    /// <returns></returns>
    public static RedisReliableQueue<T> GetRedisReliableQueue<T>(string topic)
    {
        //var queue = (_cache as FullRedis).GetReliableQueue<T>(topic);
        //return queue;

        // 队列需要单列
        var key = $"myQueue:{topic}";
        if (_cacheProvider.InnerCache.TryGetValue<RedisReliableQueue<T>>(key, out var queue)) return queue;

        queue = (_cacheProvider.Cache as FullRedis).GetReliableQueue<T>(topic);
        _cacheProvider.Cache.Set(key, queue);

        return queue;
    }

    /// <summary>
    /// 可信队列回滚
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="retryInterval"></param>
    /// <returns></returns>
    public static int RollbackAllAck(string topic, int retryInterval = 60)
    {
        var queue = GetRedisReliableQueue<string>(topic);
        queue.RetryInterval = retryInterval;
        return queue.RollbackAllAck();
    }

    /// <summary>
    /// 发送一个数据列表到可信队列
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int AddReliableQueueList<T>(string topic, List<T> value)
    {
        //var queue = (_cache as FullRedis).GetReliableQueue<T>(topic);
        var queue = GetRedisReliableQueue<T>(topic);
        var count = queue.Count;
        var result = queue.Add(value.ToArray());
        return result - count;
    }

    /// <summary>
    /// 发送一条数据到可信队列
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int AddReliableQueue<T>(string topic, T value)
    {
        //var queue = (_cache as FullRedis).GetReliableQueue<T>(topic);
        var queue = GetRedisReliableQueue<T>(topic);
        var count = queue.Count;
        var result = queue.Add(value);
        return result - count;
    }

    /// <summary>
    /// 获取延迟队列
    /// </summary>
    /// <param name="topic"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static RedisDelayQueue<T> GetDelayQueue<T>(string topic)
    {
        //var queue = (_cache as FullRedis).GetDelayQueue<T>(topic);
        //return queue;

        // 队列需要单列
        var key = $"myDelay:{topic}";
        if (_cacheProvider.InnerCache.TryGetValue<RedisDelayQueue<T>>(key, out var queue)) return queue;

        queue = (_cacheProvider.Cache as FullRedis).GetDelayQueue<T>(topic);
        _cacheProvider.Cache.Set(key, queue);

        return queue;
    }

    /// <summary>
    /// 发送一条数据到延迟队列
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="value"></param>
    /// <param name="delay">延迟时间。单位秒</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int AddDelayQueue<T>(string topic, T value, int delay)
    {
        var queue = GetDelayQueue<T>(topic);
        return queue.Add(value, delay);
    }

    /// <summary>
    /// 发送数据列表到延迟队列
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="value"></param>
    /// <param name="delay"></param>
    /// <typeparam name="T">延迟时间。单位秒</typeparam>
    /// <returns></returns>
    public static int AddDelayQueue<T>(string topic, List<T> value, int delay)
    {
        var queue = GetDelayQueue<T>(topic);
        queue.Delay = delay;
        return queue.Add(value.ToArray());
    }

    /// <summary>
    /// 在可信队列获取一条数据
    /// </summary>
    /// <param name="topic"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReliableTakeOne<T>(string topic)
    {
        var queue = GetRedisReliableQueue<T>(topic);
        return queue.TakeOne(1);
    }

    /// <summary>
    /// 异步在可信队列获取一条数据
    /// </summary>
    /// <param name="topic"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T> ReliableTakeOneAsync<T>(string topic)
    {
        var queue = GetRedisReliableQueue<T>(topic);
        return await queue.TakeOneAsync(1);
    }

    /// <summary>
    /// 在可信队列获取多条数据
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="count"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> ReliableTake<T>(string topic, int count)
    {
        var queue = GetRedisReliableQueue<T>(topic);
        return queue.Take(count).ToList();
    }
}