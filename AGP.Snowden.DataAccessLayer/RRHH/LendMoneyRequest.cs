using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.DataAccessLayer.RRHH
{
    [Table("LoanMoneyRequest", Schema = "RRHH")]
    public partial class LoanMoneyRequest
    {
        public int Id { get; set; }
        public string? DocumentNumber { get; set; }
        public string? CellNumber { get; set; }
        public string? RequestType { get; set; }
        public decimal? Salary { get; set; }
        public decimal? AmountRequested { get; set; }
        public int? Installments { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public int ReasonLendRequestId { get; set; }
        public string? ReasonLendRequestDescription { get; set; }
        public string?  BeneficiaryDocumentNumber { get; set; }
        public bool  HasFormat { get; set; }
        public bool HasSupport { get; set; }
        public string?  Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Active { get; set; }

        public DateTime? PaymenDate { get; set; }
        public DateTime? FirstDeductionDate { get; set; }
        public DateTime? LastDeductionDate { get; set; }

        public string? FormatFileName { get; set; }
        public string? SupportFileName { get; set; }

        [NotMapped]
        public string? StatusDescription { get; set; }

    }
}
