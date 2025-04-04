using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sms.Adapter;
using Sms.Repository;
using Sms.Service;

namespace Sms;
public static class ServiceCollectionExtensions
{ 
    
    public static IServiceCollection AddSmsLibraryService(this IServiceCollection services, IConfiguration namedConfigurationSection)
    {
        // Default library options are overridden
        // by bound configuration values.
        //services.Configure<LibraryOptions>(namedConfigurationSection);

        // Register lib services here...
       // services.AddSingleton<AppConfig>(new AppConfig { ClientId = 224, SourceSystemCode = 7, CurrencyCode = "USD", PropertyName = "pebble", ResortName = "pebble"});

        services.AddTransient<ILodgingReservationRepository, LodgingReservationRepository>();
        services.AddTransient<IItineraryArrivalRepository, ItineraryArrivalRepository>();
        services.AddTransient<IItineraryHistoryRepository, ItineraryHistoryRepository>();
        services.AddTransient<ILookupCodeRepostitory, LookupCodeRepostitory>();
        services.AddTransient<IItineraryHistoryReservationRepository, ItineraryHistoryReservationRepository>();
        services.AddTransient<IActivityReservationRepository, ActivityReservationRepository>();
        services.AddTransient<IItineraryArrivalService, ItineraryArrivalService>();
        services.AddTransient<IItineraryHistoryGuestRepository, ItineraryHistoryGuestRepository>();
        services.AddTransient<IGuestRepository, GuestRepository>();
        services.AddTransient<IMiscDescriptionRepository, MiscDescriptionRepository>();
        services.AddTransient<ILodgingReservationSpecialBillingRepostitory, LodgingReservationSpecialBillingRepostitory>();
        services.AddTransient<ILodgingReservationNoteRepostitory, LodgingReservationNoteRepostitory>();
        services.AddTransient<ILodgingReservationRatePlanPolicyRepostitory, LodgingReservationRatePlanPolicyRepostitory>();
        services.AddTransient<ILodgingUnitRepostitory, LodgingUnitRepostitory>();
        services.AddTransient<ISourceOfBusinessRepostitory, SourceOfBusinessRepostitory>();
       // services.AddTransient<ICustomerBuilder, CustomerBuilder>();
        services.AddTransient<ICustomerBuilderFactory, CustomerBuilderFactory>();
        services.AddTransient<IUserRepostitory, UserRepostitory>();
        services.AddTransient<ILodgingReservationTransactionRepostitory, LodgingReservationTransactionRepostitory>();
        //services.AddTransient<ILodgingReservationBuilder, LodgingReservationBuilder>();
        services.AddTransient<ILodgingReservationDirector, LodgingReservationDirector>();
        services.AddTransient<ILodgingReservationBuilderFactory, LodgingReservationBuilderFactory>();
        services.AddTransient<IActivityReservationDirector, ActivityReservationDirector>();
        services.AddTransient<IActivityReservationBuilderFactory, ActivityReservationBuilderFactory>();

        services.AddTransient<ILetterService, LetterService>();
        services.AddTransient<ILetterRequestRepository, LetterRequestRepository>();
        services.AddTransient<ILetterHistoryItineraryRepository, LetterHistoryItineraryRepository>();

        services.AddTransient<ILetterHistoryItineraryReservationRepository, LetterHistoryItineraryReservationRepository>();
        services.AddTransient<IItineraryDirector, ItineraryDirector>();
        services.AddTransient<IItineraryBuilderFactory, ItineraryBuilderFactory>();
        services.AddTransient<IInstanceService, InstanceService>();
        services.AddTransient<IDatabaseRestoreService, DatabaseRestoreService>();
        services.AddTransient<ISmsIntegrationRepostitory, SmsIntegrationRepostitory>();


        //services.AddDbContext<SmsDbContext>(options => options.UseSqlServer("data source=192.168.1.2;database=inntopia;user id=sa;password=tiger123$;Connection Timeout=120;Encrypt=False;"));
        services.AddDbContext<SmsDbContext>(options => options.UseSqlServer("data source=localhost\\SQLEXPRESS;database=inntopia;user id=innde;password=redstar2!;integrated security=false;Connection Timeout=120;Encrypt=False;"));

        return services;
    }
}