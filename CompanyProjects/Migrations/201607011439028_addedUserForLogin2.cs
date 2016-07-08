namespace CompanyProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUserForLogin2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 64));
        }
    }
}
