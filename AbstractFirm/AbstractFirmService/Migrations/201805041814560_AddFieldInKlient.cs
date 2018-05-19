namespace AbstractFirmService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldInKlient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Klients", "Mail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Klients", "Mail");
        }
    }
}
