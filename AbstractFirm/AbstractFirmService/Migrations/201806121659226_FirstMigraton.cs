namespace AbstractFirmService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigraton : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArchiveBlanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArchiveId = c.Int(nullable: false),
                        BlankId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Archives", t => t.ArchiveId, cascadeDelete: true)
                .ForeignKey("dbo.Blanks", t => t.BlankId, cascadeDelete: true)
                .Index(t => t.ArchiveId)
                .Index(t => t.BlankId);
            
            CreateTable(
                "dbo.Archives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArchiveName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Blanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlankName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PackageBlanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageId = c.Int(nullable: false),
                        BlankId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blanks", t => t.BlankId, cascadeDelete: true)
                .ForeignKey("dbo.Packages", t => t.PackageId, cascadeDelete: true)
                .Index(t => t.PackageId)
                .Index(t => t.BlankId);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KlientId = c.Int(nullable: false),
                        PackageId = c.Int(nullable: false),
                        LawyerId = c.Int(),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateImplement = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Klients", t => t.KlientId, cascadeDelete: true)
                .ForeignKey("dbo.Lawyers", t => t.LawyerId)
                .ForeignKey("dbo.Packages", t => t.PackageId, cascadeDelete: true)
                .Index(t => t.KlientId)
                .Index(t => t.PackageId)
                .Index(t => t.LawyerId);
            
            CreateTable(
                "dbo.Klients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KlientFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lawyers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LawyerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArchiveBlanks", "BlankId", "dbo.Blanks");
            DropForeignKey("dbo.Requests", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.Requests", "LawyerId", "dbo.Lawyers");
            DropForeignKey("dbo.Requests", "KlientId", "dbo.Klients");
            DropForeignKey("dbo.PackageBlanks", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.PackageBlanks", "BlankId", "dbo.Blanks");
            DropForeignKey("dbo.ArchiveBlanks", "ArchiveId", "dbo.Archives");
            DropIndex("dbo.Requests", new[] { "LawyerId" });
            DropIndex("dbo.Requests", new[] { "PackageId" });
            DropIndex("dbo.Requests", new[] { "KlientId" });
            DropIndex("dbo.PackageBlanks", new[] { "BlankId" });
            DropIndex("dbo.PackageBlanks", new[] { "PackageId" });
            DropIndex("dbo.ArchiveBlanks", new[] { "BlankId" });
            DropIndex("dbo.ArchiveBlanks", new[] { "ArchiveId" });
            DropTable("dbo.Lawyers");
            DropTable("dbo.Klients");
            DropTable("dbo.Requests");
            DropTable("dbo.Packages");
            DropTable("dbo.PackageBlanks");
            DropTable("dbo.Blanks");
            DropTable("dbo.Archives");
            DropTable("dbo.ArchiveBlanks");
        }
    }
}
