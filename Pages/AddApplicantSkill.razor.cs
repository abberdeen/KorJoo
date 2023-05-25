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
    public partial class AddApplicantSkill
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
            applicantSkill = new KorJoo.Models.korjoo.ApplicantSkill();

            applicantsForApplicantId = await korjooService.GetApplicants();

            skillsForSkillId = await korjooService.GetSkills();
        }
        protected bool errorVisible;
        protected KorJoo.Models.korjoo.ApplicantSkill applicantSkill;

        protected IEnumerable<KorJoo.Models.korjoo.Applicant> applicantsForApplicantId;

        protected IEnumerable<KorJoo.Models.korjoo.Skill> skillsForSkillId;

        protected async Task FormSubmit()
        {
            try
            {
                await korjooService.CreateApplicantSkill(applicantSkill);
                DialogService.Close(applicantSkill);
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
    }
}