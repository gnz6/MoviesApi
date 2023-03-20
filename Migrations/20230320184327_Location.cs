using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace MoviesApi.Migrations
{
    /// <inheritdoc />
    public partial class Location : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Rooms_CinemaRoom_RoomId",
                table: "Movies_Rooms");

            migrationBuilder.DropTable(
                name: "CinemaRoom");

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Location = table.Column<Point>(type: "geography", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Rooms_Room_RoomId",
                table: "Movies_Rooms",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Rooms_Room_RoomId",
                table: "Movies_Rooms");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.CreateTable(
                name: "CinemaRoom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaRoom", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Rooms_CinemaRoom_RoomId",
                table: "Movies_Rooms",
                column: "RoomId",
                principalTable: "CinemaRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
