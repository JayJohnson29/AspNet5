using IndeService.Repository;

namespace IndeService.Service;

public class IntegrationService(ILogger<IntegrationService> logger, ConfigurationSettings configurationSettings,
     IIntegrationInstanceRepository integrationInstanceRepository,
    IServiceProvider serviceProvider,
     IServiceScopeFactory serviceScopeFactory) : IIntegrationService
{
    private readonly ILogger<IntegrationService> _logger = logger;
    private readonly ConfigurationSettings _configurationSettings = configurationSettings;
    private readonly IServiceProvider _serviceProvider = serviceProvider;


    public async Task<bool> RunAsync(CancellationToken cancellationToken)
    {
        var integrationInstanceConfigurations = await integrationInstanceRepository.GetAsync();

        var rundate = DateTime.Now;
        foreach (var integration in integrationInstanceConfigurations)
        {
            if (AdapterScheduled(integration.Schedule, rundate))
            {
                //var smsService = serviceProvider.GetRequiredService<ILetterService>();
                using IServiceScope scope = serviceScopeFactory.CreateScope();
                var s = scope.ServiceProvider.GetRequiredService<Sms.Service.IInstanceService>();

                await s.ExecuteAsync(cancellationToken, integration.InstanceConfig);
                // RunAsync( integration )
            }

        }

        return true;
    }


    public bool AdapterScheduled(Schedule schedule, DateTime runDateTime)
    {
        // _log.Debug("Begin: AdapterScheduled(XmlNode scheduleNode, DateTime runDateTime)");

        bool runApplet = false;

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
                    runApplet = true;
                }

            }
        }
        else if (schedule.ScheduleFrequencyId == 1  /*"Daily" */)
        {
            // For daily, weekly, monthly we check
            // if hour and minute match the scheduled time


            //string time = schedule.ScheduleTime;
            //string[] timePart = time.Split(':');
            //if ((runDateTime.Hour == int.Parse(timePart[0])) && (runDateTime.Minute == int.Parse(timePart[1])))
            //{
            //    runApplet = true;
            //}

            if ((runDateTime.Hour == schedule.ScheduleTime.Hour) && (runDateTime.Minute == schedule.ScheduleTime.Minute))
            {
                runApplet = true;
            }
        }
        else if (schedule.ScheduleFrequencyId == 2  /* "Weekly" */)
        {
            //string time = scheduleNode.Attributes["Time"].Value;
            //string dayOfWeek = runDateTime.DayOfWeek.ToString();

            //bool dayEnabled = bool.Parse(scheduleNode.Attributes[dayOfWeek].Value);
            //if (dayEnabled)
            //{
            //    string[] timePart = time.Split(':');
            //    if ((runDateTime.Hour == int.Parse(timePart[0])) && (runDateTime.Minute == int.Parse(timePart[1])))
            //    {
            //        runApplet = true;
            //    }
            //}

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
                //runApplet = true;
                return true;
            }

            return false;

        }
        else if (schedule.ScheduleFrequencyId == 3 /* "Monthly"  */)
        {
            //if (scheduleNode.Attributes["DaysOfMonth"] != null)
            //{
            //    int dayOfMonth = int.Parse(scheduleNode.Attributes["DaysOfMonth"].Value);

            //    if (runDateTime.Day == dayOfMonth)
            //    {
            //        string time = scheduleNode.Attributes["Time"].Value;
            //        string[] timePart = time.Split(':');
            //        if ((runDateTime.Hour == int.Parse(timePart[0])) && (runDateTime.Minute == int.Parse(timePart[1])))
            //        {
            //            runApplet = true;
            //        }
            //    }
            //}

            if (runDateTime.Day == schedule.DayofMonth)
            {
                if ((runDateTime.Hour == schedule.ScheduleTime.Hour) && (runDateTime.Minute == schedule.ScheduleTime.Minute))
                {
                    runApplet = true;
                }
            }

        }
        // _log.Debug("End: AdapterScheduled(XmlNode scheduleNode, DateTime runDateTime)");
        // DONT COMMIT
        return true;

        return runApplet;
    }

}
