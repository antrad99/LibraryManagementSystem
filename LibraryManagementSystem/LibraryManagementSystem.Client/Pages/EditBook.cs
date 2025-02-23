using LibraryManagementSystem.Shared.Constants;
using LibraryManagementSystem.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;


namespace LibraryManagementSystem.Client.Pages
{
    public partial class EditBook
    {
        [Parameter]
        public int id { get; set; }
        public bool Loading { get; set; }
        private bool DisplayError { get; set; }
        private bool DisplayResults { get; set; }
        private bool DisplayMessage { get; set; }
        private string Message { get; set; } = string.Empty;
        private BookDto? bookDto { get; set; } = new BookDto();
        [Inject]
        HttpClient httpClient { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }


        protected async override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Loading = true;
                DisplayResults = false;
                DisplayMessage = false;
                Message = string.Empty;
                DisplayError = false;
                StateHasChanged();

                if (id != -1)
                {
                    var response = await httpClient.GetAsync($"{ConstBaseUrls.BookApi}/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.NoContent)
                        {
                            Message = "No book found";
                            DisplayMessage = true;
                        }
                        else
                        {
                            bookDto = await response.Content.ReadFromJsonAsync<BookDto>();
                            DisplayResults = true;
                        }
                    }
                    else
                    { 
                        //There has been an internal error, do not display the stack info to the user, the actual error in save in the log file
                        DisplayError = true;
                    }

                }
                else
                {
                    //DO NOTHING --> ADD NEW BOOK
                    bookDto.PublicationYear = DateTime.Today.Year;
                    DisplayResults = true;
                }

                Loading = false;
                StateHasChanged();

            }
        }

        private async void HandleValidSubmit()
        {

            Loading = true;
            DisplayError = false;
            DisplayResults = false;
            DisplayMessage = false;
            Message = string.Empty;
            StateHasChanged();

            //check Pub Year in the future
            //Also it could be useful to check if it is too old.
            if (bookDto.PublicationYear > DateTime.Today.Year)
            {
                Message = "Publication year cannot be in the future; ";
                DisplayMessage = true;
            }

            if (id == -1)
            { 
                //check ISBN is unique when POST
                var responseISBN = await httpClient.GetAsync($"{ConstBaseUrls.BookApi}/isbn/{bookDto.ISBN}");

                if (responseISBN.IsSuccessStatusCode)
                {
                    var exist = await responseISBN.Content.ReadFromJsonAsync<bool>();
                    if (exist)
                    {
                        Message += "ISBN must be unique;";
                        DisplayMessage = true;
                    }
                }
            }

            if (DisplayMessage)
            {
                Loading = false;
                DisplayResults = true;
                StateHasChanged();
                return;
            }

            if (id == -1)
            {
                var response = await httpClient.PostAsJsonAsync($"{ConstBaseUrls.BookApi}", bookDto);

                if (response.IsSuccessStatusCode)
                {
                    bookDto = await response.Content.ReadFromJsonAsync<BookDto>();
                }
                else
                {
                    //There has been an internal error, do not display the stack info to the user, the actual error in save in the log file
                    DisplayError = true;
                    Loading = false;
                    StateHasChanged();
                    return;
                }

            }
            else
            {
                var response = await httpClient.PutAsJsonAsync($"{ConstBaseUrls.BookApi}/{id}", bookDto);

                if (response.IsSuccessStatusCode)
                {
                    bookDto = await response.Content.ReadFromJsonAsync<BookDto>();
                }
                else
                {
                    //There has been an internal error, do not display the stack info to the user, the actual error in save in the log file
                    DisplayError = true;
                    Loading = false;
                    StateHasChanged();
                    return;
                }

            }

            NavigationManager?.NavigateTo("/");

        }
    }
}
