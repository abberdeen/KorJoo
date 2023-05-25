using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("job_skills")]
    public partial class JobSkill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? JobId { get; set; }

        public int? SkillId { get; set; }

        public Job Job { get; set; }

        public Skill Skill { get; set; }

    }
}