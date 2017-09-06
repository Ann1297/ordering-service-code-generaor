using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MatOrderingService.Migrations
{
    public partial class SeedProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[Products]([Code],[Name])
                VALUES
                    ('EW723','Elder Wand'),    
                    ('TH365','Thestral tail hair'),
                    ('TB105','The Tales of Beedle the Bard'),
                    ('IC970','Invisibility cloak'),
                    ('RS074','Resurrection Stone'),
                    ('SG137','Sword of Godric Gryffindor')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
