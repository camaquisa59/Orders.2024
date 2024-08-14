using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Pages.Countries;
using Orders.FrontEnd.Repositories;
using Orders.FrontEnd.Shared;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Categories
{
    public partial class CategoryCreate
    {


        private FormWithName<Category>? categoryform;
        private Category category = new();
        [Inject] private IRepository Repository { get; set; } = null!;

        [Inject] private SweetAlertService AlertService { get; set; } = null!;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;


        private async Task CreateAsync()
        {
            var responseHttp = await Repository.PostAsync("/api/categories", category);
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
                ShowConfirmButton = true,
                Timer = 3000

            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado correctamente");


        }

        private void Return()
        {
            categoryform!.FormPostSuccessfully = true;
            NavigationManager.NavigateTo("/categories");

        }
    }
}
