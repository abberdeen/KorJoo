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
    public partial class AddApplicantLanguage
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
            applicantLanguage = new KorJoo.Models.korjoo.ApplicantLanguage();
        }
        protected bool errorVisible;
        protected KorJoo.Models.korjoo.ApplicantLanguage applicantLanguage;

        protected async Task FormSubmit()
        {
            try
            {
                var applicantId = await korjooService.GetApplicantIdByUserId(Security.User.Id);
                applicantLanguage.ApplicantId = applicantId;

                await korjooService.CreateApplicantLanguage(applicantLanguage);
                DialogService.Close(applicantLanguage);
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