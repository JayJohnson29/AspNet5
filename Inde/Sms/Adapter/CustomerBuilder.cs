using Microsoft.Extensions.Logging;
using Sms.MC;

namespace Sms.Adapter;

public class CustomerBuilder : ICustomerBuilder
{
    private readonly ILogger _logger;

    private Customer _customer;
    public Customer Customer { get { return _customer; } }
    public CustomerBuilder(ILogger<CustomerBuilder> logger, int clientId, int sourceSystemCode, Entity.Guest source)
    {
        _logger = logger;
        var doNotMail = source.nomail; // dr.HAGetString("nomail");
        var doNotPhone = source.nophone; // dr.HAGetString("nophone");
        var doNotEmail = source.noemail; // dr.HAGetString("noemail");
        var phone = source.phone; // dr.HAGetString("phone");
        var phoneNum = source.pphonenum; // dr.HAGetString("pphonenum");
        var title = source.title; // dr.HAGetString("title");
        var genderValue = source.gender; // dr.HAGetString("gender");

        var gender = CustomerGender.U;
        if (string.IsNullOrEmpty(genderValue))
            gender = CustomerGender.U;
        else if (genderValue.ToLower().Substring(0, 1) == "m")
            gender = CustomerGender.M;
        else if (genderValue.ToLower().Substring(0, 1) == "f")
            gender = CustomerGender.F;

        var phoneNumber = !string.IsNullOrEmpty(phoneNum) ? phoneNum : phone;

        var phoneProfileType = string.Empty;
        var phoneType = source.phonetype; // dr.HAGetString("phonetype");
        if (!string.IsNullOrEmpty(phoneType) && string.Equals(phoneType, "m", StringComparison.InvariantCultureIgnoreCase))
        {
            phoneProfileType = "Mobile";
        }

        _customer = new MC.Customer
        {
            CustomerKey = 0,
            Origin = "SMS",
            MatchOnly = false,
            MatchOnlySpecified = false,
            GuestType = CustomerGuestType.FIT,
            Title = title.Replace("&", "and"),
            FirstName = source.first, // dr.HAGetString("first"),
            LastName = source.last, // dr.HAGetString("last"),
            Company = string.Empty,
            Birthdate = source.birthday ?? new DateTime(1900, 1, 1), // dr.HAGetDateTime("birthday"),
            Gender = gender,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            Addresses = new CustomerAddress[]
            {
                                new CustomerAddress
                                {
                                    AddressKey = 0,
                                    ProfileType = string.Empty,
                                    Score = "N",
                                    ScoreDate = DateTime.Now,
                                    Line1 = source.address1, // dr.HAGetString("address1"),
                                    Line2 = source.address2, //  dr.HAGetString("address2"),
                                    Line3 = source.address3, // dr.HAGetString("address3"),
                                    City = source.city, // dr.HAGetString("city"),
                                    State = source.state, // dr.HAGetString("state"),
                                    PostalCode = source.zip, // dr.HAGetString("zip"),
                                    Country = source.country, // dr.HAGetString("country"),
                                    IsActive = true,
                                    IsValid = true,
                                    DoNotMail = doNotMail.Length > 0 && doNotMail.ToLower() == "x",
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                    MSA = string.Empty,
                                    County = string.Empty,
                                    Region = string.Empty,
                                    SubRegion = string.Empty,
                                    AirportCode = string.Empty,
                                    Latitude = 0,
                                    Longitude = 0
                                },
            },
            Phones = new CustomerPhone[]
            {
                                new CustomerPhone
                                {
                                    PhoneKey = 0,
                                    ProfileType = phoneProfileType,
                                    Number = phoneNumber,
                                    Extension = string.Empty,
                                    IsActive = true,
                                    IsValid = true,
                                    DoNotPhone = doNotPhone.Length > 0 && doNotPhone.ToLower() =="x",
                                    Score = string.Empty,
                                    ScoreDate = DateTime.Now,
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now
                                },
            },
            Emails = new CustomerEmail[]
            {
                                new CustomerEmail
                                {
                                    EmailKey = 0,
                                    ProfileType = string.Empty,
                                    EmailAddress = source.email, // dr.HAGetString("email"),
                                    IsActive = true,
                                    IsValid = true,
                                    DoNotEmail = doNotEmail.Length > 0 && doNotEmail.ToLower() == "x",
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                    Bounced = false,
                                    BouncedSpecified = true,
                                    BounceDate = DateTime.Now,
                                    Suppress = false,
                                    SuppressSpecified = true,
                                    Subscriptions = new CustomerEmailSubscription[]
                                    {
                                    }
                                },
            },
            SourceSystems = new CustomerSourceSystem[]
            {
                                new CustomerSourceSystem
                                {
                                    Id = sourceSystemCode,
                                    Name = "SMS",
                                    GuestKey = source.guestnum,  // guestNum,
                                    IsActive = true,
                                    UpdateDate = DateTime.Now,
                                },
            },
            Traits = new CustomerTrait[]
            {
            },
            LoyaltyProfiles = new CustomerLoyaltyProfile[]
            {
            },
            NameValues = new CustomerNameValue[]
            {
                                new CustomerNameValue { Name = "SMS_VIP" , Attribute = source.vip }, // dr.HAGetString("vip")},
                                new CustomerNameValue { Name = "SMS_GuestSource" , Attribute = source.source }, // dr.HAGetString("source")},
                                new CustomerNameValue { Name = "PhoneProfileType", Attribute = phoneProfileType },
            },
            Notes = new CustomerNote[]
            {
                                new CustomerNote
                                {
                                    Comment = source.remarks, // dr.HAGetString("remarks"),
                                    UserId = "SYSTEM",
                                    NoteCreateDate = DateTime.Now,
                                    NoteUpdateDate = DateTime.Now,
                                    SourceSystemKey = source.guestnum, // guestNum,
                                    ChangeType = string.Empty,
                                    NoteKey = 0
                                },
            },
           // SalesAgent = new CustomerSalesAgent { AccountId = 0, EmailAddress = string.Empty, FirstName = string.Empty, Id = string.Empty, LastName = string.Empty, LeadSource = string.Empty, LeadStatus = string.Empty },
            SalesAgent = null,
            SocialProfiles = new CustomerSocialProfile[]
            {
            },
            CID = clientId,
            SID = sourceSystemCode,
            IntegrationHistoryId = 0,
            IntegrationHistoryIdSpecified = false,
        };

    }

