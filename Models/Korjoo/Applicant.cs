using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KorJoo.Models.korjoo
{
    [Table("applicants")]
    public partial class Applicant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? LivingCityId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FatherName { get; set; }

        public int? GenderType { get; set; }

        public DateTime? Birthday { get; set; }

        public string Phone { get; set; }

        public byte? IsPhoneDesired { get; set; }

        public string PhoneComment { get; set; }

        public string Email { get; set; }

        public bool? IsEmailDesired { get; set; }

        public string Linkedin { get; set; }

        public string Telegram { get; set; }

        public string Skype { get; set; }

        public string Stackoverflow { get; set; }

        public string Github { get; set; }

        public string Dribble { get; set; }

        public string Behance { get; set; }

        public string DesiredJobName { get; set; }

        public int? SpecializationId { get; set; }

        public int? Salary { get; set; }

        public int? SalaryCurrency { get; set; }

        public int? EmploymentType { get; set; }

        public int? WorkSchedule { get; set; }

        public string About { get; set; }

        public ICollection<ApplicantCoursesTest> ApplicantCoursesTests { get; set; }

        public ICollection<ApplicantEducationHistory> ApplicantEducationHistories { get; set; }

        public ICollection<ApplicantPortofolio> ApplicantPortofolios { get; set; }

        public ICollection<ApplicantSkill> ApplicantSkills { get; set; }

        public ICollection<ApplicantWorkHistory> ApplicantWorkHistories { get; set; }

    }
}