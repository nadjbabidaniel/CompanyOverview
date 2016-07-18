namespace CompanyProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class final3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DataEntries", "DataProject");
            DropColumn("dbo.DataEntries", "TitleDataProject");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataEntries", "TitleDataProject", c => c.String());
            AddColumn("dbo.DataEntries", "DataProject", c => c.String());
        }
    }
}
