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
    public partial class Specializations
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

        protected IEnumerable<KorJoo.Models.korjoo.Specialization> specializations;

        protected RadzenDataGrid<KorJoo.Models.korjoo.Specialization> grid0;
        protected override async Task OnInitializedAsync()
        {
            specializations = await korjooService.GetSpecializations(new Query { Expand = "SpecCategory" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddSpecialization>("Add Specialization", null);
            await grid0.Reload();
        }

        protected async Task EditRow(KorJoo.Models.korjoo.Specialization args)
        {
            await DialogService.OpenAsync<EditSpecialization>("Edit Specialization", new Dictionary<string, object> { {"Id", args.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, KorJoo.Models.korjoo.Specialization specialization)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await korjooService.DeleteSpecialization(specialization.Id);

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
                    Detail = $"Unable to delete Specialization" 
                });
            }
        }
    }
}