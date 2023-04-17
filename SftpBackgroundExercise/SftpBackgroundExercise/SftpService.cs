using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;

namespace SftpBackgroundExercise;

public interface ISftpService
{
    Task DownloadFilesAndArchive(string remoteDirectory = ".", string remoteArchiveDirectory = "./archive", string localDirectory = ".", int count = 1);
}

public class SftpService : ISftpService
{
    private readonly ILogger<SftpService> _logger;
    private readonly SftpSettings _config;
    private readonly IFileRecordRepository _fileRecordsRepository;

    public SftpService(ILogger<SftpService> logger, IOptions<SftpSettings> sftpConfig,
        IFileRecordRepository fileRecordsRepository)
    {
        _logger = logger;
        _config = sftpConfig.Value;
        _fileRecordsRepository = fileRecordsRepository;
    }

    /// <summary>
    /// Downloads files from the remote directory to the local directory. After downloading creates record about the file in the datbaase. After creating record moves file to archive.
    /// </summary>
    public async Task DownloadFilesAndArchive(string remoteDirectory = ".", string remoteArchiveDirectory = "./archive", string localDirectory = ".", int count = 1)
    {
        using var client = new SftpClient(_config.Host, _config.Port == 0 ? 22 : _config.Port, _config.UserName, _config.Password);

        try
        {
            client.Connect();

            var allFiles = client.ListDirectory(remoteDirectory);

            //Take oldest files and download them
            var targetFiles = allFiles
                .Where(f => f.IsRegularFile)
                .OrderByDescending(f => f.LastWriteTime)
                .Take(count)
                .ToList();

            foreach (var file in targetFiles)
            {
                var localFilePath = $"{localDirectory}/{file.Name}";
                var remoteFilePath = $"{remoteDirectory}/{file.Name}";
                var remoteArchiveFilePath = $"{remoteArchiveDirectory}/{file.Name}";

                DownloadFile(client, file, localDirectory);
                _logger.LogInformation("Finished downloading the file [{localFilePath}] from [{remoteFilePath}]", localFilePath, remoteFilePath);

                FileRecord fileRecord = new()
                {
                    FileName = file.Name,
                    LocalPath = localFilePath,
                    RemotePath = remoteArchiveFilePath,
                    MigratedAt = DateTime.Now
                };

                await _fileRecordsRepository.AddOrUpdateFileRecord(fileRecord);
                _logger.LogInformation("File record was succesfully created");

                MoveFile(client, file, remoteArchiveDirectory); ;
                _logger.LogInformation("Moved the file from [{remoteFilePath}] to [{remoteArchiveFilePath}]", remoteFilePath, remoteArchiveFilePath);
            }

            _logger.LogInformation("Finished downloading {targetFilesCount} files", targetFiles.Count.ToString());
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed in downloading the files");
        }
        finally
        {
            client.Disconnect();
        }
    }

    /// <summary>
    /// Downloads a remote file through the client into a local directory.
    /// </summary>
    private void DownloadFile(SftpClient client, SftpFile file, string directory)
    {
        _logger.LogInformation("Downloading {fileName}", file.FullName);

        using Stream fileStream = File.OpenWrite($"{directory}/{file.Name}");
        client.DownloadFile(file.FullName, fileStream);
    }

    /// <summary>
    /// Moves a remote file through the client into another remote directory directory.
    /// </summary>
    private void MoveFile(SftpClient client, SftpFile file, string directory)
    {
        _logger.LogInformation("Moving {fileName}", file.FullName);

        //Create directory if it doesn't exist
        if (!client.Exists(directory)) client.CreateDirectory(directory);

        file.MoveTo($"{directory}/{file.Name}");
    }
}