using System;
using FluentMigrator;

namespace DevDayCFP.Migrations
{
    [Migration(3)]
    public class DB03_AddRegistrationTokenToUser : Migration
    {
        public override void Up()
        {
            Alter.Table("Users")
                .AddColumn("RegistrationToken").AsGuid().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("RegistrationToken").FromTable("Users");
        }
    }
}