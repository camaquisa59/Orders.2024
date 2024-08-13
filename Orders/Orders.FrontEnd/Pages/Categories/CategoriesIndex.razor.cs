using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Categories
{
    public partial class CategoriesIndex
    {
        [Inject] private IRepository Repository { get; set; } = null!;

        [Inject] private SweetAlertService AlertService { get; set; } = null!;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        public List<Category>? ListaCategorias { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Category>>("api/categories");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsyn();
                await AlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            ListaCategorias = responseHttp.Response;
        }

        private async Task DeleteAsync(Category c)
        {
            var result = await AlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmar",
                Text = $"¿Desea eliminar la categoria: {c.Name}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            //eliminamos
            var responseHttp = await Repository.DeleteAsync<Category>($"/api/categories/{c.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/categories");
                }
                else
                {
                    var messageError = await responseHttp.GetErrorMessageAsyn();
                    await AlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                }
                return;
            }
            else
            {
                await LoadAsync();
                var toast = AlertService.Mixin(new SweetAlertOptions
                {
                    Toast = true,
                    Position = SweetAlertPosition.BottomEnd,
                    ShowConfirmButton = true,
                    Timer = 3000
                });
                await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro eliminado correctamente");
            }
        }
    }
}