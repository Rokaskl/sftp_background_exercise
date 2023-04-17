using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SftpBackgroundExercise;

await Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    // Load the configuration file
    IConfiguration Config = new ConfigurationBuilder()
    .AddJsonFile("appSettings.json")
    .Build();

    // Bind configurations
    services.Configure<JobSettings>(Config.GetSection("JobSettings"));
    services.Configure<SftpSettings>(Config.GetSection("SftpSettings"));

    // Setup postgres
    services.AddDbContext<FileDBContext>(options => options.UseNpgsql(Config.GetConnectionString("postgres")));
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    // DI
    services.AddTransient<IFileRecordRepository, FileRecordRepository>();
    services.AddTransient<ISftpService, SftpService>();
    
    // Add background worker
    services.AddHostedService<PeriodicBackgroundTask>();

}).Build().RunAsync();
