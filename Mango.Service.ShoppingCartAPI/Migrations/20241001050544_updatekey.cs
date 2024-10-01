using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Service.ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatekey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CardDetailsId",
                table: "CartDetails",
                newName: "CartDetailsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartDetailsId",
                table: "CartDetails",
                newName: "CardDetailsId");
        }
    }
}
