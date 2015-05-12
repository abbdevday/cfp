using System;
using FluentMigrator;

namespace DevDayCFP.Migrations
{
    // Commented out in case someone would like to make a deployment

    //[Migration(5)]
    //public class DB05_AddTokensTable : Migration
    //{
    //    public override void Up()
    //    {
    //        Create.Table("Tokens")
    //            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
    //            .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("FK_Tokens_UserId", "Users", "Id")
    //            .WithColumn("TokenGuid").AsGuid().NotNullable()
    //            .WithColumn("Type").AsInt16().NotNullable()
    //            .WithColumn("CreateDate").AsDateTime().NotNullable()
    //            .WithColumn("IsActive").AsBoolean().NotNullable();
    //    }

    //    public override void Down()
    //    {
    //        Delete.Table("Tokens");
    //    }
    //}
}