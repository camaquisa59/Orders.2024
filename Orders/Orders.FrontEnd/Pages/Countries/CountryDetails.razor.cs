using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Countries
{
    public partial class CountryDetails
    {
        private Country? country;
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private SweetAlertService SweetAlertService { get; set; }
        [Inject] private IRepository Repository { get; set; }
        [Parameter] public int CountryId { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHttp = await Repository.GetAsync<Country>($"/api/countries/{CountryId}");
            if (responseHttp.Error) { 
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/countries");
                    return;
                }
                var message = await responseHttp.GetErrorMessageAsyn();
                await SweetAlertService.FireAsync("Error: ", message, SweetAlertIcon.Error);
                return;
            
            }
            country = responseHttp.Response;

        }

        private async Task DeleteAsync(State state)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Desea eliminar el Dpto/Estado/Provincia? {state.Name}",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText = "NO",
                ConfirmButtonText = "SI",

            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync<State>($"/api/states/{state.Id}");
            if (responseHttp.Error) {
                if (responseHttp.HttpResponseMessage.StatusCode != System.Net.HttpStatusCode.NotFound) {
                    var message = await responseHttp.GetErrorMessageAsyn();
                    await SweetAlertService.FireAsync( "Error: ", message, SweetAlertIcon.Error);
                    return;
                }
            }

            await LoadAsync();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000

            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro eliminado correctamente.");

            

        }
    }
}
