using FluentMigrator;

namespace DevDayCFP.Migrations
{
    [Migration(1)]
    public class AddUsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity();
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}