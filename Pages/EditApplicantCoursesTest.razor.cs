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
    public partial class EditApplicantCoursesTest
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
            applicantCoursesTest = await korjooService.GetApplicantCoursesTestById(Id);

            applicantsForApplicantId = await korjooService.GetApplicants();
        }
        protected bool errorVisible;
        protected KorJoo.Models.korjoo.ApplicantCoursesTest applicantCoursesTest;

        protected IEnumerable<KorJoo.Models.korjoo.Applicant> applicantsForApplicantId;

        protected async Task FormSubmit()
        {
            try
            {
                await korjooService.UpdateApplicantCoursesTest(Id, applicantCoursesTest);
                DialogService.Close(applicantCoursesTest);
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
           korjooService.Reset();
            hasChanges = false;
            canEdit = true;

            applicantCoursesTest = await korjooService.GetApplicantCoursesTestById(Id);
        }
    }
}