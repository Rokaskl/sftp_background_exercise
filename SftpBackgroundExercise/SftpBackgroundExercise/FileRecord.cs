using System.ComponentModel.DataAnnotations;

namespace SftpBackgroundExercise
{
    public class FileRecord
    {
        [Key]
        public string FileName { get; set; }
        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
        public DateTime MigratedAt { get; set; }
    }
}