using System;
using FluentMigrator;

namespace DevDayCFP.Migrations
{
    [Migration(1)]
    public class AddUsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("AccountStatus").AsInt16().NotNullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Location").AsString(255)
                .WithColumn("Bio").AsString(Int32.MaxValue)
                .WithColumn("AvatarPath").AsString(255)
                .WithColumn("TwitterHandle").AsString(128)
                .WithColumn("Website").AsString(255);
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}