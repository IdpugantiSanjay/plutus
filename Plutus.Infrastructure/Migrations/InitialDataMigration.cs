using Microsoft.EntityFrameworkCore.Migrations;
using Plutus.Application;

namespace Plutus.Infrastructure.Migrations
{
    public class InitialDataMigration: Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string username = "sanjay";
            const string password = "sanjay_11";
            var hashedPassword = PasswordHelper.GeneratePassword(password, username);
            
            migrationBuilder.Sql($@"
                INSERT INTO User (Username, Email, FirstName, LastName, Password)
                SELECT ('{username}', 'sanjay11@outlook.com', 'Sanjay', 'Idpuganti', '{hashedPassword}')
            "); 
        }
    }
}