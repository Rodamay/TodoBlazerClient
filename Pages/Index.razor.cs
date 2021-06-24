using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TodoBlazorClient.Models;

namespace TodoBlazorClient.Pages
{
    public partial class Index
    {
        private List<TodoItemDTO> TodoItems = new List<TodoItemDTO>();

        private TodoItemDTO TodoItem = new TodoItemDTO();

        protected override async Task OnInitializedAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44354/");

            //Load all Todos
            var streamTask = client.GetStreamAsync("api/Todoitems");
            TodoItems = await JsonSerializer.DeserializeAsync<List<TodoItemDTO>>(await streamTask);
        }

        private async Task CreateTodo(string name)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44354/");

            var todo = new TodoItemDTO { Name = name };

            var body = JsonSerializer.Serialize(todo);
            var content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");

            await client.PostAsync("api/Todoitems", content);

            //Reload all todos

            var streamTask = client.GetStreamAsync("api/Todoitems");
            TodoItems = await JsonSerializer.DeserializeAsync<List<TodoItemDTO>>(await streamTask);
        }

        private async Task CompleteTodo(long id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44354/");

            var url = string.Format(CultureInfo.InvariantCulture, "api/Todoitems/{0}", id);

            await client.DeleteAsync(url);

            //Reload all todos
            var streamTask = client.GetStreamAsync("api/Todoitems");
            TodoItems = await JsonSerializer.DeserializeAsync<List<TodoItemDTO>>(await streamTask);
        }
    }
}
