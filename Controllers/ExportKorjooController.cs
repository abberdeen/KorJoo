using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using KorJoo.Data;

namespace KorJoo.Controllers
{
    public partial class ExportkorjooController : ExportController
    {
        private readonly korjooContext context;
        private readonly korjooService service;

        public ExportkorjooController(korjooContext context, korjooService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/korjoo/applicantcoursestests/csv")]
        [HttpGet("/export/korjoo/applicantcoursestests/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantCoursesTestsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetApplicantCoursesTests(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicantcoursestests/excel")]
        [HttpGet("/export/korjoo/applicantcoursestests/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantCoursesTestsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetApplicantCoursesTests(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicanteducationhistories/csv")]
        [HttpGet("/export/korjoo/applicanteducationhistories/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantEducationHistoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetApplicantEducationHistories(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicanteducationhistories/excel")]
        [HttpGet("/export/korjoo/applicanteducationhistories/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantEducationHistoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetApplicantEducationHistories(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicantlanguages/csv")]
        [HttpGet("/export/korjoo/applicantlanguages/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantLanguagesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetApplicantLanguages(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicantlanguages/excel")]
        [HttpGet("/export/korjoo/applicantlanguages/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantLanguagesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetApplicantLanguages(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicantportofolios/csv")]
        [HttpGet("/export/korjoo/applicantportofolios/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantPortofoliosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetApplicantPortofolios(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicantportofolios/excel")]
        [HttpGet("/export/korjoo/applicantportofolios/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantPortofoliosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetApplicantPortofolios(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicantskills/csv")]
        [HttpGet("/export/korjoo/applicantskills/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantSkillsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetApplicantSkills(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicantskills/excel")]
        [HttpGet("/export/korjoo/applicantskills/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantSkillsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetApplicantSkills(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicantworkhistories/csv")]
        [HttpGet("/export/korjoo/applicantworkhistories/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantWorkHistoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetApplicantWorkHistories(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicantworkhistories/excel")]
        [HttpGet("/export/korjoo/applicantworkhistories/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantWorkHistoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetApplicantWorkHistories(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicants/csv")]
        [HttpGet("/export/korjoo/applicants/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetApplicants(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/applicants/excel")]
        [HttpGet("/export/korjoo/applicants/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportApplicantsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetApplicants(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/citiies/csv")]
        [HttpGet("/export/korjoo/citiies/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCitiiesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCitiies(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/citiies/excel")]
        [HttpGet("/export/korjoo/citiies/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCitiiesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCitiies(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/companies/csv")]
        [HttpGet("/export/korjoo/companies/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompaniesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCompanies(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/companies/excel")]
        [HttpGet("/export/korjoo/companies/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompaniesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCompanies(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/joblanguages/csv")]
        [HttpGet("/export/korjoo/joblanguages/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportJobLanguagesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetJobLanguages(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/joblanguages/excel")]
        [HttpGet("/export/korjoo/joblanguages/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportJobLanguagesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetJobLanguages(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/jobskills/csv")]
        [HttpGet("/export/korjoo/jobskills/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportJobSkillsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetJobSkills(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/jobskills/excel")]
        [HttpGet("/export/korjoo/jobskills/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportJobSkillsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetJobSkills(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/jobs/csv")]
        [HttpGet("/export/korjoo/jobs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportJobsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetJobs(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/jobs/excel")]
        [HttpGet("/export/korjoo/jobs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportJobsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetJobs(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/skills/csv")]
        [HttpGet("/export/korjoo/skills/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSkillsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSkills(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/skills/excel")]
        [HttpGet("/export/korjoo/skills/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSkillsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSkills(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/speccategories/csv")]
        [HttpGet("/export/korjoo/speccategories/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSpecCategoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSpecCategories(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/speccategories/excel")]
        [HttpGet("/export/korjoo/speccategories/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSpecCategoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSpecCategories(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/specializations/csv")]
        [HttpGet("/export/korjoo/specializations/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSpecializationsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSpecializations(), Request.Query), fileName);
        }

        [HttpGet("/export/korjoo/specializations/excel")]
        [HttpGet("/export/korjoo/specializations/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSpecializationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSpecializations(), Request.Query), fileName);
        }
    }
}
