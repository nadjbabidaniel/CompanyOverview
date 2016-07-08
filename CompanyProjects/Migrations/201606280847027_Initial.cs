namespace CompanyProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                        CompanyId = c.Int(nullable: false),
                        CompanyTitle = c.String(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        ProjectTitle = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        TextInput = c.String(nullable: false, maxLength: 512),
                        DataProject = c.String(),
                        TitleDataProject = c.String(),
                    })
                .PrimaryKey(t => t.DataEntryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "FKCompanyId", "dbo.Companies");
            DropIndex("dbo.Projects", new[] { "FKCompanyId" });
            DropTable("dbo.DataEntries");
            DropTable("dbo.Projects");
            DropTable("dbo.Companies");
        }
    }
}
