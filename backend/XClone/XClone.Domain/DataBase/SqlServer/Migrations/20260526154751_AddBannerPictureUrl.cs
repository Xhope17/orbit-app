using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XClone.Domain.DataBase.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerPictureUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannerPictureUrl",
                table: "User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerPictureUrl",
                table: "User");
        }
    }
}
