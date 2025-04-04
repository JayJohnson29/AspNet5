using Serilog;
using IndeService;
using IndeService.Repository;
using IndeService.Service;
using System.Net.Http.Headers;
using Sms;


var builder = Host.CreateApplicationBuilder(args);


var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var log = logger.ForContext(typeof(Program));
//builder.Services.AddSingleton(ApplicationOptions.GetConfigurationSettings(builder.Configuration));
builder.Services.AddTransient<UserTokenHandler>();
builder.Services.AddTransient<IUserTokenService,UserTokenService>();

builder.Services.AddWindowsService();
builder.Services.AddHostedService<Worker>();

var baseUrl = builder.Configuration["MarketingCloud:BaseUrl"];

builder.Services.AddHttpClient<IMcLoginRepository, McLoginRepository>(client =>
{
    client.BaseAddress = new Uri($"{baseUrl}/securityapi/v1/login");
    client.Timeout = new TimeSpan(0, 0, 60);
    client.DefaultRequestHeaders.Add("ClientId", "0");
    client.DefaultRequestHeaders.Add("UserId", "Inde");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", builder.Configuration["MarketingCloud:Authorization"]);
}); 

builder.Services.AddHttpClient<IIntegrationInstanceRepository, IntegrationInstanceRepository>(client =>
{
    client.BaseAddress = new Uri($"{baseUrl}/integrationapi/v2/hostadapter/configuration/adapterconfiguration");
    client.Timeout = new TimeSpan(0, 0, 60);
    client.DefaultRequestHeaders.Add("ClientId", "0");
    client.DefaultRequestHeaders.Add("UserId", "Inde");
    client.DefaultRequestHeaders.Add("HostAdapterInstanceId", builder.Configuration["MarketingCloud:InstanceId"]);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", builder.Configuration["MarketingCloud:Authorization"]);
}).AddHttpMessageHandler<UserTokenHandler>();

builder.Services.AddHttpClient<IBatchFileRepository, BatchFileRepository>(client =>
{
    client.BaseAddress = new Uri($"{baseUrl}/batchapi");
    client.Timeout = new TimeSpan(0, 0, 60);
    client.DefaultRequestHeaders.Add("ClientId", "0");
    client.DefaultRequestHeaders.Add("UserId", "Inde");
    client.DefaultRequestHeaders.Add("HostAdapterInstanceId", builder.Configuration["MarketingCloud:InstanceId"]);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", builder.Configuration["MarketingCloud:Authorization"]);
}).AddHttpMessageHandler<UserTokenHandler>();

builder.Services.AddSingleton<IMcMemoryCache<McUser>, McMemoryCache<McUser>>();
builder.Services.AddTransient<IIntegrationService, IntegrationService>();
builder.Services.AddTransient<IBatchService, BatchService>();


builder.Services.AddSmsLibraryService(builder.Configuration);

IHost host = builder.Build();
host.Run();