using Microsoft.EntityFrameworkCore;
using Sms.Entity;


namespace Sms;

public class SmsDbContext : DbContext
{
    public DbSet<LodgingReservation> LodgingReservations { get; set; }
    public DbSet<ItineraryArrival> ItineraryArrivals { get; set; }
    public DbSet<ItineraryHistory> ItineraryHistories { get; set; }
    public DbSet<LookupCode> LookupCodes { get; set; }
    public DbSet<ItineraryHistoryReservationId> ItineraryHistoryReservationIds { get; set; }
    public DbSet<ItineraryHistoryReservation> ItineraryHistoryReservations { get; set; }
    public DbSet<ActivityReservation> ActivityReservations { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<ItineraryHistoryGuest> ItineraryHistoryGuests { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<LodgingReservationRatePlanPolicy> LodgingReservationRatePlanPolicies { get; set; }
    public DbSet<LodgingReservationNote> LodgingReservationNotes { get; set; }
    public DbSet<SourceOfBusiness> SourcesOfBusiness { get; set; }
    public DbSet<Misc> MiscDescriptions { get; set; }
    public DbSet<LodgingReservationSpecialBilling> LodgingReservationSpecialBillingCodes { get; set; }
    public DbSet<LodgingUnit> LodgingUnits { get; set; }
    public DbSet<LodgingReservationTransaction> LodgingReservationTransactions { get; set; }

    public DbSet<LetterRequest> LetterRequests { get; set; }
    public DbSet<LetterHistoryItinerary> LetterHistoryItineraries { get; set; }

    public DbSet<LetterHistoryItineraryReservation> LetterHistoryItineraryReservations { get; set; }
    public DbSet<SmsIntegration> SmsIntegrations { get; set; }

    public SmsDbContext(DbContextOptions<SmsDbContext> options)
            : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LodgingReservation>()
            .HasNoKey();

        modelBuilder.Entity<LodgingReservationSpecialBilling>()
    .HasNoKey();

        modelBuilder.Entity<LodgingReservationNote>()
    .HasNoKey();

        modelBuilder.Entity<LodgingReservationTransaction>()
    .HasNoKey();

        modelBuilder.Entity<LodgingUnit>()
.HasNoKey();

        modelBuilder.Entity<Misc>()
    .HasNoKey();
        modelBuilder.Entity<ActivityReservation>()
            .HasNoKey();
        modelBuilder.Entity<SourceOfBusiness>()
          .HasNoKey();
        modelBuilder.Entity<User>()
            .HasNoKey();

        modelBuilder.Entity<Guest>()
            .HasNoKey();

        modelBuilder.Entity<LodgingReservationRatePlanPolicy>()
            .HasNoKey();

        modelBuilder.Entity<ItineraryArrival>()
            .ToTable("ItineraryArrival", "HA");

        modelBuilder.Entity<ItineraryHistory>()
            .ToTable("ItineraryHistory", "HA");

        modelBuilder.Entity<LookupCode>()
            .HasNoKey();

        modelBuilder.Entity<ItineraryHistoryReservationId>()
            .HasNoKey();

        modelBuilder.Entity<ItineraryHistoryReservation>()
            .ToTable("ItineraryHistoryReservation", "HA");

        modelBuilder.Entity<ItineraryHistoryReservation>()
            .ToTable("ItineraryHistoryGuest", "HA");


        modelBuilder.Entity<LetterRequest>()
.ToTable("LetterRequest", "HA");

        modelBuilder.Entity<LetterHistoryItinerary>()
.ToTable("LetterHistoryItinerary", "HA");


        modelBuilder.Entity<LetterHistoryItineraryReservation>()
.ToTable("LetterHistoryItineraryReservation", "HA");

        modelBuilder.Entity<SmsIntegration>()
            .ToTable("SmsIntegration", "HA");

    }
}
