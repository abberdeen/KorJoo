using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using KorJoo.Data.Enums;

namespace KorJoo.Pages
{
    public partial class RegisterApplicationUser
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

        protected KorJoo.Models.ApplicationUser user;
        protected bool isBusy;
        protected bool errorVisible;
        protected string error;

        UserRole userRole = UserRole.Applicant;

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            user = new KorJoo.Models.ApplicationUser();
        }

        protected async Task FormSubmit()
        {
            try
            {
                isBusy = true;

                await Security.Register(user.Email, user.Password, userRole);

                DialogService.Close(true);
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }

            isBusy = false;
        }

        protected async Task CancelClick()
        {
            DialogService.Close(false);
        }
    }
}