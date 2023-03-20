using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesApi.Migrations
{
    /// <inheritdoc />
    public partial class Movies_Room : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActorId",
                table: "Movies_Genres",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Movies_Actors",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "Movies_Rooms",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies_Rooms", x => new { x.MovieId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_Movies_Rooms_CinemaRoom_RoomId",
                        column: x => x.RoomId,
                        principalTable: "CinemaRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movies_Rooms_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Genres_ActorId",
                table: "Movies_Genres",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Actors_GenreId",
                table: "Movies_Actors",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Rooms_RoomId",
                table: "Movies_Rooms",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Actors_Genres_GenreId",
                table: "Movies_Actors",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Genres_Actors_ActorId",
                table: "Movies_Genres",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Actors_Genres_GenreId",
                table: "Movies_Actors");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Genres_Actors_ActorId",
                table: "Movies_Genres");

            migrationBuilder.DropTable(
                name: "Movies_Rooms");

            migrationBuilder.DropTable(
                name: "CinemaRoom");

            migrationBuilder.DropIndex(
                name: "IX_Movies_Genres_ActorId",
                table: "Movies_Genres");

            migrationBuilder.DropIndex(
                name: "IX_Movies_Actors_GenreId",
                table: "Movies_Actors");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "Movies_Genres");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Movies_Actors");
        }
    }
}
