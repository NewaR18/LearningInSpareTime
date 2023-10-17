using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnExcelToSQLUsingDotNet.Migrations
{
    /// <inheritdoc />
    public partial class mg11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ColumnDefinition",
                columns: table => new
                {
                    ColumnId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColumnName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColumnDefinition", x => x.ColumnId);
                });

            migrationBuilder.CreateTable(
                name: "PriceCalculation",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceCalculation", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TransactionDatas",
                columns: table => new
                {
                    TRAN_ID = table.Column<int>(type: "int", nullable: false),
                    MESSAGE_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ORIGINATOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRAN_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DATE_TIME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TRAN_CHARGE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TRACE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACQUIRER_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TERM_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCOUNT_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POSTING_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OLD_DATE_TIME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OLD_TRACE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OLD_ACQUIRER_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RESP_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LEDGER_BALANCE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VCH_NO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ERR_DESC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SYS_DATE_TIME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISSUER_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WAIVER_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDatas", x => x.TRAN_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColumnDefinition");

            migrationBuilder.DropTable(
                name: "PriceCalculation");

            migrationBuilder.DropTable(
                name: "TransactionDatas");
        }
    }
}
