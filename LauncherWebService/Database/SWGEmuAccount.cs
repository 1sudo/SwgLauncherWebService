using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace LauncherWebService.Database;

public class SwgEmuAccount
{
    public class AccountContext : DbContext
    {
        // ReSharper disable once InconsistentNaming
        public DbSet<Account>? accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connection = new MySqlConnection(Program.Settings!.MySqlConnectionString);
            var serverVersion = ServerVersion.AutoDetect(Program.Settings!.MySqlConnectionString);

            if (Program.Settings!.MySqlDebug)
            {
                options.UseMySql(connection, serverVersion)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
            else
            {
                options.UseMySql(connection, serverVersion);
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
