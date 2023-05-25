using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("applicant_languages")]
    public partial class ApplicantLanguage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ApplicantId { get; set; }

        public int? Language { get; set; }

        public int? LanguageLevel { get; set; }

    }
}