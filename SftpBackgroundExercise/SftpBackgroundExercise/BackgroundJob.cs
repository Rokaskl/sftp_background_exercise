using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SftpBackgroundExercise;
public class PeriodicBackgroundTask : BackgroundService
{
    private readonly ISftpService _sftpService;
    private readonly ILogger<PeriodicBackgroundTask> _logger;
    private readonly JobSettings _jobSettings;

    public PeriodicBackgroundTask(ISftpService sftpService, ILogger<PeriodicBackgroundTask> logger, IOptions<JobSettings> jobSettings)
    {
        _sftpService = sftpService;
        _logger = logger;
        _jobSettings = jobSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(_jobSettings.JobIntervalInSeconds));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                _logger.LogInformation($"Starting a new job itteration");
                await _sftpService.DownloadFilesAndArchive(_jobSettings.RemoteDirectory, _jobSettings.RemoteArchiveDirectory, _jobSettings.LocalDirectory);
                _logger.LogInformation($"Finished the job itteration");

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to execute PeriodicHostedService with exception message: {message}", ex.Message);
            }
        }
    }
}