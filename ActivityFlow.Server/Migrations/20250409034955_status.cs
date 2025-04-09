using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityFlow.Server.Migrations
{
    /// <inheritdoc />
    public partial class status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_ApplicationUserId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_ApplicationUserId1",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Categories_CategoryId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Categories_CategoryId1",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Statuses_StatusId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Statuses_StatusId1",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ApplicationUserId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ApplicationUserId1",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_CategoryId1",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_StatusId1",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "StatusId1",
                table: "Activities");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Statuses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Statuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Statuses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Statuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Statuses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Activities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Activities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "Progress",
                table: "Activities",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Activities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Color", "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { "#000000", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Color", "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { "#000000", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Color", "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { "#000000", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Color", "CreatedAt", "IsActive", "UpdatedAt" },
                values: new object[] { "#000000", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Color", "CreatedAt", "DisplayOrder", "Name", "UpdatedAt" },
                values: new object[] { null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, null });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Color", "CreatedAt", "DisplayOrder", "Name", "UpdatedAt" },
                values: new object[] { null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, null });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Color", "CreatedAt", "DisplayOrder", "Name", "UpdatedAt" },
                values: new object[] { null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, null });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Color", "CreatedAt", "DisplayOrder", "Name", "UpdatedAt" },
                values: new object[] { null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, null });

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Categories_CategoryId",
                table: "Activities",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Statuses_StatusId",
                table: "Activities",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Categories_CategoryId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Statuses_StatusId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Activities");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Statuses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Statuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Activities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Activities",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId1",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "Order" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "Order" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "Order" },
                values: new object[] { 3, 3 });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "Order" },
                values: new object[] { 4, 4 });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ApplicationUserId",
                table: "Activities",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ApplicationUserId1",
                table: "Activities",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CategoryId1",
                table: "Activities",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_StatusId1",
                table: "Activities",
                column: "StatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_ApplicationUserId",
                table: "Activities",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_ApplicationUserId1",
                table: "Activities",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Categories_CategoryId",
                table: "Activities",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Categories_CategoryId1",
                table: "Activities",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Statuses_StatusId",
                table: "Activities",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Statuses_StatusId1",
                table: "Activities",
                column: "StatusId1",
                principalTable: "Statuses",
                principalColumn: "Id");
        }
    }
}