    //public async Task<MC.Customer> Map(int clientId, int sourceSystemCode, Entity.Guest source)
    //{



    //    var doNotMail = source.nomail; // dr.HAGetString("nomail");
    //    var doNotPhone = source.nophone; // dr.HAGetString("nophone");
    //    var doNotEmail = source.noemail; // dr.HAGetString("noemail");
    //    var phone = source.phone; // dr.HAGetString("phone");
    //    var phoneNum = source.pphonenum; // dr.HAGetString("pphonenum");
    //    var title = source.title; // dr.HAGetString("title");
    //    var genderValue = source.gender; // dr.HAGetString("gender");

    //    var gender = CustomerGender.U;
    //    if (string.IsNullOrEmpty(genderValue))
    //        gender = CustomerGender.U;
    //    else if (genderValue.ToLower().Substring(0, 1) == "m")
    //        gender = CustomerGender.M;
    //    else if (genderValue.ToLower().Substring(0, 1) == "f")
    //        gender = CustomerGender.F;

    //    var phoneNumber = !string.IsNullOrEmpty(phoneNum) ? phoneNum : phone;

    //    var phoneProfileType = string.Empty;
    //    var phoneType = source.phonetype; // dr.HAGetString("phonetype");
    //    if (!string.IsNullOrEmpty(phoneType) && string.Equals(phoneType, "m", StringComparison.InvariantCultureIgnoreCase))
    //    {
    //        phoneProfileType = "Mobile";
    //    }

