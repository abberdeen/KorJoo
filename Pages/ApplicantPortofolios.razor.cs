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
    public partial class ApplicantPortofolios
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

        protected IEnumerable<KorJoo.Models.korjoo.ApplicantPortofolio> applicantPortofolios;

        protected RadzenDataGrid<KorJoo.Models.korjoo.ApplicantPortofolio> grid0;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            applicantPortofolios = await korjooService.GetApplicantPortofolios(new Query { Expand = "Applicant" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddApplicantPortofolio>("Add ApplicantPortofolio", null);
            await grid0.Reload();
        }

        protected async Task EditRow(KorJoo.Models.korjoo.ApplicantPortofolio args)
        {
            await DialogService.OpenAsync<EditApplicantPortofolio>("Edit ApplicantPortofolio", new Dictionary<string, object> { {"Id", args.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, KorJoo.Models.korjoo.ApplicantPortofolio applicantPortofolio)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await korjooService.DeleteApplicantPortofolio(applicantPortofolio.Id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete ApplicantPortofolio" 
                });
            }
        }
    }
}