namespace SftpBackgroundExercise
{
    public interface IFileRecordRepository
    {
        Task AddOrUpdateFileRecord(FileRecord fileRecord);
    }
    public class FileRecordRepository : IFileRecordRepository
    {
        private readonly FileDBContext _dbContext;
        public FileRecordRepository(FileDBContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task AddOrUpdateFileRecord(FileRecord fileRecord)
        {
            var currentRecord = _dbContext.Files.Find(fileRecord.FileName);

            if (currentRecord is not null)
            {
                currentRecord.LocalPath = fileRecord.LocalPath;
                currentRecord.RemotePath = fileRecord.RemotePath;
                currentRecord.MigratedAt = fileRecord.MigratedAt;
            }
            else
            {
                await _dbContext.AddAsync(fileRecord);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}