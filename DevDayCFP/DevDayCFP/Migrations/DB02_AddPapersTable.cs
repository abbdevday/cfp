﻿using System;
using FluentMigrator;

namespace DevDayCFP.Migrations
{
    [Migration(2)]
    public class DB02_AddPapersTable : Migration
    {
        public override void Up()
        {
            Create.Table("Papers")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("FK_Papers_UserId", "Users", "Id")
                .WithColumn("Title").AsString(255).NotNullable()
                .WithColumn("Level").AsString(255).NotNullable()
                .WithColumn("Abstract").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("Justification").AsString(Int32.MaxValue).Nullable()
                .WithColumn("IsThereLiveCoding").AsBoolean()
                .WithColumn("EventName").AsString(255).NotNullable()
                .WithColumn("LastModificationDate").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Papers");
        }
    }
}