using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("jobs")]
    public partial class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? SpecializationId { get; set; }

        public int? CityId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int? SalaryFrom { get; set; }

        public int? SalaryTo { get; set; }

        public bool? SalaryAfterTax { get; set; }

        public int? SalaryCurrency { get; set; }

        public int? ExperienceYear { get; set; }

        public string Description { get; set; }

        public int? ContactPersonId { get; set; }

        public bool? ContactPersonVisible { get; set; }

        public bool? WorkOnlyOnWeekendDays { get; set; }

        public int? HalfShifts { get; set; }

        public int? EmploymentType { get; set; }

        public int? WorkScheduleType { get; set; }

        public int? CompanyId { get; set; }

        public ICollection<JobLanguage> JobLanguages { get; set; }

        public ICollection<JobSkill> JobSkills { get; set; }

    }
}