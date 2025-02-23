using LibraryManagementSystem.Shared.Constants;
using LibraryManagementSystem.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;


namespace LibraryManagementSystem.Client.Pages
{
    public partial class BookList
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        HttpClient httpClient { get; set; }
        private bool Loading { get; set; }
        private bool DisplayError { get; set; }
        private bool DisplayResults { get; set; }
        private bool NoResults { get; set; }
        private List<BookDto> booksDto { get; set; } = new List<BookDto>();
        private bool selectedOrderByTitle { get; set;  } = true;
        private bool selectedOrderByPubYear { get; set; } = false;

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                BindData("Title");
            }
        }

        private async void BindData(string orderBy)
        {
            Loading = true;
            DisplayResults = false;
            NoResults = false;
            StateHasChanged();

            var response = await httpClient.GetAsync($"{ConstBaseUrls.BookApi}/orderBy/{orderBy}");

            if (response.IsSuccessStatusCode)
            {

                booksDto = await response.Content.ReadFromJsonAsync<List<BookDto>>();

                if (booksDto.Count == 0)
                    NoResults = true;
                else
                    DisplayResults = true;
            }
            else
            {
                //There has been an internal error, do not display the stack info to the user, the actual error in save in the log file
                DisplayError = true;
            }

            Loading = false;
            StateHasChanged();
        }

        private void orderByTitle()
        {
            selectedOrderByTitle= true;
            selectedOrderByPubYear = false;
            BindData("Title");
        }

        private void orderByPubYear()
        {
            selectedOrderByTitle = false;
            selectedOrderByPubYear = true;
            BindData("PublicationYear");  
        }

        private void editBook(int id)
        {
            NavigationManager.NavigateTo("/edit-book/" + id);
        }

        private void addBook()
        {
            NavigationManager.NavigateTo("/edit-book/-1");
        }
    }
}
