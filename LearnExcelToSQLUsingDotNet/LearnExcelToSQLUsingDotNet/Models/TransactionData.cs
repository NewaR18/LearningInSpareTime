using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnExcelToSQLUsingDotNet.Models
{
    public class TransactionData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TRAN_ID { get; set; }
        public string? MESSAGE_TYPE { get; set; }
        public string? ORIGINATOR { get; set; }
        public string? TRAN_TYPE { get; set; }
        public string? PAN { get; set; }
        public string? DATE_TIME { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal TRAN_CHARGE { get; set; }
        public string? TRACE { get; set; }
        public string? ACQUIRER_ID { get; set; }
        public string? TERM_ID { get; set; }
        public string? ACCOUNT_NUMBER { get; set; }
        public string? POSTING_TYPE { get; set; }
        public string? OLD_DATE_TIME { get; set; }
        public string? OLD_TRACE { get; set; }
        public string? OLD_ACQUIRER_ID { get; set; }
        public string? RESP_CODE { get; set; }
        public decimal LEDGER_BALANCE { get; set; }
        public string? VCH_NO { get; set; }
        public string? ERR_DESC { get; set; }
        public string? SYS_DATE_TIME { get; set; }
        public string? ISSUER_ID { get; set; }
        public decimal WAIVER_AMOUNT { get; set; }
    }
}
