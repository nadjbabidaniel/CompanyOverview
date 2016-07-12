namespace CompanyProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedTextForDataEntry : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DataEntries", "TextInput", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DataEntries", "TextInput", c => c.String(nullable: false, maxLength: 512));
        }
    }
}
