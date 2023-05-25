using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("citiies")]
    public partial class Citiie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? RegionId { get; set; }

        public string Name { get; set; }

        public int? Order { get; set; }

    }
}