using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("applicant_skills")]
    public partial class ApplicantSkill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ApplicantId { get; set; }

        public int? SkillId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Applicant Applicant { get; set; }

        public Skill Skill { get; set; }

    }
}