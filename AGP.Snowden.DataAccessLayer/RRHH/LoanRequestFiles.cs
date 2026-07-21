using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.DataAccessLayer.RRHH
{
    [Table("LoanRequestFiles", Schema = "RRHH")]
    public partial class LoanRequestFile
    {
        public int Id { get; set; }

        public int  LoanRequestId { get; set; }
        
        [Required]
        public string FileName { get; set; }
        [Required]
        public string OriginalFileName { get; set; }
        [Required]
        public string? FileType { get; set; }
        [Required]
        public string? ContentType { get; set; }
        public Int64 FileSize { get; set; }
        public DateTime? UploadedAt { get; set; }
        public string? UploadedBy { get; set; }
        public bool Active { get; set; }

    }
}