    //    var customer = new MC.Customer
    //    {
    //        CustomerKey = 0,
    //        Origin = "SMS",
    //        MatchOnly = false,
    //        MatchOnlySpecified = false,
    //        GuestType = CustomerGuestType.FIT,
    //        Title = title.Replace("&", "and"),
    //        FirstName = source.first, // dr.HAGetString("first"),
    //        LastName = source.last, // dr.HAGetString("last"),
    //        Company = string.Empty,
    //        Birthdate = source.birthday ?? new DateTime(1900, 1, 1), // dr.HAGetDateTime("birthday"),
    //        Gender = gender,
    //        CreateDate = DateTime.Now,
    //        UpdateDate = DateTime.Now,
    //        Addresses = new CustomerAddress[]
    //        {
    //                            new CustomerAddress
    //                            {
    //                                AddressKey = 0,
    //                                ProfileType = string.Empty,
    //                                Score = "N",
    //                                ScoreDate = DateTime.Now,
    //                                Line1 = source.address1, // dr.HAGetString("address1"),
    //                                Line2 = source.address2, //  dr.HAGetString("address2"),
    //                                Line3 = source.address3, // dr.HAGetString("address3"),
    //                                City = source.city, // dr.HAGetString("city"),
    //                                State = source.state, // dr.HAGetString("state"),
    //                                PostalCode = source.zip, // dr.HAGetString("zip"),
    //                                Country = source.country, // dr.HAGetString("country"),
    //                                IsActive = true,
    //                                IsValid = true,
    //                                DoNotMail = doNotMail.Length > 0 && doNotMail.ToLower() == "x",
    //                                CreateDate = DateTime.Now,
    //                                UpdateDate = DateTime.Now,
    //                                MSA = string.Empty,
    //                                County = string.Empty,
    //                                Region = string.Empty,
    //                                SubRegion = string.Empty,
    //                                AirportCode = string.Empty,
    //                                Latitude = 0,
    //                                Longitude = 0
    //                            },
    //        },
    //        Phones = new CustomerPhone[]
    //        {
    //                            new CustomerPhone
    //                            {
    //                                PhoneKey = 0,
    //                                ProfileType = phoneProfileType,
    //                                Number = phoneNumber,
    //                                Extension = string.Empty,
    //                                IsActive = true,
    //                                IsValid = true,
    //                                DoNotPhone = doNotPhone.Length > 0 && doNotPhone.ToLower() =="x",
    //                                Score = string.Empty,
    //                                ScoreDate = DateTime.Now,
    //                                CreateDate = DateTime.Now,
    //                                UpdateDate = DateTime.Now
    //                            },
    //        },
    //        Emails = new CustomerEmail[]
    //        {
    //                            new CustomerEmail
    //                            {
    //                                EmailKey = 0,
    //                                ProfileType = string.Empty,
    //                                EmailAddress = source.email, // dr.HAGetString("email"),
    //                                IsActive = true,
    //                                IsValid = true,
    //                                DoNotEmail = doNotEmail.Length > 0 && doNotEmail.ToLower() == "x",
    //                                CreateDate = DateTime.Now,
    //                                UpdateDate = DateTime.Now,
    //                                Bounced = false,
    //                                BouncedSpecified = true,
    //                                BounceDate = DateTime.Now,
    //                                Suppress = false,
    //                                SuppressSpecified = true,
    //                                Subscriptions = new CustomerEmailSubscription[]
    //                                {
    //                                }
    //                            },
    //        },
    //        SourceSystems = new CustomerSourceSystem[]
    //        {
    //                            new CustomerSourceSystem
    //                            {
    //                                Id = sourceSystemCode,
    //                                Name = "SMS",
    //                                GuestKey = source.guestnum,  // guestNum,
    //                                IsActive = true,
    //                                UpdateDate = DateTime.Now,
    //                            },
    //        },
    //        Traits = new CustomerTrait[]
    //        {
    //        },
    //        LoyaltyProfiles = new CustomerLoyaltyProfile[]
    //        {
    //        },
    //        NameValues = new CustomerNameValue[]
    //        {
    //                            new CustomerNameValue { Name = "SMS_VIP" , Attribute = source.vip }, // dr.HAGetString("vip")},
    //                            new CustomerNameValue { Name = "SMS_GuestSource" , Attribute = source.source }, // dr.HAGetString("source")},
    //                            new CustomerNameValue { Name = "PhoneProfileType", Attribute = phoneProfileType },
    //        },
    //        Notes = new CustomerNote[]
    //        {
    //                            new CustomerNote
    //                            {
    //                                Comment = source.remarks, // dr.HAGetString("remarks"),
    //                                UserId = "SYSTEM",
    //                                NoteCreateDate = DateTime.Now,
    //                                NoteUpdateDate = DateTime.Now,
    //                                SourceSystemKey = source.guestnum, // guestNum,
    //                                ChangeType = string.Empty,
    //                                NoteKey = 0
    //                            },
    //        },
    //        SalesAgent = new CustomerSalesAgent { AccountId = 0, EmailAddress = string.Empty, FirstName = string.Empty, Id = string.Empty, LastName = string.Empty, LeadSource = string.Empty, LeadStatus = string.Empty },
    //        SocialProfiles = new CustomerSocialProfile[]
    //        {
    //        },
    //        CID = clientId,
    //        SID = sourceSystemCode,
    //        IntegrationHistoryId = 0,
    //        IntegrationHistoryIdSpecified = false,
    //    };

