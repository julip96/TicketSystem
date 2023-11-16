using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "Tickets",
                newName: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Tickets",
                newName: "TicketId");
        }
    }
}
