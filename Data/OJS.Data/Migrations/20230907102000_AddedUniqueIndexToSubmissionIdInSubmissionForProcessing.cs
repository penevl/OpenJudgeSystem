﻿#nullable disable

namespace OJS.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedUniqueIndexToSubmissionIdInSubmissionForProcessing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SubmissionsForProcessing_SubmissionId",
                table: "SubmissionsForProcessing",
                column: "SubmissionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubmissionsForProcessing_SubmissionId",
                table: "SubmissionsForProcessing");
        }
    }
}
