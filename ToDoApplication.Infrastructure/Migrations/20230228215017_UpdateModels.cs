using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApplication.Infrastructure.Migrations
{
#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1600 // Elements should be documented

    public partial class UpdateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskNotes_ToDoTasks_ToDoTaskId",
                table: "TaskNotes");

            migrationBuilder.AlterColumn<int>(
                name: "ToDoTaskId",
                table: "TaskNotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskNotes_ToDoTasks_ToDoTaskId",
                table: "TaskNotes",
                column: "ToDoTaskId",
                principalTable: "ToDoTasks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskNotes_ToDoTasks_ToDoTaskId",
                table: "TaskNotes");

            migrationBuilder.AlterColumn<int>(
                name: "ToDoTaskId",
                table: "TaskNotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskNotes_ToDoTasks_ToDoTaskId",
                table: "TaskNotes",
                column: "ToDoTaskId",
                principalTable: "ToDoTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
