using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.FrontEnd.Shared;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.States
{
    public partial class StateCreate
    {

        private State state = new();
        private FormWithName<State>? stateform;

        [Parameter] public int CountryId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;

        [Inject] private SweetAlertService AlertService { get; set; } = null!;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;


        private async Task CreateAsync()
        {
            state.CountryId = CountryId;
            var responseHttp = await Repository.PostAsync("/api/states", state);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsyn();
                await AlertService.FireAsync("Error: ", message, SweetAlertIcon.Error);
                return;
            }

            Return();
            var toast = AlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000

            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado correctamente");


        }

        private void Return()
        {
            stateform!.FormPostSuccessfully = true;
            NavigationManager.NavigateTo($"/countries/details/{CountryId}");

        }
    }
}
