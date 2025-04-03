namespace IndeService;

public class Schedule
{
    public int ScheduleId { get; set; }

    public string Name { get; set; }

    public int ClientId { get; set; }

    public int ScheduleTypeId { get; set; }

    public int ScheduleFrequencyId { get; set; }

    public int SourceSystemCode { get; set; }

    public int ClientReportId { get; set; }

    public Boolean IsActive { get; set; }

    public Boolean Monday { get; set; }

    public Boolean Tuesday { get; set; }

    public Boolean Wednesday { get; set; }

    public Boolean Thursday { get; set; }

    public Boolean Friday { get; set; }

    public Boolean Saturday { get; set; }

    public Boolean Sunday { get; set; }

    public int DayofMonth { get; set; }

    public DateTime ScheduleTime { get; set; }

    public int QueueRequestId { get; set; }

    public int ExportAccountId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime StopTime { get; set; }

    public int Interval { get; set; }

    public DateTime EndDate { get; set; }

}

