#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IdentityServer.Data.Migrations.IdentityServer.ConfigurationDb;

public partial class InitialIdentityServerConfigurationDbMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "ApiResources",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Name = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                AllowedAccessTokenSigningAlgorithms =
                    table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                ShowInDiscoveryDocument = table.Column<bool>("boolean", nullable: false),
                RequireResourceIndicator = table.Column<bool>("boolean", nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "ApiScopes",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Name = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                Required = table.Column<bool>("boolean", nullable: false),
                Emphasize = table.Column<bool>("boolean", nullable: false),
                ShowInDiscoveryDocument = table.Column<bool>("boolean", nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "Clients",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                ClientId = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ProtocolType = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                RequireClientSecret = table.Column<bool>("boolean", nullable: false),
                ClientName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                ClientUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                LogoUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                RequireConsent = table.Column<bool>("boolean", nullable: false),
                AllowRememberConsent = table.Column<bool>("boolean", nullable: false),
                AlwaysIncludeUserClaimsInIdToken = table.Column<bool>("boolean", nullable: false),
                RequirePkce = table.Column<bool>("boolean", nullable: false),
                AllowPlainTextPkce = table.Column<bool>("boolean", nullable: false),
                RequireRequestObject = table.Column<bool>("boolean", nullable: false),
                AllowAccessTokensViaBrowser = table.Column<bool>("boolean", nullable: false),
                FrontChannelLogoutUri =
                    table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                FrontChannelLogoutSessionRequired = table.Column<bool>("boolean", nullable: false),
                BackChannelLogoutUri = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                BackChannelLogoutSessionRequired = table.Column<bool>("boolean", nullable: false),
                AllowOfflineAccess = table.Column<bool>("boolean", nullable: false),
                IdentityTokenLifetime = table.Column<int>("integer", nullable: false),
                AllowedIdentityTokenSigningAlgorithms =
                    table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                AccessTokenLifetime = table.Column<int>("integer", nullable: false),
                AuthorizationCodeLifetime = table.Column<int>("integer", nullable: false),
                ConsentLifetime = table.Column<int>("integer", nullable: true),
                AbsoluteRefreshTokenLifetime = table.Column<int>("integer", nullable: false),
                SlidingRefreshTokenLifetime = table.Column<int>("integer", nullable: false),
                RefreshTokenUsage = table.Column<int>("integer", nullable: false),
                UpdateAccessTokenClaimsOnRefresh = table.Column<bool>("boolean", nullable: false),
                RefreshTokenExpiration = table.Column<int>("integer", nullable: false),
                AccessTokenType = table.Column<int>("integer", nullable: false),
                EnableLocalLogin = table.Column<bool>("boolean", nullable: false),
                IncludeJwtId = table.Column<bool>("boolean", nullable: false),
                AlwaysSendClientClaims = table.Column<bool>("boolean", nullable: false),
                ClientClaimsPrefix = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                PairWiseSubjectSalt = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                UserSsoLifetime = table.Column<int>("integer", nullable: true),
                UserCodeType = table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                DeviceCodeLifetime = table.Column<int>("integer", nullable: false),
                CibaLifetime = table.Column<int>("integer", nullable: true),
                PollingInterval = table.Column<int>("integer", nullable: true),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clients", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "IdentityProviders",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scheme = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Type = table.Column<string>("character varying(20)", maxLength: 20, nullable: false),
                Properties = table.Column<string>("text", nullable: true),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                LastAccessed = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityProviders", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "IdentityResources",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Enabled = table.Column<bool>("boolean", nullable: false),
                Name = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                DisplayName = table.Column<string>("character varying(200)", maxLength: 200, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                Required = table.Column<bool>("boolean", nullable: false),
                Emphasize = table.Column<bool>("boolean", nullable: false),
                ShowInDiscoveryDocument = table.Column<bool>("boolean", nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Updated = table.Column<DateTime>("timestamp with time zone", nullable: true),
                NonEditable = table.Column<bool>("boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "ApiResourceClaims",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>("integer", nullable: false),
                Type = table.Column<string>("character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceClaims", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceClaims_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    "ApiResources",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiResourceProperties",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceProperties", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceProperties_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    "ApiResources",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiResourceScopes",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scope = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ApiResourceId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceScopes_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    "ApiResources",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiResourceSecrets",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ApiResourceId = table.Column<int>("integer", nullable: false),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                Value = table.Column<string>("character varying(4000)", maxLength: 4000, nullable: false),
                Expiration = table.Column<DateTime>("timestamp with time zone", nullable: true),
                Type = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiResourceSecrets", x => x.Id);
                table.ForeignKey(
                    "FK_ApiResourceSecrets_ApiResources_ApiResourceId",
                    x => x.ApiResourceId,
                    "ApiResources",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiScopeClaims",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ScopeId = table.Column<int>("integer", nullable: false),
                Type = table.Column<string>("character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopeClaims", x => x.Id);
                table.ForeignKey(
                    "FK_ApiScopeClaims_ApiScopes_ScopeId",
                    x => x.ScopeId,
                    "ApiScopes",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ApiScopeProperties",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ScopeId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiScopeProperties", x => x.Id);
                table.ForeignKey(
                    "FK_ApiScopeProperties_ApiScopes_ScopeId",
                    x => x.ScopeId,
                    "ApiScopes",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientClaims",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Type = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientClaims", x => x.Id);
                table.ForeignKey(
                    "FK_ClientClaims_Clients_ClientId",
                    x => x.ClientId,
                    "Clients",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientCorsOrigins",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Origin = table.Column<string>("character varying(150)", maxLength: 150, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
                table.ForeignKey(
                    "FK_ClientCorsOrigins_Clients_ClientId",
                    x => x.ClientId,
                    "Clients",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientGrantTypes",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                GrantType = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientGrantTypes", x => x.Id);
                table.ForeignKey(
                    "FK_ClientGrantTypes_Clients_ClientId",
                    x => x.ClientId,
                    "Clients",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientIdPRestrictions",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Provider = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientIdPRestrictions", x => x.Id);
                table.ForeignKey(
                    "FK_ClientIdPRestrictions_Clients_ClientId",
                    x => x.ClientId,
                    "Clients",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientPostLogoutRedirectUris",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                PostLogoutRedirectUri = table.Column<string>("character varying(400)", maxLength: 400, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientPostLogoutRedirectUris", x => x.Id);
                table.ForeignKey(
                    "FK_ClientPostLogoutRedirectUris_Clients_ClientId",
                    x => x.ClientId,
                    "Clients",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientProperties",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ClientId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientProperties", x => x.Id);
                table.ForeignKey(
                    "FK_ClientProperties_Clients_ClientId",
                    x => x.ClientId,
                    "Clients",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientRedirectUris",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                RedirectUri = table.Column<string>("character varying(400)", maxLength: 400, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientRedirectUris", x => x.Id);
                table.ForeignKey(
                    "FK_ClientRedirectUris_Clients_ClientId",
                    x => x.ClientId,
                    "Clients",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientScopes",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Scope = table.Column<string>("character varying(200)", maxLength: 200, nullable: false),
                ClientId = table.Column<int>("integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientScopes", x => x.Id);
                table.ForeignKey(
                    "FK_ClientScopes_Clients_ClientId",
                    x => x.ClientId,
                    "Clients",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientSecrets",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ClientId = table.Column<int>("integer", nullable: false),
                Description = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                Value = table.Column<string>("character varying(4000)", maxLength: 4000, nullable: false),
                Expiration = table.Column<DateTime>("timestamp with time zone", nullable: true),
                Type = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Created = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                table.ForeignKey(
                    "FK_ClientSecrets_Clients_ClientId",
                    x => x.ClientId,
                    "Clients",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "IdentityResourceClaims",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdentityResourceId = table.Column<int>("integer", nullable: false),
                Type = table.Column<string>("character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResourceClaims", x => x.Id);
                table.ForeignKey(
                    "FK_IdentityResourceClaims_IdentityResources_IdentityResourceId",
                    x => x.IdentityResourceId,
                    "IdentityResources",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "IdentityResourceProperties",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdentityResourceId = table.Column<int>("integer", nullable: false),
                Key = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Value = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdentityResourceProperties", x => x.Id);
                table.ForeignKey(
                    "FK_IdentityResourceProperties_IdentityResources_IdentityResour~",
                    x => x.IdentityResourceId,
                    "IdentityResources",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_ApiResourceClaims_ApiResourceId_Type",
            "ApiResourceClaims",
            new[] { "ApiResourceId", "Type" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceProperties_ApiResourceId_Key",
            "ApiResourceProperties",
            new[] { "ApiResourceId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResources_Name",
            "ApiResources",
            "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceScopes_ApiResourceId_Scope",
            "ApiResourceScopes",
            new[] { "ApiResourceId", "Scope" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceSecrets_ApiResourceId",
            "ApiResourceSecrets",
            "ApiResourceId");

        migrationBuilder.CreateIndex(
            "IX_ApiScopeClaims_ScopeId_Type",
            "ApiScopeClaims",
            new[] { "ScopeId", "Type" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiScopeProperties_ScopeId_Key",
            "ApiScopeProperties",
            new[] { "ScopeId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiScopes_Name",
            "ApiScopes",
            "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientClaims_ClientId_Type_Value",
            "ClientClaims",
            new[] { "ClientId", "Type", "Value" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientCorsOrigins_ClientId_Origin",
            "ClientCorsOrigins",
            new[] { "ClientId", "Origin" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientGrantTypes_ClientId_GrantType",
            "ClientGrantTypes",
            new[] { "ClientId", "GrantType" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientIdPRestrictions_ClientId_Provider",
            "ClientIdPRestrictions",
            new[] { "ClientId", "Provider" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientPostLogoutRedirectUris_ClientId_PostLogoutRedirectUri",
            "ClientPostLogoutRedirectUris",
            new[] { "ClientId", "PostLogoutRedirectUri" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientProperties_ClientId_Key",
            "ClientProperties",
            new[] { "ClientId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientRedirectUris_ClientId_RedirectUri",
            "ClientRedirectUris",
            new[] { "ClientId", "RedirectUri" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Clients_ClientId",
            "Clients",
            "ClientId",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientScopes_ClientId_Scope",
            "ClientScopes",
            new[] { "ClientId", "Scope" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientSecrets_ClientId",
            "ClientSecrets",
            "ClientId");

        migrationBuilder.CreateIndex(
            "IX_IdentityProviders_Scheme",
            "IdentityProviders",
            "Scheme",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResourceClaims_IdentityResourceId_Type",
            "IdentityResourceClaims",
            new[] { "IdentityResourceId", "Type" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResourceProperties_IdentityResourceId_Key",
            "IdentityResourceProperties",
            new[] { "IdentityResourceId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResources_Name",
            "IdentityResources",
            "Name",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "ApiResourceClaims");

        migrationBuilder.DropTable(
            "ApiResourceProperties");

        migrationBuilder.DropTable(
            "ApiResourceScopes");

        migrationBuilder.DropTable(
            "ApiResourceSecrets");

        migrationBuilder.DropTable(
            "ApiScopeClaims");

        migrationBuilder.DropTable(
            "ApiScopeProperties");

        migrationBuilder.DropTable(
            "ClientClaims");

        migrationBuilder.DropTable(
            "ClientCorsOrigins");

        migrationBuilder.DropTable(
            "ClientGrantTypes");

        migrationBuilder.DropTable(
            "ClientIdPRestrictions");

        migrationBuilder.DropTable(
            "ClientPostLogoutRedirectUris");

        migrationBuilder.DropTable(
            "ClientProperties");

        migrationBuilder.DropTable(
            "ClientRedirectUris");

        migrationBuilder.DropTable(
            "ClientScopes");

        migrationBuilder.DropTable(
            "ClientSecrets");

        migrationBuilder.DropTable(
            "IdentityProviders");

        migrationBuilder.DropTable(
            "IdentityResourceClaims");

        migrationBuilder.DropTable(
            "IdentityResourceProperties");

        migrationBuilder.DropTable(
            "ApiResources");

        migrationBuilder.DropTable(
            "ApiScopes");

        migrationBuilder.DropTable(
            "Clients");

        migrationBuilder.DropTable(
            "IdentityResources");
    }
}
