namespace CompanyProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class final2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataItems", "TitleDataProject", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataItems", "TitleDataProject");
        }
    }
}
