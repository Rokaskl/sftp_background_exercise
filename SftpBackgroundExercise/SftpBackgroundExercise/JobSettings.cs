namespace SftpBackgroundExercise
{
    public class JobSettings
    {
        public int JobIntervalInSeconds { get; set; }
        public string RemoteDirectory { get; set; } = string.Empty;
        public string RemoteArchiveDirectory { get; set; } = string.Empty;
        public string LocalDirectory { get; set; } = string.Empty;

    }
}