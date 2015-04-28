using System;
using FluentMigrator;

namespace DevDayCFP.Migrations
{
    [Migration(1)]
    public class DB01_AddUsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("AccountStatus").AsInt16().NotNullable()
                .WithColumn("UserName").AsString(64).NotNullable()
                .WithColumn("Password").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("EmailHash").AsString(255).NotNullable()
                .WithColumn("Name").AsString(255).Nullable()
                .WithColumn("Location").AsString(255).Nullable()
                .WithColumn("Bio").AsString(Int32.MaxValue).Nullable()
                .WithColumn("AvatarPath").AsString(255).Nullable()
                .WithColumn("TwitterHandle").AsString(128).Nullable()
                .WithColumn("Website").AsString(255).Nullable()
                .WithColumn("ClaimsList").AsString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}