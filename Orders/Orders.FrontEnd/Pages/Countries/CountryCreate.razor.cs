using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Countries
{
    public partial class CountryCreate
    {
        private CountryForm? countryform;
        private Country country = new();
        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService alertService { get; set; } = null!;

        [Inject] private NavigationManager navigationManager { get; set; } = null!;


        private async Task CreateAsync()
        {
            var responseHttp = await repository.PostAsync("/api/countries", country);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsyn();
                await alertService.FireAsync("Error: ", message);
                return;
            }

            Return();
            var toast = alertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton=true,
                Timer=3000

            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado correctamente");


        }

        private void Return()
        {
            countryform!.FormPostSuccessfully = true;
            navigationManager.NavigateTo("/countries");

        }
    }
}
