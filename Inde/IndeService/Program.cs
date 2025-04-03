using Serilog;
using IndeService;
using IndeService.Repository;
using IndeService.Service;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Sms;
using Sms.Adapter;
using Sms.Repository;
using Sms.Service;



var builder = Host.CreateApplicationBuilder(args);


var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var log = logger.ForContext(typeof(Program));
builder.Services.AddSingleton(ApplicationOptions.GetConfigurationSettings(builder.Configuration));
builder.Services.AddTransient<MyTokenHandler>();
builder.Services.AddTransient<IMyTokenService,MyTokenService>();

builder.Services.AddWindowsService();
builder.Services.AddHostedService<Worker>();

builder.Services.AddHttpClient<IMcLoginRepository, McLoginRepository>(client =>
{
    client.BaseAddress = new Uri("https://insight-qa.inntopia.com/securityapi/v1/login");
    client.Timeout = new TimeSpan(0, 0, 60);
    client.DefaultRequestHeaders.Add("ClientId", "0");
    client.DefaultRequestHeaders.Add("UserId", "IntegrationApi");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", builder.Configuration["MarketingCloud:Authorization"]);
}); 

builder.Services.AddHttpClient<IIntegrationInstanceRepository, IntegrationInstanceRepository>(client =>
{
    client.BaseAddress = new Uri("https://insight-qa.inntopia.com/integrationapi/v2/hostadapter/configuration/adapterconfiguration");
    client.Timeout = new TimeSpan(0, 0, 60);
    client.DefaultRequestHeaders.Add("ClientId", "0");
    client.DefaultRequestHeaders.Add("UserId", "IntegrationApi");
    client.DefaultRequestHeaders.Add("HostAdapterInstanceId", "1");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", builder.Configuration["MarketingCloud:Authorization"]);
}).AddHttpMessageHandler<MyTokenHandler>();

builder.Services.AddSingleton<IMcMemoryCache<McUser>, McMemoryCache<McUser>>();
builder.Services.AddTransient<IIntegrationService, IntegrationService>();

////builder.Configuration.AddMcConfiguration(builder.Configuration.GetSection("MarketingCloud"), log);
//builder.Services.AddTransient<ILodgingReservationRepository, LodgingReservationRepository>();
//builder.Services.AddTransient<IItineraryArrivalRepository, ItineraryArrivalRepository>();
//builder.Services.AddTransient<IItineraryHistoryRepository, ItineraryHistoryRepository>();
//builder.Services.AddTransient<ILookupCodeRepostitory, LookupCodeRepostitory>();
//builder.Services.AddTransient<IItineraryHistoryReservationRepository, ItineraryHistoryReservationRepository>();
//builder.Services.AddTransient<IActivityReservationRepository, ActivityReservationRepository>();
//builder.Services.AddTransient<IItineraryArrivalService, ItineraryArrivalService>();
//builder.Services.AddTransient<IItineraryHistoryGuestRepository, ItineraryHistoryGuestRepository>();
//builder.Services.AddTransient<IGuestRepository, GuestRepository>();
//builder.Services.AddTransient<IMiscDescriptionRepository, MiscDescriptionRepository>();
//builder.Services.AddTransient<ILodgingReservationSpecialBillingRepostitory, LodgingReservationSpecialBillingRepostitory>();
//builder.Services.AddTransient<ILodgingReservationNoteRepostitory, LodgingReservationNoteRepostitory>();
//builder.Services.AddTransient<ILodgingReservationRatePlanPolicyRepostitory, LodgingReservationRatePlanPolicyRepostitory>();
//builder.Services.AddTransient<ILodgingUnitRepostitory, LodgingUnitRepostitory>();
//builder.Services.AddTransient<ISourceOfBusinessRepostitory, SourceOfBusinessRepostitory>();
//// services.AddTransient<ICustomerBuilder, CustomerBuilder>();
//builder.Services.AddTransient<ICustomerBuilderFactory, CustomerBuilderFactory>();
//builder.Services.AddTransient<IUserRepostitory, UserRepostitory>();
//builder.Services.AddTransient<ILodgingReservationTransactionRepostitory, LodgingReservationTransactionRepostitory>();
////services.AddTransient<ILodgingReservationBuilder, LodgingReservationBuilder>();
//builder.Services.AddTransient<ILodgingReservationDirector, LodgingReservationDirector>();
//builder.Services.AddTransient<ILodgingReservationBuilderFactory, LodgingReservationBuilderFactory>();
//builder.Services.AddTransient<IActivityReservationDirector, ActivityReservationDirector>();
//builder.Services.AddTransient<IActivityReservationBuilderFactory, ActivityReservationBuilderFactory>();

//builder.Services.AddTransient<ILetterService, LetterService>();
//builder.Services.AddTransient<ILetterRequestRepository, LetterRequestRepository>();
//builder.Services.AddTransient<ILetterHistoryItineraryRepository, LetterHistoryItineraryRepository>();
//builder.Services.AddTransient<ILetterHistoryItineraryReservationRepository, LetterHistoryItineraryReservationRepository>();
//builder.Services.AddTransient<IItineraryDirector, ItineraryDirector>();
//builder.Services.AddTransient<IItineraryBuilderFactory, ItineraryBuilderFactory>();
//builder.Services.AddDbContext<SmsDbContext>(options => options.UseSqlServer("data source=192.168.1.2;database=inntopia;user id=sa;password=tiger123$;Connection Timeout=120;Encrypt=False;"));

builder.Services.AddMyLibraryService(builder.Configuration);

IHost host = builder.Build();
host.Run();