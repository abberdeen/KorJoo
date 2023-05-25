using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("job_languages")]
    public partial class JobLanguage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? JobId { get; set; }

        public int? Language { get; set; }

        public int? LanguageLevel { get; set; }

        public Job Job { get; set; }

    }
}