using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace KorJoo.Pages
{
    public partial class EditApplicantWorkHistory
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

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            applicantWorkHistory = await korjooService.GetApplicantWorkHistoryById(Id);

            applicantsForApplicantId = await korjooService.GetApplicants();
        }
        protected bool errorVisible;
        protected KorJoo.Models.korjoo.ApplicantWorkHistory applicantWorkHistory;

        protected IEnumerable<KorJoo.Models.korjoo.Applicant> applicantsForApplicantId;

        protected async Task FormSubmit()
        {
            try
            {
                await korjooService.UpdateApplicantWorkHistory(Id, applicantWorkHistory);
                DialogService.Close(applicantWorkHistory);
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
           korjooService.Reset();
            hasChanges = false;
            canEdit = true;

            applicantWorkHistory = await korjooService.GetApplicantWorkHistoryById(Id);
        }
    }
}