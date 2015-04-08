using System;
using FluentMigrator;

namespace DevDayCFP.Migrations
{
    [Migration(4)]
    public class DB04_AddShowOffFieldToUser : Migration
    {
        public override void Up()
        {
            Alter.Table("Users")
                .AddColumn("ShowOff").AsString(Int32.MaxValue).Nullable();
        }

        public override void Down()
        {
            Delete.Column("ShowOff").FromTable("Users");
        }
    }
}