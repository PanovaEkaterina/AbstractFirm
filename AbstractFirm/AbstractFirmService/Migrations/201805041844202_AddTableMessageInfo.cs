namespace AbstractFirmService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableMessageInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageId = c.String(),
                        FromMailAddress = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        DateDelivery = c.DateTime(nullable: false),
                        KlientId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Klients", t => t.KlientId)
                .Index(t => t.KlientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageInfoes", "KlientId", "dbo.Klients");
            DropIndex("dbo.MessageInfoes", new[] { "KlientId" });
            DropTable("dbo.MessageInfoes");
        }
    }
}
