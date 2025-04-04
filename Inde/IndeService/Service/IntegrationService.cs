using IndeService.Repository;

namespace IndeService.Service;

public class IntegrationService(
    ILogger<IntegrationService> logger,
    IIntegrationInstanceRepository integrationInstanceRepository,
    IServiceScopeFactory serviceScopeFactory,
    IBatchService batchService
     ) : IIntegrationService
{


    public async Task<bool> RunAsync(CancellationToken cancellationToken)
    {
        try
        {

            var integrationInstanceConfigurations = await integrationInstanceRepository.GetAsync();

            logger.LogDebug("Retrieved {count} integrations", integrationInstanceConfigurations.Count);

            var scheduledIntegrations = GetServerScheduledIntegrations(integrationInstanceConfigurations);

            foreach (var integration in scheduledIntegrations)
            {
                    //var smsService = serviceProvider.GetRequiredService<ILetterService>();
                    using IServiceScope scope = serviceScopeFactory.CreateScope();
                    var s = scope.ServiceProvider.GetRequiredService<Sms.Service.IInstanceService>();

                    var resp = await s.ExecuteAsync(cancellationToken, integration.InstanceConfig);
                    if (resp.IsSuccess)
                    {
                        var a = await batchService.PostFileAsync(resp.Data);

                    }

            }

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error running integration");
            return false;
        }

    }


    public List<IntegrationInstanceConfiguration> GetServerScheduledIntegrations(List<IntegrationInstanceConfiguration> integrations)
    {
        var rundate = DateTime.Now;
        var scheduledIntegrations = new List<IntegrationInstanceConfiguration>();

        foreach (var integration in integrations)
        {
            if (AdapterScheduled(integration.Schedule, rundate))
            {
                logger.LogDebug("Integration {0} is scheduled to run",integration.InstanceConfig.Name);

                scheduledIntegrations.Add(integration);
            }
        }

        return scheduledIntegrations;
    }

    public bool AdapterScheduled(Schedule schedule, DateTime runDateTime)
    {


        if (!schedule.IsActive)
        {
            return false;
        }

        if (schedule.ScheduleFrequencyId == 4  /*"Real-Time"*/)
        {
            DateTime endDate = schedule.EndDate;

            if (runDateTime > endDate)   // now is past end date
            {
                return false;
            }


            var startTime = schedule.StartTime;
            var stopTime = schedule.StopTime;

            int interval = schedule.Interval;

            if ((runDateTime.TimeOfDay >= startTime.TimeOfDay) && (runDateTime.TimeOfDay < stopTime.TimeOfDay))
            {
                var dateDiff = DateTime.Now.TimeOfDay.Subtract(startTime.TimeOfDay);

                var totalMinutes = (int)Math.Truncate(dateDiff.TotalMinutes);
                var remainder = totalMinutes % interval;

                if (remainder == 0)
                {
                    return true;
                }

            }
        }
        else if (schedule.ScheduleFrequencyId == 1  /*"Daily" */)
        {
            // For daily, weekly, monthly we check
            // if hour and minute match the scheduled time

            if ((runDateTime.Hour == schedule.ScheduleTime.Hour) && (runDateTime.Minute == schedule.ScheduleTime.Minute))
            {
               return true;
            }
        }
        else if (schedule.ScheduleFrequencyId == 2  /* "Weekly" */)
        {

            var dayEnabled = false;
            switch (runDateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    //return schedule.Sunday;
                    dayEnabled = schedule.Sunday;
                    break;
                case DayOfWeek.Monday:
                    // return schedule.Monday;
                    dayEnabled = schedule.Monday;
                    break;

                case DayOfWeek.Tuesday:
                    //return schedule.Tuesday;    
                    dayEnabled = schedule.Tuesday;
                    break;

                case DayOfWeek.Wednesday:
                    //return schedule.Wednesday;
                    dayEnabled = schedule.Wednesday;
                    break;


                case DayOfWeek.Thursday:
                    //return schedule.Thursday;
                    dayEnabled = schedule.Thursday;
                    break;


                case DayOfWeek.Friday:
                    //return schedule.Friday;
                    dayEnabled = schedule.Friday;
                    break;


                case DayOfWeek.Saturday:
                    //return schedule.Saturday;
                    dayEnabled = schedule.Saturday;
                    break;


                default:
                    dayEnabled = false;
                    break;
            }


            if (dayEnabled && (runDateTime.Hour == schedule.ScheduleTime.Hour) && (runDateTime.Minute == schedule.ScheduleTime.Minute))
            {
                return true;
            }

            return false;

        }
        else if (schedule.ScheduleFrequencyId == 3 /* "Monthly"  */)
        {

            if (runDateTime.Day == schedule.DayofMonth)
            {
                if ((runDateTime.Hour == schedule.ScheduleTime.Hour) && (runDateTime.Minute == schedule.ScheduleTime.Minute))
                {
                    return true;
                }
            }

        }

        return false;
    }

}
