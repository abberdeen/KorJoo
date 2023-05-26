using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using KorJoo.Data;

namespace KorJoo
{
    public partial class korjooService
    {
        korjooContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly korjooContext context;
        private readonly NavigationManager navigationManager;

        public korjooService(korjooContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportApplicantCoursesTestsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicantcoursestests/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicantcoursestests/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportApplicantCoursesTestsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicantcoursestests/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicantcoursestests/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnApplicantCoursesTestsRead(ref IQueryable<KorJoo.Models.korjoo.ApplicantCoursesTest> items);

        public async Task<int> GetApplicantIdByUserId(string userId)
        {
            var applicant  = Context.Applicants
                              .AsNoTracking()
                              .FirstOrDefault(i => i.UserId == userId);
 
            return await Task.FromResult(applicant.Id);
        }

        public async Task<int> GetCompanyIdByUserId(string userId)
        {
            var company = Context.Companies
                              .AsNoTracking()
                              .FirstOrDefault(i => i.UserId == userId);

            return await Task.FromResult(company.Id);
        }

        public async Task<IQueryable<KorJoo.Models.korjoo.ApplicantCoursesTest>> GetApplicantCoursesTests(Query query = null)
        {
            var items = Context.ApplicantCoursesTests.AsQueryable();

            items = items.Include(i => i.Applicant);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnApplicantCoursesTestsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnApplicantCoursesTestGet(KorJoo.Models.korjoo.ApplicantCoursesTest item);
        partial void OnGetApplicantCoursesTestById(ref IQueryable<KorJoo.Models.korjoo.ApplicantCoursesTest> items);


        public async Task<KorJoo.Models.korjoo.ApplicantCoursesTest> GetApplicantCoursesTestById(int id)
        {
            var items = Context.ApplicantCoursesTests
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Applicant);
 
            OnGetApplicantCoursesTestById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnApplicantCoursesTestGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnApplicantCoursesTestCreated(KorJoo.Models.korjoo.ApplicantCoursesTest item);
        partial void OnAfterApplicantCoursesTestCreated(KorJoo.Models.korjoo.ApplicantCoursesTest item);

        public async Task<KorJoo.Models.korjoo.ApplicantCoursesTest> CreateApplicantCoursesTest(KorJoo.Models.korjoo.ApplicantCoursesTest applicantcoursestest)
        {
            OnApplicantCoursesTestCreated(applicantcoursestest);

            var existingItem = Context.ApplicantCoursesTests
                              .Where(i => i.Id == applicantcoursestest.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ApplicantCoursesTests.Add(applicantcoursestest);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(applicantcoursestest).State = EntityState.Detached;
                throw;
            }

            OnAfterApplicantCoursesTestCreated(applicantcoursestest);

            return applicantcoursestest;
        }

        public async Task<KorJoo.Models.korjoo.ApplicantCoursesTest> CancelApplicantCoursesTestChanges(KorJoo.Models.korjoo.ApplicantCoursesTest item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnApplicantCoursesTestUpdated(KorJoo.Models.korjoo.ApplicantCoursesTest item);
        partial void OnAfterApplicantCoursesTestUpdated(KorJoo.Models.korjoo.ApplicantCoursesTest item);

        public async Task<KorJoo.Models.korjoo.ApplicantCoursesTest> UpdateApplicantCoursesTest(int id, KorJoo.Models.korjoo.ApplicantCoursesTest applicantcoursestest)
        {
            OnApplicantCoursesTestUpdated(applicantcoursestest);

            var itemToUpdate = Context.ApplicantCoursesTests
                              .Where(i => i.Id == applicantcoursestest.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(applicantcoursestest);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterApplicantCoursesTestUpdated(applicantcoursestest);

            return applicantcoursestest;
        }

        partial void OnApplicantCoursesTestDeleted(KorJoo.Models.korjoo.ApplicantCoursesTest item);
        partial void OnAfterApplicantCoursesTestDeleted(KorJoo.Models.korjoo.ApplicantCoursesTest item);

        public async Task<KorJoo.Models.korjoo.ApplicantCoursesTest> DeleteApplicantCoursesTest(int id)
        {
            var itemToDelete = Context.ApplicantCoursesTests
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnApplicantCoursesTestDeleted(itemToDelete);


            Context.ApplicantCoursesTests.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterApplicantCoursesTestDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportApplicantEducationHistoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicanteducationhistories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicanteducationhistories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportApplicantEducationHistoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicanteducationhistories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicanteducationhistories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnApplicantEducationHistoriesRead(ref IQueryable<KorJoo.Models.korjoo.ApplicantEducationHistory> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.ApplicantEducationHistory>> GetApplicantEducationHistories(Query query = null)
        {
            var items = Context.ApplicantEducationHistories.AsQueryable();

            items = items.Include(i => i.Applicant);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnApplicantEducationHistoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnApplicantEducationHistoryGet(KorJoo.Models.korjoo.ApplicantEducationHistory item);
        partial void OnGetApplicantEducationHistoryById(ref IQueryable<KorJoo.Models.korjoo.ApplicantEducationHistory> items);


        public async Task<KorJoo.Models.korjoo.ApplicantEducationHistory> GetApplicantEducationHistoryById(int id)
        {
            var items = Context.ApplicantEducationHistories
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Applicant);
 
            OnGetApplicantEducationHistoryById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnApplicantEducationHistoryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnApplicantEducationHistoryCreated(KorJoo.Models.korjoo.ApplicantEducationHistory item);
        partial void OnAfterApplicantEducationHistoryCreated(KorJoo.Models.korjoo.ApplicantEducationHistory item);

        public async Task<KorJoo.Models.korjoo.ApplicantEducationHistory> CreateApplicantEducationHistory(KorJoo.Models.korjoo.ApplicantEducationHistory applicanteducationhistory)
        {
            OnApplicantEducationHistoryCreated(applicanteducationhistory);

            var existingItem = Context.ApplicantEducationHistories
                              .Where(i => i.Id == applicanteducationhistory.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ApplicantEducationHistories.Add(applicanteducationhistory);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(applicanteducationhistory).State = EntityState.Detached;
                throw;
            }

            OnAfterApplicantEducationHistoryCreated(applicanteducationhistory);

            return applicanteducationhistory;
        }

        public async Task<KorJoo.Models.korjoo.ApplicantEducationHistory> CancelApplicantEducationHistoryChanges(KorJoo.Models.korjoo.ApplicantEducationHistory item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnApplicantEducationHistoryUpdated(KorJoo.Models.korjoo.ApplicantEducationHistory item);
        partial void OnAfterApplicantEducationHistoryUpdated(KorJoo.Models.korjoo.ApplicantEducationHistory item);

        public async Task<KorJoo.Models.korjoo.ApplicantEducationHistory> UpdateApplicantEducationHistory(int id, KorJoo.Models.korjoo.ApplicantEducationHistory applicanteducationhistory)
        {
            OnApplicantEducationHistoryUpdated(applicanteducationhistory);

            var itemToUpdate = Context.ApplicantEducationHistories
                              .Where(i => i.Id == applicanteducationhistory.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(applicanteducationhistory);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterApplicantEducationHistoryUpdated(applicanteducationhistory);

            return applicanteducationhistory;
        }

        partial void OnApplicantEducationHistoryDeleted(KorJoo.Models.korjoo.ApplicantEducationHistory item);
        partial void OnAfterApplicantEducationHistoryDeleted(KorJoo.Models.korjoo.ApplicantEducationHistory item);

        public async Task<KorJoo.Models.korjoo.ApplicantEducationHistory> DeleteApplicantEducationHistory(int id)
        {
            var itemToDelete = Context.ApplicantEducationHistories
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnApplicantEducationHistoryDeleted(itemToDelete);


            Context.ApplicantEducationHistories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterApplicantEducationHistoryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportApplicantLanguagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicantlanguages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicantlanguages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportApplicantLanguagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicantlanguages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicantlanguages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnApplicantLanguagesRead(ref IQueryable<KorJoo.Models.korjoo.ApplicantLanguage> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.ApplicantLanguage>> GetApplicantLanguages(Query query = null)
        {
            var items = Context.ApplicantLanguages.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnApplicantLanguagesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnApplicantLanguageGet(KorJoo.Models.korjoo.ApplicantLanguage item);
        partial void OnGetApplicantLanguageById(ref IQueryable<KorJoo.Models.korjoo.ApplicantLanguage> items);


        public async Task<KorJoo.Models.korjoo.ApplicantLanguage> GetApplicantLanguageById(int id)
        {
            var items = Context.ApplicantLanguages
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetApplicantLanguageById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnApplicantLanguageGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnApplicantLanguageCreated(KorJoo.Models.korjoo.ApplicantLanguage item);
        partial void OnAfterApplicantLanguageCreated(KorJoo.Models.korjoo.ApplicantLanguage item);

        public async Task<KorJoo.Models.korjoo.ApplicantLanguage> CreateApplicantLanguage(KorJoo.Models.korjoo.ApplicantLanguage applicantlanguage)
        {
            OnApplicantLanguageCreated(applicantlanguage);

            var existingItem = Context.ApplicantLanguages
                              .Where(i => i.Id == applicantlanguage.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ApplicantLanguages.Add(applicantlanguage);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(applicantlanguage).State = EntityState.Detached;
                throw;
            }

            OnAfterApplicantLanguageCreated(applicantlanguage);

            return applicantlanguage;
        }

        public async Task<KorJoo.Models.korjoo.ApplicantLanguage> CancelApplicantLanguageChanges(KorJoo.Models.korjoo.ApplicantLanguage item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnApplicantLanguageUpdated(KorJoo.Models.korjoo.ApplicantLanguage item);
        partial void OnAfterApplicantLanguageUpdated(KorJoo.Models.korjoo.ApplicantLanguage item);

        public async Task<KorJoo.Models.korjoo.ApplicantLanguage> UpdateApplicantLanguage(int id, KorJoo.Models.korjoo.ApplicantLanguage applicantlanguage)
        {
            OnApplicantLanguageUpdated(applicantlanguage);

            var itemToUpdate = Context.ApplicantLanguages
                              .Where(i => i.Id == applicantlanguage.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(applicantlanguage);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterApplicantLanguageUpdated(applicantlanguage);

            return applicantlanguage;
        }

        partial void OnApplicantLanguageDeleted(KorJoo.Models.korjoo.ApplicantLanguage item);
        partial void OnAfterApplicantLanguageDeleted(KorJoo.Models.korjoo.ApplicantLanguage item);

        public async Task<KorJoo.Models.korjoo.ApplicantLanguage> DeleteApplicantLanguage(int id)
        {
            var itemToDelete = Context.ApplicantLanguages
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnApplicantLanguageDeleted(itemToDelete);


            Context.ApplicantLanguages.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterApplicantLanguageDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportApplicantPortofoliosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicantportofolios/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicantportofolios/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportApplicantPortofoliosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicantportofolios/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicantportofolios/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnApplicantPortofoliosRead(ref IQueryable<KorJoo.Models.korjoo.ApplicantPortofolio> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.ApplicantPortofolio>> GetApplicantPortofolios(Query query = null)
        {
            var items = Context.ApplicantPortofolios.AsQueryable();

            items = items.Include(i => i.Applicant);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnApplicantPortofoliosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnApplicantPortofolioGet(KorJoo.Models.korjoo.ApplicantPortofolio item);
        partial void OnGetApplicantPortofolioById(ref IQueryable<KorJoo.Models.korjoo.ApplicantPortofolio> items);


        public async Task<KorJoo.Models.korjoo.ApplicantPortofolio> GetApplicantPortofolioById(int id)
        {
            var items = Context.ApplicantPortofolios
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Applicant);
 
            OnGetApplicantPortofolioById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnApplicantPortofolioGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnApplicantPortofolioCreated(KorJoo.Models.korjoo.ApplicantPortofolio item);
        partial void OnAfterApplicantPortofolioCreated(KorJoo.Models.korjoo.ApplicantPortofolio item);

        public async Task<KorJoo.Models.korjoo.ApplicantPortofolio> CreateApplicantPortofolio(KorJoo.Models.korjoo.ApplicantPortofolio applicantportofolio)
        {
            OnApplicantPortofolioCreated(applicantportofolio);

            var existingItem = Context.ApplicantPortofolios
                              .Where(i => i.Id == applicantportofolio.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ApplicantPortofolios.Add(applicantportofolio);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(applicantportofolio).State = EntityState.Detached;
                throw;
            }

            OnAfterApplicantPortofolioCreated(applicantportofolio);

            return applicantportofolio;
        }

        public async Task<KorJoo.Models.korjoo.ApplicantPortofolio> CancelApplicantPortofolioChanges(KorJoo.Models.korjoo.ApplicantPortofolio item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnApplicantPortofolioUpdated(KorJoo.Models.korjoo.ApplicantPortofolio item);
        partial void OnAfterApplicantPortofolioUpdated(KorJoo.Models.korjoo.ApplicantPortofolio item);

        public async Task<KorJoo.Models.korjoo.ApplicantPortofolio> UpdateApplicantPortofolio(int id, KorJoo.Models.korjoo.ApplicantPortofolio applicantportofolio)
        {
            OnApplicantPortofolioUpdated(applicantportofolio);

            var itemToUpdate = Context.ApplicantPortofolios
                              .Where(i => i.Id == applicantportofolio.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(applicantportofolio);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterApplicantPortofolioUpdated(applicantportofolio);

            return applicantportofolio;
        }

        partial void OnApplicantPortofolioDeleted(KorJoo.Models.korjoo.ApplicantPortofolio item);
        partial void OnAfterApplicantPortofolioDeleted(KorJoo.Models.korjoo.ApplicantPortofolio item);

        public async Task<KorJoo.Models.korjoo.ApplicantPortofolio> DeleteApplicantPortofolio(int id)
        {
            var itemToDelete = Context.ApplicantPortofolios
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnApplicantPortofolioDeleted(itemToDelete);


            Context.ApplicantPortofolios.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterApplicantPortofolioDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportApplicantSkillsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicantskills/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicantskills/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportApplicantSkillsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicantskills/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicantskills/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnApplicantSkillsRead(ref IQueryable<KorJoo.Models.korjoo.ApplicantSkill> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.ApplicantSkill>> GetApplicantSkills(Query query = null)
        {
            var items = Context.ApplicantSkills.AsQueryable();

            items = items.Include(i => i.Applicant);
            items = items.Include(i => i.Skill);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnApplicantSkillsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnApplicantSkillGet(KorJoo.Models.korjoo.ApplicantSkill item);
        partial void OnGetApplicantSkillById(ref IQueryable<KorJoo.Models.korjoo.ApplicantSkill> items);


        public async Task<KorJoo.Models.korjoo.ApplicantSkill> GetApplicantSkillById(int id)
        {
            var items = Context.ApplicantSkills
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Applicant);
            items = items.Include(i => i.Skill);
 
            OnGetApplicantSkillById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnApplicantSkillGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnApplicantSkillCreated(KorJoo.Models.korjoo.ApplicantSkill item);
        partial void OnAfterApplicantSkillCreated(KorJoo.Models.korjoo.ApplicantSkill item);

        public async Task<KorJoo.Models.korjoo.ApplicantSkill> CreateApplicantSkill(KorJoo.Models.korjoo.ApplicantSkill applicantskill)
        {
            OnApplicantSkillCreated(applicantskill);

            var existingItem = Context.ApplicantSkills
                              .Where(i => i.Id == applicantskill.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ApplicantSkills.Add(applicantskill);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(applicantskill).State = EntityState.Detached;
                throw;
            }

            OnAfterApplicantSkillCreated(applicantskill);

            return applicantskill;
        }

        public async Task<KorJoo.Models.korjoo.ApplicantSkill> CancelApplicantSkillChanges(KorJoo.Models.korjoo.ApplicantSkill item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnApplicantSkillUpdated(KorJoo.Models.korjoo.ApplicantSkill item);
        partial void OnAfterApplicantSkillUpdated(KorJoo.Models.korjoo.ApplicantSkill item);

        public async Task<KorJoo.Models.korjoo.ApplicantSkill> UpdateApplicantSkill(int id, KorJoo.Models.korjoo.ApplicantSkill applicantskill)
        {
            OnApplicantSkillUpdated(applicantskill);

            var itemToUpdate = Context.ApplicantSkills
                              .Where(i => i.Id == applicantskill.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(applicantskill);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterApplicantSkillUpdated(applicantskill);

            return applicantskill;
        }

        partial void OnApplicantSkillDeleted(KorJoo.Models.korjoo.ApplicantSkill item);
        partial void OnAfterApplicantSkillDeleted(KorJoo.Models.korjoo.ApplicantSkill item);

        public async Task<KorJoo.Models.korjoo.ApplicantSkill> DeleteApplicantSkill(int id)
        {
            var itemToDelete = Context.ApplicantSkills
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnApplicantSkillDeleted(itemToDelete);


            Context.ApplicantSkills.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterApplicantSkillDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportApplicantWorkHistoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicantworkhistories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicantworkhistories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportApplicantWorkHistoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicantworkhistories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicantworkhistories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnApplicantWorkHistoriesRead(ref IQueryable<KorJoo.Models.korjoo.ApplicantWorkHistory> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.ApplicantWorkHistory>> GetApplicantWorkHistories(Query query = null)
        {
            var items = Context.ApplicantWorkHistories.AsQueryable();

            items = items.Include(i => i.Applicant);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnApplicantWorkHistoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnApplicantWorkHistoryGet(KorJoo.Models.korjoo.ApplicantWorkHistory item);
        partial void OnGetApplicantWorkHistoryById(ref IQueryable<KorJoo.Models.korjoo.ApplicantWorkHistory> items);


        public async Task<KorJoo.Models.korjoo.ApplicantWorkHistory> GetApplicantWorkHistoryById(int id)
        {
            var items = Context.ApplicantWorkHistories
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Applicant);
 
            OnGetApplicantWorkHistoryById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnApplicantWorkHistoryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnApplicantWorkHistoryCreated(KorJoo.Models.korjoo.ApplicantWorkHistory item);
        partial void OnAfterApplicantWorkHistoryCreated(KorJoo.Models.korjoo.ApplicantWorkHistory item);

        public async Task<KorJoo.Models.korjoo.ApplicantWorkHistory> CreateApplicantWorkHistory(KorJoo.Models.korjoo.ApplicantWorkHistory applicantworkhistory)
        {
            OnApplicantWorkHistoryCreated(applicantworkhistory);

            var existingItem = Context.ApplicantWorkHistories
                              .Where(i => i.Id == applicantworkhistory.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ApplicantWorkHistories.Add(applicantworkhistory);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(applicantworkhistory).State = EntityState.Detached;
                throw;
            }

            OnAfterApplicantWorkHistoryCreated(applicantworkhistory);

            return applicantworkhistory;
        }

        public async Task<KorJoo.Models.korjoo.ApplicantWorkHistory> CancelApplicantWorkHistoryChanges(KorJoo.Models.korjoo.ApplicantWorkHistory item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnApplicantWorkHistoryUpdated(KorJoo.Models.korjoo.ApplicantWorkHistory item);
        partial void OnAfterApplicantWorkHistoryUpdated(KorJoo.Models.korjoo.ApplicantWorkHistory item);

        public async Task<KorJoo.Models.korjoo.ApplicantWorkHistory> UpdateApplicantWorkHistory(int id, KorJoo.Models.korjoo.ApplicantWorkHistory applicantworkhistory)
        {
            OnApplicantWorkHistoryUpdated(applicantworkhistory);

            var itemToUpdate = Context.ApplicantWorkHistories
                              .Where(i => i.Id == applicantworkhistory.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(applicantworkhistory);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterApplicantWorkHistoryUpdated(applicantworkhistory);

            return applicantworkhistory;
        }

        partial void OnApplicantWorkHistoryDeleted(KorJoo.Models.korjoo.ApplicantWorkHistory item);
        partial void OnAfterApplicantWorkHistoryDeleted(KorJoo.Models.korjoo.ApplicantWorkHistory item);

        public async Task<KorJoo.Models.korjoo.ApplicantWorkHistory> DeleteApplicantWorkHistory(int id)
        {
            var itemToDelete = Context.ApplicantWorkHistories
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnApplicantWorkHistoryDeleted(itemToDelete);


            Context.ApplicantWorkHistories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterApplicantWorkHistoryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportApplicantsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicants/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicants/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportApplicantsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/applicants/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/applicants/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnApplicantsRead(ref IQueryable<KorJoo.Models.korjoo.Applicant> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.Applicant>> GetApplicants(Query query = null)
        {
            var items = Context.Applicants.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnApplicantsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnApplicantGet(KorJoo.Models.korjoo.Applicant item);
        partial void OnGetApplicantById(ref IQueryable<KorJoo.Models.korjoo.Applicant> items);


        public async Task<KorJoo.Models.korjoo.Applicant> GetApplicantById(int id)
        {
            var items = Context.Applicants
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetApplicantById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnApplicantGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnApplicantCreated(KorJoo.Models.korjoo.Applicant item);
        partial void OnAfterApplicantCreated(KorJoo.Models.korjoo.Applicant item);

        public async Task<KorJoo.Models.korjoo.Applicant> CreateApplicant(KorJoo.Models.korjoo.Applicant applicant)
        {
            OnApplicantCreated(applicant);

            var existingItem = Context.Applicants
                              .Where(i => i.Id == applicant.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Applicants.Add(applicant);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(applicant).State = EntityState.Detached;
                throw;
            }

            OnAfterApplicantCreated(applicant);

            return applicant;
        }

        public async Task<KorJoo.Models.korjoo.Applicant> CancelApplicantChanges(KorJoo.Models.korjoo.Applicant item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnApplicantUpdated(KorJoo.Models.korjoo.Applicant item);
        partial void OnAfterApplicantUpdated(KorJoo.Models.korjoo.Applicant item);

        public async Task<KorJoo.Models.korjoo.Applicant> UpdateApplicant(int id, KorJoo.Models.korjoo.Applicant applicant)
        {
            OnApplicantUpdated(applicant);

            var itemToUpdate = Context.Applicants
                              .Where(i => i.Id == applicant.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(applicant);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterApplicantUpdated(applicant);

            return applicant;
        }

        partial void OnApplicantDeleted(KorJoo.Models.korjoo.Applicant item);
        partial void OnAfterApplicantDeleted(KorJoo.Models.korjoo.Applicant item);

        public async Task<KorJoo.Models.korjoo.Applicant> DeleteApplicant(int id)
        {
            var itemToDelete = Context.Applicants
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnApplicantDeleted(itemToDelete);


            Context.Applicants.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterApplicantDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCitiiesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/citiies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/citiies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCitiiesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/citiies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/citiies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCitiiesRead(ref IQueryable<KorJoo.Models.korjoo.Citiie> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.Citiie>> GetCitiies(Query query = null)
        {
            var items = Context.Citiies.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCitiiesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCitiieGet(KorJoo.Models.korjoo.Citiie item);
        partial void OnGetCitiieById(ref IQueryable<KorJoo.Models.korjoo.Citiie> items);


        public async Task<KorJoo.Models.korjoo.Citiie> GetCitiieById(int id)
        {
            var items = Context.Citiies
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetCitiieById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCitiieGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCitiieCreated(KorJoo.Models.korjoo.Citiie item);
        partial void OnAfterCitiieCreated(KorJoo.Models.korjoo.Citiie item);

        public async Task<KorJoo.Models.korjoo.Citiie> CreateCitiie(KorJoo.Models.korjoo.Citiie citiie)
        {
            OnCitiieCreated(citiie);

            var existingItem = Context.Citiies
                              .Where(i => i.Id == citiie.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Citiies.Add(citiie);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(citiie).State = EntityState.Detached;
                throw;
            }

            OnAfterCitiieCreated(citiie);

            return citiie;
        }

        public async Task<KorJoo.Models.korjoo.Citiie> CancelCitiieChanges(KorJoo.Models.korjoo.Citiie item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCitiieUpdated(KorJoo.Models.korjoo.Citiie item);
        partial void OnAfterCitiieUpdated(KorJoo.Models.korjoo.Citiie item);

        public async Task<KorJoo.Models.korjoo.Citiie> UpdateCitiie(int id, KorJoo.Models.korjoo.Citiie citiie)
        {
            OnCitiieUpdated(citiie);

            var itemToUpdate = Context.Citiies
                              .Where(i => i.Id == citiie.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(citiie);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCitiieUpdated(citiie);

            return citiie;
        }

        partial void OnCitiieDeleted(KorJoo.Models.korjoo.Citiie item);
        partial void OnAfterCitiieDeleted(KorJoo.Models.korjoo.Citiie item);

        public async Task<KorJoo.Models.korjoo.Citiie> DeleteCitiie(int id)
        {
            var itemToDelete = Context.Citiies
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCitiieDeleted(itemToDelete);


            Context.Citiies.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCitiieDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCompaniesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/companies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/companies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCompaniesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/companies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/companies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCompaniesRead(ref IQueryable<KorJoo.Models.korjoo.Company> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.Company>> GetCompanies(Query query = null)
        {
            var items = Context.Companies.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCompaniesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCompanyGet(KorJoo.Models.korjoo.Company item);
        partial void OnGetCompanyById(ref IQueryable<KorJoo.Models.korjoo.Company> items);


        public async Task<KorJoo.Models.korjoo.Company> GetCompanyById(int id)
        {
            var items = Context.Companies
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetCompanyById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCompanyGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCompanyCreated(KorJoo.Models.korjoo.Company item);
        partial void OnAfterCompanyCreated(KorJoo.Models.korjoo.Company item);

        public async Task<KorJoo.Models.korjoo.Company> CreateCompany(KorJoo.Models.korjoo.Company company)
        {
            OnCompanyCreated(company);

            var existingItem = Context.Companies
                              .Where(i => i.Id == company.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Companies.Add(company);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(company).State = EntityState.Detached;
                throw;
            }

            OnAfterCompanyCreated(company);

            return company;
        }

        public async Task<KorJoo.Models.korjoo.Company> CancelCompanyChanges(KorJoo.Models.korjoo.Company item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCompanyUpdated(KorJoo.Models.korjoo.Company item);
        partial void OnAfterCompanyUpdated(KorJoo.Models.korjoo.Company item);

        public async Task<KorJoo.Models.korjoo.Company> UpdateCompany(int id, KorJoo.Models.korjoo.Company company)
        {
            OnCompanyUpdated(company);

            var itemToUpdate = Context.Companies
                              .Where(i => i.Id == company.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(company);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCompanyUpdated(company);

            return company;
        }

        partial void OnCompanyDeleted(KorJoo.Models.korjoo.Company item);
        partial void OnAfterCompanyDeleted(KorJoo.Models.korjoo.Company item);

        public async Task<KorJoo.Models.korjoo.Company> DeleteCompany(int id)
        {
            var itemToDelete = Context.Companies
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCompanyDeleted(itemToDelete);


            Context.Companies.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCompanyDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportJobLanguagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/joblanguages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/joblanguages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportJobLanguagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/joblanguages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/joblanguages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnJobLanguagesRead(ref IQueryable<KorJoo.Models.korjoo.JobLanguage> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.JobLanguage>> GetJobLanguages(Query query = null)
        {
            var items = Context.JobLanguages.AsQueryable();

            items = items.Include(i => i.Job);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnJobLanguagesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnJobLanguageGet(KorJoo.Models.korjoo.JobLanguage item);
        partial void OnGetJobLanguageById(ref IQueryable<KorJoo.Models.korjoo.JobLanguage> items);


        public async Task<KorJoo.Models.korjoo.JobLanguage> GetJobLanguageById(int id)
        {
            var items = Context.JobLanguages
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Job);
 
            OnGetJobLanguageById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnJobLanguageGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnJobLanguageCreated(KorJoo.Models.korjoo.JobLanguage item);
        partial void OnAfterJobLanguageCreated(KorJoo.Models.korjoo.JobLanguage item);

        public async Task<KorJoo.Models.korjoo.JobLanguage> CreateJobLanguage(KorJoo.Models.korjoo.JobLanguage joblanguage)
        {
            OnJobLanguageCreated(joblanguage);

            var existingItem = Context.JobLanguages
                              .Where(i => i.Id == joblanguage.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.JobLanguages.Add(joblanguage);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(joblanguage).State = EntityState.Detached;
                throw;
            }

            OnAfterJobLanguageCreated(joblanguage);

            return joblanguage;
        }

        public async Task<KorJoo.Models.korjoo.JobLanguage> CancelJobLanguageChanges(KorJoo.Models.korjoo.JobLanguage item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnJobLanguageUpdated(KorJoo.Models.korjoo.JobLanguage item);
        partial void OnAfterJobLanguageUpdated(KorJoo.Models.korjoo.JobLanguage item);

        public async Task<KorJoo.Models.korjoo.JobLanguage> UpdateJobLanguage(int id, KorJoo.Models.korjoo.JobLanguage joblanguage)
        {
            OnJobLanguageUpdated(joblanguage);

            var itemToUpdate = Context.JobLanguages
                              .Where(i => i.Id == joblanguage.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(joblanguage);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterJobLanguageUpdated(joblanguage);

            return joblanguage;
        }

        partial void OnJobLanguageDeleted(KorJoo.Models.korjoo.JobLanguage item);
        partial void OnAfterJobLanguageDeleted(KorJoo.Models.korjoo.JobLanguage item);

        public async Task<KorJoo.Models.korjoo.JobLanguage> DeleteJobLanguage(int id)
        {
            var itemToDelete = Context.JobLanguages
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnJobLanguageDeleted(itemToDelete);


            Context.JobLanguages.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterJobLanguageDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportJobSkillsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/jobskills/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/jobskills/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportJobSkillsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/jobskills/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/jobskills/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnJobSkillsRead(ref IQueryable<KorJoo.Models.korjoo.JobSkill> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.JobSkill>> GetJobSkills(Query query = null)
        {
            var items = Context.JobSkills.AsQueryable();

            items = items.Include(i => i.Job);
            items = items.Include(i => i.Skill);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnJobSkillsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnJobSkillGet(KorJoo.Models.korjoo.JobSkill item);
        partial void OnGetJobSkillById(ref IQueryable<KorJoo.Models.korjoo.JobSkill> items);


        public async Task<KorJoo.Models.korjoo.JobSkill> GetJobSkillById(int id)
        {
            var items = Context.JobSkills
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Job);
            items = items.Include(i => i.Skill);
 
            OnGetJobSkillById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnJobSkillGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnJobSkillCreated(KorJoo.Models.korjoo.JobSkill item);
        partial void OnAfterJobSkillCreated(KorJoo.Models.korjoo.JobSkill item);

        public async Task<KorJoo.Models.korjoo.JobSkill> CreateJobSkill(KorJoo.Models.korjoo.JobSkill jobskill)
        {
            OnJobSkillCreated(jobskill);

            var existingItem = Context.JobSkills
                              .Where(i => i.Id == jobskill.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.JobSkills.Add(jobskill);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(jobskill).State = EntityState.Detached;
                throw;
            }

            OnAfterJobSkillCreated(jobskill);

            return jobskill;
        }

        public async Task<KorJoo.Models.korjoo.JobSkill> CancelJobSkillChanges(KorJoo.Models.korjoo.JobSkill item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnJobSkillUpdated(KorJoo.Models.korjoo.JobSkill item);
        partial void OnAfterJobSkillUpdated(KorJoo.Models.korjoo.JobSkill item);

        public async Task<KorJoo.Models.korjoo.JobSkill> UpdateJobSkill(int id, KorJoo.Models.korjoo.JobSkill jobskill)
        {
            OnJobSkillUpdated(jobskill);

            var itemToUpdate = Context.JobSkills
                              .Where(i => i.Id == jobskill.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(jobskill);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterJobSkillUpdated(jobskill);

            return jobskill;
        }

        partial void OnJobSkillDeleted(KorJoo.Models.korjoo.JobSkill item);
        partial void OnAfterJobSkillDeleted(KorJoo.Models.korjoo.JobSkill item);

        public async Task<KorJoo.Models.korjoo.JobSkill> DeleteJobSkill(int id)
        {
            var itemToDelete = Context.JobSkills
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnJobSkillDeleted(itemToDelete);


            Context.JobSkills.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterJobSkillDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportJobsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/jobs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/jobs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportJobsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/jobs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/jobs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnJobsRead(ref IQueryable<KorJoo.Models.korjoo.Job> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.Job>> GetJobs(Query query = null)
        {
            var items = Context.Jobs.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnJobsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnJobGet(KorJoo.Models.korjoo.Job item);
        partial void OnGetJobById(ref IQueryable<KorJoo.Models.korjoo.Job> items);


        public async Task<KorJoo.Models.korjoo.Job> GetJobById(int id)
        {
            var items = Context.Jobs
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetJobById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnJobGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnJobCreated(KorJoo.Models.korjoo.Job item);
        partial void OnAfterJobCreated(KorJoo.Models.korjoo.Job item);

        public async Task<KorJoo.Models.korjoo.Job> CreateJob(KorJoo.Models.korjoo.Job job)
        {
            OnJobCreated(job);

            var existingItem = Context.Jobs
                              .Where(i => i.Id == job.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Jobs.Add(job);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(job).State = EntityState.Detached;
                throw;
            }

            OnAfterJobCreated(job);

            return job;
        }

        public async Task<KorJoo.Models.korjoo.Job> CancelJobChanges(KorJoo.Models.korjoo.Job item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnJobUpdated(KorJoo.Models.korjoo.Job item);
        partial void OnAfterJobUpdated(KorJoo.Models.korjoo.Job item);

        public async Task<KorJoo.Models.korjoo.Job> UpdateJob(int id, KorJoo.Models.korjoo.Job job)
        {
            OnJobUpdated(job);

            var itemToUpdate = Context.Jobs
                              .Where(i => i.Id == job.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(job);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterJobUpdated(job);

            return job;
        }

        partial void OnJobDeleted(KorJoo.Models.korjoo.Job item);
        partial void OnAfterJobDeleted(KorJoo.Models.korjoo.Job item);

        public async Task<KorJoo.Models.korjoo.Job> DeleteJob(int id)
        {
            var itemToDelete = Context.Jobs
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnJobDeleted(itemToDelete);


            Context.Jobs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterJobDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSkillsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/skills/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/skills/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSkillsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/skills/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/skills/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSkillsRead(ref IQueryable<KorJoo.Models.korjoo.Skill> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.Skill>> GetSkills(Query query = null)
        {
            var items = Context.Skills.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSkillsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSkillGet(KorJoo.Models.korjoo.Skill item);
        partial void OnGetSkillById(ref IQueryable<KorJoo.Models.korjoo.Skill> items);


        public async Task<KorJoo.Models.korjoo.Skill> GetSkillById(int id)
        {
            var items = Context.Skills
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetSkillById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSkillGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSkillCreated(KorJoo.Models.korjoo.Skill item);
        partial void OnAfterSkillCreated(KorJoo.Models.korjoo.Skill item);

        public async Task<KorJoo.Models.korjoo.Skill> CreateSkill(KorJoo.Models.korjoo.Skill skill)
        {
            OnSkillCreated(skill);

            var existingItem = Context.Skills
                              .Where(i => i.Id == skill.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Skills.Add(skill);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(skill).State = EntityState.Detached;
                throw;
            }

            OnAfterSkillCreated(skill);

            return skill;
        }

        public async Task<KorJoo.Models.korjoo.Skill> CancelSkillChanges(KorJoo.Models.korjoo.Skill item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSkillUpdated(KorJoo.Models.korjoo.Skill item);
        partial void OnAfterSkillUpdated(KorJoo.Models.korjoo.Skill item);

        public async Task<KorJoo.Models.korjoo.Skill> UpdateSkill(int id, KorJoo.Models.korjoo.Skill skill)
        {
            OnSkillUpdated(skill);

            var itemToUpdate = Context.Skills
                              .Where(i => i.Id == skill.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(skill);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSkillUpdated(skill);

            return skill;
        }

        partial void OnSkillDeleted(KorJoo.Models.korjoo.Skill item);
        partial void OnAfterSkillDeleted(KorJoo.Models.korjoo.Skill item);

        public async Task<KorJoo.Models.korjoo.Skill> DeleteSkill(int id)
        {
            var itemToDelete = Context.Skills
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSkillDeleted(itemToDelete);


            Context.Skills.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSkillDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSpecCategoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/speccategories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/speccategories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSpecCategoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/speccategories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/speccategories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSpecCategoriesRead(ref IQueryable<KorJoo.Models.korjoo.SpecCategory> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.SpecCategory>> GetSpecCategories(Query query = null)
        {
            var items = Context.SpecCategories.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSpecCategoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSpecCategoryGet(KorJoo.Models.korjoo.SpecCategory item);
        partial void OnGetSpecCategoryById(ref IQueryable<KorJoo.Models.korjoo.SpecCategory> items);


        public async Task<KorJoo.Models.korjoo.SpecCategory> GetSpecCategoryById(int id)
        {
            var items = Context.SpecCategories
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetSpecCategoryById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSpecCategoryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSpecCategoryCreated(KorJoo.Models.korjoo.SpecCategory item);
        partial void OnAfterSpecCategoryCreated(KorJoo.Models.korjoo.SpecCategory item);

        public async Task<KorJoo.Models.korjoo.SpecCategory> CreateSpecCategory(KorJoo.Models.korjoo.SpecCategory speccategory)
        {
            OnSpecCategoryCreated(speccategory);

            var existingItem = Context.SpecCategories
                              .Where(i => i.Id == speccategory.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SpecCategories.Add(speccategory);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(speccategory).State = EntityState.Detached;
                throw;
            }

            OnAfterSpecCategoryCreated(speccategory);

            return speccategory;
        }

        public async Task<KorJoo.Models.korjoo.SpecCategory> CancelSpecCategoryChanges(KorJoo.Models.korjoo.SpecCategory item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSpecCategoryUpdated(KorJoo.Models.korjoo.SpecCategory item);
        partial void OnAfterSpecCategoryUpdated(KorJoo.Models.korjoo.SpecCategory item);

        public async Task<KorJoo.Models.korjoo.SpecCategory> UpdateSpecCategory(int id, KorJoo.Models.korjoo.SpecCategory speccategory)
        {
            OnSpecCategoryUpdated(speccategory);

            var itemToUpdate = Context.SpecCategories
                              .Where(i => i.Id == speccategory.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(speccategory);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSpecCategoryUpdated(speccategory);

            return speccategory;
        }

        partial void OnSpecCategoryDeleted(KorJoo.Models.korjoo.SpecCategory item);
        partial void OnAfterSpecCategoryDeleted(KorJoo.Models.korjoo.SpecCategory item);

        public async Task<KorJoo.Models.korjoo.SpecCategory> DeleteSpecCategory(int id)
        {
            var itemToDelete = Context.SpecCategories
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSpecCategoryDeleted(itemToDelete);


            Context.SpecCategories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSpecCategoryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSpecializationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/specializations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/specializations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSpecializationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/korjoo/specializations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/korjoo/specializations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSpecializationsRead(ref IQueryable<KorJoo.Models.korjoo.Specialization> items);

        public async Task<IQueryable<KorJoo.Models.korjoo.Specialization>> GetSpecializations(Query query = null)
        {
            var items = Context.Specializations.AsQueryable();

            items = items.Include(i => i.SpecCategory);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSpecializationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSpecializationGet(KorJoo.Models.korjoo.Specialization item);
        partial void OnGetSpecializationById(ref IQueryable<KorJoo.Models.korjoo.Specialization> items);


        public async Task<KorJoo.Models.korjoo.Specialization> GetSpecializationById(int id)
        {
            var items = Context.Specializations
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.SpecCategory);
 
            OnGetSpecializationById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSpecializationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSpecializationCreated(KorJoo.Models.korjoo.Specialization item);
        partial void OnAfterSpecializationCreated(KorJoo.Models.korjoo.Specialization item);

        public async Task<KorJoo.Models.korjoo.Specialization> CreateSpecialization(KorJoo.Models.korjoo.Specialization specialization)
        {
            OnSpecializationCreated(specialization);

            var existingItem = Context.Specializations
                              .Where(i => i.Id == specialization.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Specializations.Add(specialization);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(specialization).State = EntityState.Detached;
                throw;
            }

            OnAfterSpecializationCreated(specialization);

            return specialization;
        }

        public async Task<KorJoo.Models.korjoo.Specialization> CancelSpecializationChanges(KorJoo.Models.korjoo.Specialization item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSpecializationUpdated(KorJoo.Models.korjoo.Specialization item);
        partial void OnAfterSpecializationUpdated(KorJoo.Models.korjoo.Specialization item);

        public async Task<KorJoo.Models.korjoo.Specialization> UpdateSpecialization(int id, KorJoo.Models.korjoo.Specialization specialization)
        {
            OnSpecializationUpdated(specialization);

            var itemToUpdate = Context.Specializations
                              .Where(i => i.Id == specialization.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(specialization);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSpecializationUpdated(specialization);

            return specialization;
        }

        partial void OnSpecializationDeleted(KorJoo.Models.korjoo.Specialization item);
        partial void OnAfterSpecializationDeleted(KorJoo.Models.korjoo.Specialization item);

        public async Task<KorJoo.Models.korjoo.Specialization> DeleteSpecialization(int id)
        {
            var itemToDelete = Context.Specializations
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSpecializationDeleted(itemToDelete);


            Context.Specializations.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSpecializationDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}