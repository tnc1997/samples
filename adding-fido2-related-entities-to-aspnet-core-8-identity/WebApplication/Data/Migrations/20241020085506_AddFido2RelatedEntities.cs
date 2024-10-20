using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFido2RelatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fido2PublicKeyCredentials",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    PublicKey = table.Column<byte[]>(type: "bytea", nullable: false),
                    SignatureCounter = table.Column<long>(type: "bigint", nullable: false),
                    IsBackupEligible = table.Column<bool>(type: "boolean", nullable: false),
                    IsBackedUp = table.Column<bool>(type: "boolean", nullable: false),
                    AttestationObject = table.Column<byte[]>(type: "bytea", nullable: false),
                    AttestationClientDataJson = table.Column<byte[]>(type: "bytea", nullable: false),
                    AttestationFormat = table.Column<string>(type: "text", nullable: false),
                    AaGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fido2PublicKeyCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fido2PublicKeyCredentials_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fido2AuthenticatorTransports",
                columns: table => new
                {
                    PublicKeyCredentialId = table.Column<byte[]>(type: "bytea", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fido2AuthenticatorTransports", x => new { x.PublicKeyCredentialId, x.Value });
                    table.ForeignKey(
                        name: "FK_Fido2AuthenticatorTransports_Fido2PublicKeyCredentials_Publ~",
                        column: x => x.PublicKeyCredentialId,
                        principalTable: "Fido2PublicKeyCredentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fido2DevicePublicKeys",
                columns: table => new
                {
                    PublicKeyCredentialId = table.Column<byte[]>(type: "bytea", nullable: false),
                    Value = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fido2DevicePublicKeys", x => new { x.PublicKeyCredentialId, x.Value });
                    table.ForeignKey(
                        name: "FK_Fido2DevicePublicKeys_Fido2PublicKeyCredentials_PublicKeyCr~",
                        column: x => x.PublicKeyCredentialId,
                        principalTable: "Fido2PublicKeyCredentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fido2PublicKeyCredentials_UserId",
                table: "Fido2PublicKeyCredentials",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fido2AuthenticatorTransports");

            migrationBuilder.DropTable(
                name: "Fido2DevicePublicKeys");

            migrationBuilder.DropTable(
                name: "Fido2PublicKeyCredentials");
        }
    }
}
