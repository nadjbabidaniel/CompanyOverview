namespace CompanyProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUserForLogin3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 512));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
