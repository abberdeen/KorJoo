using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using KorJoo.Models.korjoo;

namespace KorJoo.Pages
{
    public partial class AddApplicantPortofolio
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public korjooService korjooService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            applicantPortofolio = new KorJoo.Models.korjoo.ApplicantPortofolio();

            applicantsForApplicantId = await korjooService.GetApplicants();
        }
        protected bool errorVisible;
        protected KorJoo.Models.korjoo.ApplicantPortofolio applicantPortofolio;

        protected IEnumerable<KorJoo.Models.korjoo.Applicant> applicantsForApplicantId;

        protected async Task FormSubmit()
        {
            try
            {
                var applicantId = await korjooService.GetApplicantIdByUserId(Security.User.Id);
                applicantPortofolio.ApplicantId = applicantId;

                await korjooService.CreateApplicantPortofolio(applicantPortofolio);
                DialogService.Close(applicantPortofolio);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}