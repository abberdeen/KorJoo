using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("applicant_courses_tests")]
    public partial class ApplicantCoursesTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ApplicantId { get; set; }

        public int? Type { get; set; }

        public string Title { get; set; }

        public string Organization { get; set; }

        public int? OrganizationCompanyId { get; set; }

        public string Specialization { get; set; }

        public DateTime? IssueDate { get; set; }

        public string CredentialUrl { get; set; }

        public Applicant Applicant { get; set; }

    }
}