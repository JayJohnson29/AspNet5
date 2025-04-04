using Microsoft.Extensions.Logging;
using Sms.Repository;

namespace Sms.Service;

public class DatabaseRestoreService(ILogger<DatabaseRestoreService> logger, ISmsIntegrationRepostitory smsIntegrationRepostitory) : IDatabaseRestoreService
{
    public async Task<Tuple<bool, int>> RunAsync(AppConfig config)
    {

        logger.LogDebug("Starting Database Restore ");


        const string smsDrPath = $"C:\\Ryan Solutions\\Host Adapter\\SmsDr\\SmsDr.exe";
        var output = RunSyncAndGetResults(smsDrPath, $"--connectionstring \"{config.ConnectionString}\"");

        var smsIntegrations = await smsIntegrationRepostitory.GetAsync();
        var rebuildResults = smsIntegrations.First();

        if (rebuildResults.StatusId != 2)   // item2 is statusid 2 is success
        {
            return new Tuple<bool, int>(false, -1);
        }

        return new Tuple<bool, int>(true, rebuildResults.SmsIntegrationId);
    }

    public static string RunSyncAndGetResults(string executable, string args, int waitTimeInSeconds = 180)
    {


        if (!File.Exists(executable))
        {
            return $"Executable {executable} does not exist";
        }


        var fi = new FileInfo(executable);
        var workingDir = fi.DirectoryName;

        var psi = new System.Diagnostics.ProcessStartInfo(executable)
        {
            Arguments = args,
            RedirectStandardOutput = true,
            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            WorkingDirectory = workingDir,
        };


        var process = System.Diagnostics.Process.Start(psi);
        if (process == null)
        {
            return $"error running {executable}";
        }
        process.WaitForExit(waitTimeInSeconds * 1000);

        var output = process.StandardOutput.ReadToEnd();


        return output;
    }



}

