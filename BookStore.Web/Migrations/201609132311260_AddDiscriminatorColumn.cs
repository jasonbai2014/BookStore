namespace BookStore.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiscriminatorColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("AspNetRoles", "Discriminator", x => x.String(nullable: false, maxLength: 128));

        }

        public override void Down()
        {
            DropColumn("AspNetRoles", "Discriminator");
        }
    }
}
