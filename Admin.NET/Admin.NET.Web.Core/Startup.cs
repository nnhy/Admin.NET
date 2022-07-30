﻿using Admin.NET.Core;
using AspNetCoreRateLimit;
using Furion;
using Furion.SpecificationDocument;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnceMi.AspNetCore.OSS;
using System;
using Yitter.IdGenerator;

namespace Admin.NET.Web.Core;

public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 配置选项
        services.AddProjectOptions();
        // ORM-SqlSugar
        services.AddSqlSugarSetup();
        // JWT
        services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);
        // 允许跨域
        services.AddCorsAccessor();
        // 远程请求
        services.AddRemoteRequest();
        // 任务调度
        services.AddTaskScheduler();
        // 脱敏检测
        services.AddSensitiveDetection();
        // 结果拦截器
        services.AddMvcFilter<ResultFilter>();
        // 日志监听特性（拦截器）
        services.AddMonitorLogging();

        services.AddControllersWithViews()
            .AddAppLocalization()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 响应驼峰命名
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
                // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; // 忽略空值
            })
            .AddInjectWithUnifyResult<AdminResultProvider>();

        // 限流服务
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        // 事件总线
        services.AddEventBus(builder =>
        {
            builder.AddSubscriber<LogEventSubscriber>();
        });
        // OSS对象存储
        services.AddOSSService(options =>
        {
            options = App.GetOptions<OSSProviderOptions>();
        });
        // 电子邮件
        services.AddMailKit(options =>
        {
            options.UseMailKit(App.GetOptions<EmailOptions>());
        });

        // Redis缓存
        services.AddCSRedisSetup();

        // 模板引擎
        services.AddViewEngine();

        // 即时通讯
        services.AddSignalR();

        // logo显示
        services.AddLogoDisplay();

        // 日志记录
        services.AddLogging(builder =>
        {
            // 每天创建一个日志文件（消息日志、错误日志、警告日志）
            builder.AddFile("logs/{0:yyyyMMdd}_inf.log", options =>
            {
                options.WriteFilter = (logMsg) =>
                {
                    return logMsg.LogLevel == LogLevel.Information;
                };
                options.FileNameRule = fileName =>
                {
                    return string.Format(fileName, DateTime.Now);
                };
                options.FileSizeLimitBytes = 10 * 1024;
                options.MaxRollingFiles = 30;
            });
            builder.AddFile("logs/{0:yyyyMMdd}_err.log", options =>
            {
                options.WriteFilter = (logMsg) =>
                {
                    return logMsg.LogLevel == LogLevel.Error;
                };
                options.FileNameRule = fileName =>
                {
                    return string.Format(fileName, DateTime.Now);
                };
                options.FileSizeLimitBytes = 10 * 1024;
                options.MaxRollingFiles = 30;
            });
            builder.AddFile("logs/{0:yyyyMMdd}_wrn.log", options =>
            {
                options.WriteFilter = (logMsg) =>
                {
                    return logMsg.LogLevel == LogLevel.Warning;
                };
                options.FileNameRule = fileName =>
                {
                    return string.Format(fileName, DateTime.Now);
                };
                options.FileSizeLimitBytes = 10 * 1024;
                options.MaxRollingFiles = 30;
            });

            // 日志写入数据库
            builder.AddDatabase<DbLoggingWriter>(options =>
            {
                options.MinimumLevel = LogLevel.Information;
            });
        });

        // 设置雪花Id算法机器码
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions
        {
            WorkerId = App.GetOptions<SnowIdOptions>().WorkerId
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // 添加状态码拦截中间件
        app.UseUnifyResultStatusCodes();

        // 配置多语言
        app.UseAppLocalization();

        // 启用HTTPS
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseCorsAccessor();

        // 限流组件（在跨域之后）
        app.UseIpRateLimiting();
        app.UseClientRateLimiting();

        app.UseAuthentication();
        app.UseAuthorization();

        // 配置Swagger-Knife4UI（路由前缀一致代表独立版本配置）
        app.UseKnife4UI(options =>
        {
            options.RoutePrefix = string.Empty;
            foreach (var groupInfo in SpecificationDocumentBuilder.GetOpenApiGroups())
            {
                options.SwaggerEndpoint("/" + groupInfo.RouteTemplate, groupInfo.Title);
            }
        });

        app.UseInject(string.Empty);

        app.UseEndpoints(endpoints =>
        {
            // 注册集线器
            endpoints.MapHubs();

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}