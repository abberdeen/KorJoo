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
    public partial class EditJobSkill
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
            jobSkill = await korjooService.GetJobSkillById(Id);

            jobsForJobId = await korjooService.GetJobs();

            skillsForSkillId = await korjooService.GetSkills();
        }
        protected bool errorVisible;
        protected KorJoo.Models.korjoo.JobSkill jobSkill;

        protected IEnumerable<KorJoo.Models.korjoo.Job> jobsForJobId;

        protected IEnumerable<KorJoo.Models.korjoo.Skill> skillsForSkillId;

        protected async Task FormSubmit()
        {
            try
            {
                await korjooService.UpdateJobSkill(Id, jobSkill);
                DialogService.Close(jobSkill);
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

            jobSkill = await korjooService.GetJobSkillById(Id);
        }
    }
}