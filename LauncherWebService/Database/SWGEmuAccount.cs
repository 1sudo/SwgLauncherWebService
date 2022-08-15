using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace LauncherWebService.Database;

public class SwgEmuAccount
{
    public const bool DEBUG = true;

    public class AccountContext : DbContext
    {
        // ReSharper disable once InconsistentNaming
        public DbSet<Account>? accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = "server=localhost;user=swgemu;password=123456;database=swgemu";

            if (DEBUG)
            {
                options.UseMySql(new MySqlConnection(connectionString), ServerVersion.AutoDetect(connectionString)).LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
            else
            {
                options.UseMySql(new MySqlConnection(connectionString), ServerVersion.AutoDetect(connectionString));
            }
        }
    }

    public class Account
    {
        [Key]
        // ReSharper disable once InconsistentNaming
        public int account_id { get; set; }
        // ReSharper disable once InconsistentNaming
        public string? username { get; set; }
        // ReSharper disable once InconsistentNaming
        public string? password { get; set; }
        // ReSharper disable once InconsistentNaming
        public long station_id { get; set; }
        // ReSharper disable once InconsistentNaming
        public DateTime? created { get; set; }
        // ReSharper disable once InconsistentNaming
        public int active { get; set; }
        // ReSharper disable once InconsistentNaming
        public int admin_level { get; set; }
        // ReSharper disable once InconsistentNaming
        public string? salt { get; set; }
    }
}
