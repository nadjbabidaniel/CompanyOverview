namespace CompanyProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class final : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TitleCompany = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        FKCompanyId = c.Int(nullable: false),
                        TitleProject = c.String(nullable: false, maxLength: 64),
                        TextProject = c.String(nullable: false, maxLength: 128),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.Companies", t => t.FKCompanyId, cascadeDelete: true)
                .Index(t => t.FKCompanyId);
            
            CreateTable(
                "dbo.DataEntries",
                c => new
                    {
                        DataEntryId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        TextInput = c.String(),
                        DataProject = c.String(),
                        TitleDataProject = c.String(),
                    })
                .PrimaryKey(t => t.DataEntryId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.DataItems",
                c => new
                    {
                        DataItemId = c.Int(nullable: false, identity: true),
                        DataEntryId = c.Int(nullable: false),
                        DataProject = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DataItemId)
                .ForeignKey("dbo.DataEntries", t => t.DataEntryId, cascadeDelete: true)
                .Index(t => t.DataEntryId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        Username = c.String(nullable: false, maxLength: 64),
                        Email = c.String(nullable: false, maxLength: 64),
                        Password = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DataEntries", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.DataItems", "DataEntryId", "dbo.DataEntries");
            DropForeignKey("dbo.Projects", "FKCompanyId", "dbo.Companies");
            DropIndex("dbo.DataItems", new[] { "DataEntryId" });
            DropIndex("dbo.DataEntries", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "FKCompanyId" });
            DropTable("dbo.Users");
            DropTable("dbo.DataItems");
            DropTable("dbo.DataEntries");
            DropTable("dbo.Projects");
            DropTable("dbo.Companies");
        }
    }
}
