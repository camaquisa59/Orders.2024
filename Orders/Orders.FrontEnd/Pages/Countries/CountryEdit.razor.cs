using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.FrontEnd.Shared;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Countries
{
    public partial class CountryEdit
    {
        private Country? country;
        private FormWithName<Country>? countryForm;

        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService alertService { get; set; } = null!;

        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        [EditorRequired, Parameter] public int Id { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await repository.GetAsync<Country>($"/api/countries/{Id}");
            if ( responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    navigationManager.NavigateTo("/countries");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsyn();
                    await alertService.FireAsync("Error", message);
                }


            }
            else
            {
                country = responseHttp.Response;
            }

        }

        private async Task EditAsync()
        {
            var responseHttp = await repository.PutAsync("/api/countries", country);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsyn();
                await alertService.FireAsync("Error", message);
                return;
            }

            Return();
            var toast = alertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000

            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro actualizado correctamente.");



        }

        private void Return()
        {
            countryForm!.FormPostSuccessfully = true;
            navigationManager.NavigateTo("/countries");

        }


    }
}
