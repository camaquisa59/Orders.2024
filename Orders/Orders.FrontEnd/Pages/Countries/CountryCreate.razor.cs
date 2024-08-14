using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.FrontEnd.Shared;
using Orders.Shared.Entities;
using Orders.Shared.Interfaces;

namespace Orders.FrontEnd.Pages.Countries
{
    public partial class CountryCreate

    {  
        private Country country = new();
        private FormWithName<Country>? countryform;

      
        [Inject] private IRepository Repository { get; set; } = null!;

        [Inject] private SweetAlertService AlertService { get; set; } = null!;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;


        private async Task CreateAsync()
        {
            var responseHttp = await Repository.PostAsync("/api/countries", country);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsyn();
                await AlertService.FireAsync("Error: ", message);
                return;
            }

            Return();
            var toast = AlertService.Mixin(new SweetAlertOptions
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
            NavigationManager.NavigateTo("/countries");

        }
    }
}
