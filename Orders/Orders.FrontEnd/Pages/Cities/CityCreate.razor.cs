using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.FrontEnd.Shared;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Cities
{
    public partial class CityCreate
    {


        private City city = new();
        private FormWithName<City>? cityform;

        [Parameter] public int StateId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;

        [Inject] private SweetAlertService AlertService { get; set; } = null!;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;


        private async Task CreateAsync()
        {
            city.StateId = StateId;
            var responseHttp = await Repository.PostAsync("/api/cities/", city);
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
            cityform!.FormPostSuccessfully = true;
            NavigationManager.NavigateTo($"/states/details/{StateId}");

        }
    }
}
