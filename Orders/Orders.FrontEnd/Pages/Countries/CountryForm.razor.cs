using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Countries
{
    public partial class CountryForm
    {
        private EditContext _editContext = null!;

        [EditorRequired, Parameter] public Country country { get; set; } = null!;

        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }

        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

        [Inject] public SweetAlertService alertService { get; set; } = null!;

        public bool FormPostSuccessfully { get; set; }


        protected override void OnInitialized()
        {
            _editContext = new(country);
        }


        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            var formWasEdited = _editContext.IsModified();
            if (!formWasEdited || FormPostSuccessfully)
            {
                return;
            }

            var result = await alertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirme",
                Text = "¿Desea abandonar la página y perder los cambios?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm) {
                return;
            }

            context.PreventNavigation();

        }
    }
}
