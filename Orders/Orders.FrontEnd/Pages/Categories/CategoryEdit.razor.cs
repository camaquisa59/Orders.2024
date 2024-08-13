using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Pages.Countries;
using Orders.FrontEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Categories
{
    public partial class CategoryEdit
    {


        private Category? category;
        private CategoryForm? categoryForm;

        [Inject] private IRepository Repository { get; set; } = null!;

        [Inject] private SweetAlertService AlertService { get; set; } = null!;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [EditorRequired, Parameter] public int Id { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Category>($"/api/categories/{Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/categories");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsyn();
                    await AlertService.FireAsync("Error", message);
                }


            }
            else
            {
                category = responseHttp.Response;
            }

        }

        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync("/api/categories", category);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsyn();
                await AlertService.FireAsync("Error", message);
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
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro actualizado correctamente.");



        }

        private void Return()
        {
            categoryForm!.FormPostSuccessfully = true;
            NavigationManager.NavigateTo("/categories");

        }

    }
}
