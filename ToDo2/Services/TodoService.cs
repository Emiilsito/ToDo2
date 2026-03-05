using System.Net.Http.Json;
using ToDo2.Models;

namespace ToDo2.Services
{
    public class TodoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://unossifying-condensible-lakenya.ngrok-free.dev/todoitems";

        public TodoService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
        }

        public async Task<List<TodoItem>> GetTareasAsync()
        {
            try
            {
                var tareas = await _httpClient.GetFromJsonAsync<List<TodoItem>>("todoitems") ?? new();

                foreach (var tarea in tareas)
                {
                    tarea.CreadoEn = tarea.CreadoEn.ToLocalTime();
                    if (tarea.FechaCompletado.HasValue)
                        tarea.FechaCompletado = tarea.FechaCompletado.Value.ToLocalTime();
                    if (tarea.FinalizadoEn.HasValue)
                        tarea.FinalizadoEn = tarea.FinalizadoEn.Value.ToLocalTime();
                }

                return tareas;
            }
            catch
            {
                return new List<TodoItem>();
            }
        }

        public async Task AddTareaAsync(TodoItem item)
        {
            await _httpClient.PostAsJsonAsync("todoitems", item);
        }

        public async Task UpdateTareaAsync(TodoItem item)
        {
            if (item.TareaCodigo <= 0) return;

            await _httpClient.PutAsJsonAsync($"todoitems/{item.TareaCodigo}", item);
        }

        public async Task DeleteTareaAsync(int tareaCodigo)
        {
            if (tareaCodigo <= 0) return;

            await _httpClient.DeleteAsync($"todoitems/{tareaCodigo}");
        }
    }
}