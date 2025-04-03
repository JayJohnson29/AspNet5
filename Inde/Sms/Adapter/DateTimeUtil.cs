namespace Sms.Adapter;

public static class DateTimeUtil
{
    public static DateTime CreateDateTime(DateTime date, string time)
    {
        //var res = dr.HAGetDateTime("skdate");

        //var tm = dr.HAGetString("sktime");

        var x = new string[] { "00", "00" };
        if (time.Contains(":"))
        {
            x = time.Split(':');
        }

        var hours = 0;
        Int32.TryParse(x[0], out hours);
        hours = (hours < 0 || hours > 23) ? 0 : hours;

        var mins = 0;
        Int32.TryParse(x[1], out mins);
        mins = mins < 0 || mins > 59 ? 0 : mins;

        return new DateTime(date.Year, date.Month, date.Day, hours, mins, 0);
    }

}
