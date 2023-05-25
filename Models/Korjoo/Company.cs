using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("companies")]
    public partial class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int? TypeId { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        public string Instagram { get; set; }

        public string Telegram { get; set; }

        public string Facebook { get; set; }

        public string YouTube { get; set; }

        public string Website { get; set; }

    }
}