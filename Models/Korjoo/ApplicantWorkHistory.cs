using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("applicant_work_history")]
    public partial class ApplicantWorkHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ApplicantId { get; set; }

        public int? CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string Position { get; set; }

        public string Location { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsUntilNow { get; set; }

        public string Description { get; set; }

        public bool? IsVoluntering { get; set; }

        public Applicant Applicant { get; set; }

    }
}