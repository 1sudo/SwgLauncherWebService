using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace LauncherWebService.Database;

public class SwgEmuAccount
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AccountContext : DbContext
    {
        public DbSet<Account>? accounts { get; set; }
        public DbSet<Characters>? characters { get; set; }

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

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Account
    {
        [Key]
        public int account_id { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public long station_id { get; set; }
        public DateTime? created { get; set; }
        public int active { get; set; }
        public int admin_level { get; set; }
        public string? salt { get; set; }
        public string? email { get; set; }
        public string? discord { get; set; }
        public int subscribed { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Characters
    {
        [Key]
        public long? character_oid { get; set; }
        public int account_id { get; set; }
        public int galaxy_id { get; set; }
        public string? firstname { get; set; }
        public string? surname { get; set; }
        public int race { get; set; }
        public int gender { get; set; }
        public string? template { get; set; }
        public DateTime creation_date { get; set; }
    }
}