    //    return customer;
    //}

    public Customer BlankCustomer(AppConfig config)
    {

        var customer = new Customer
        {
            CustomerKey = 0,
            Origin = "SMS",
            MatchOnly = false,
            MatchOnlySpecified = false,
            GuestType = CustomerGuestType.FIT,
            Title = string.Empty,
            FirstName = string.Empty,
            LastName = string.Empty,
            Company = string.Empty,
            Birthdate = new DateTime(1900, 1, 1),
            Gender = CustomerGender.U,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            Addresses = new CustomerAddress[]
            {
                                new CustomerAddress
                                {
                                    AddressKey = 0,
                                    ProfileType = string.Empty,
                                    Score = "N",
                                    ScoreDate = DateTime.Now,
                                    Line1 = string.Empty,
                                    Line2 = string.Empty,
                                    Line3 = string.Empty,
                                    City = string.Empty,
                                    State = string.Empty,
                                    PostalCode = string.Empty,
                                    Country = string.Empty,
                                    IsActive = false,
                                    IsValid = false,
                                    DoNotMail = true,
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                    MSA = string.Empty,
                                    County = string.Empty,
                                    Region = string.Empty,
                                    SubRegion = string.Empty,
                                    AirportCode = string.Empty,
                                    Latitude = 0,
                                    Longitude = 0
                                },
            },
            Phones = new CustomerPhone[]
            {
                                new CustomerPhone
                                {
                                    PhoneKey = 0,
                                    ProfileType = string.Empty,
                                    Number = string.Empty,
                                    Extension = string.Empty,
                                    IsActive = false,
                                    IsValid = false,
                                    DoNotPhone = true,
                                    Score = string.Empty,
                                    ScoreDate = DateTime.Now,
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now
                                },
            },
            Emails = new CustomerEmail[]
            {
                                new CustomerEmail
                                {
                                    EmailKey = 0,
                                    ProfileType = null,
                                    EmailAddress =  "not specified", //  string.Empty,
                                    IsActive = true,
                                    IsValid = true,
                                    DoNotEmail = true,
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                    Bounced = false,
                                    BouncedSpecified = true,
                                    BounceDate = DateTime.Now,
                                    Suppress = false,
                                    SuppressSpecified = true,
                                    Subscriptions = new CustomerEmailSubscription[]
                                    {
                                    }
                                },
            },
            SourceSystems = new CustomerSourceSystem[]
            {
                                new CustomerSourceSystem
                                {
                                    Id = config.SourceSystemCode,
                                    Name = "SMS",
                                    GuestKey =  Guid.NewGuid().ToString(),
                                    IsActive = false,
                                    UpdateDate = DateTime.Now,
                                },
            },
            Traits = new CustomerTrait[]
            {
            },
            LoyaltyProfiles = new CustomerLoyaltyProfile[]
            {
            },
            NameValues = new CustomerNameValue[]
            {
                                new CustomerNameValue { Name = "SMS_VIP" ,Attribute = string.Empty},
                                new CustomerNameValue { Name = "SMS_GuestSource" ,Attribute = "HA"},
            },
            //Notes = new CustomerNote[]
            //{
            //                new CustomerNote
            //                {
            //                    Comment = string.Empty,
            //                    UserId = "SYSTEM",
            //                    NoteCreateDate = DateTime.Now,
            //                    NoteUpdateDate = DateTime.Now,
            //                    SourceSystemKey = guestNum,
            //                    ChangeType = string.Empty,
            //                    NoteKey = 0
            //                },
            //},
            SalesAgent = null,
            SocialProfiles = new CustomerSocialProfile[]
            {
            },
            CID = config.ClientId,
            SID = config.SourceSystemCode,
            IntegrationHistoryId = 0,
            IntegrationHistoryIdSpecified = false
        };

        return customer;

    }

}
