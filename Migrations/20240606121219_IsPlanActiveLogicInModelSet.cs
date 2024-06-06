using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuth.Migrations
{
    public partial class IsPlanActiveLogicInModelSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlanActive",
                table: "devices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPlanActive",
                table: "devices");
        }
    }
}
