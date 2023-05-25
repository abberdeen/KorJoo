using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("applicant_education_history")]
    public partial class ApplicantEducationHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ApplicantId { get; set; }

        public int? TypeId { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string Faculty { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsUntilNow { get; set; }

        public string Description { get; set; }

        public int? Level { get; set; }

        public string Specialization { get; set; }

        public Applicant Applicant { get; set; }

    }
}