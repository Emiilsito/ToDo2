using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using ToDo2.Models;
using ToDo2.Services;

namespace ToDo2.ViewModels
{
    public class TodoViewModel : BindableObject
    {
        private HubConnection _hubConnection;

        private readonly TodoService _todoService = new();
        public ObservableCollection<TodoItem> TodoItems { get; set; } = new();
        public ObservableCollection<string> TiposTodo { get; set; } = new();

        public TodoViewModel()
        {
            ConfigurarSignalR();
            CargarTiposDesdeStorage();
            _ = CargarTareasAsync();
            
        }

        private async void ConfigurarSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://unossifying-condensible-lakenya.ngrok-free.dev")
                .WithAutomaticReconnect()
                .Build();
            _hubConnection.On("ReceiveRefresh", async () =>
            {
                await CargarTareasAsync();
            });
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("Conexión SignalR establecida.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar a SignalR: {ex.Message}");
            }
        }

        public async Task CargarTareasAsync()
        {
            try
            {
                var tareas = await _todoService.GetTareasAsync();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TodoItems.Clear();
                    foreach (var item in tareas)
                    {
                        TodoItems.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void Agregar(string tarea, string nota, string tipo, bool agregarFechaLimite, DateTime? fechaFin = null)
        {
            if (!string.IsNullOrWhiteSpace(tarea))
            {
                var nuevo = new TodoItem
                {
                    CreadoEn = DateTime.Now,
                    Nota = nota,
                    Done = false,
                    Name = tarea,
                    Tipo = tipo,
                    FinalizadoEn = agregarFechaLimite ? fechaFin : null
                };

                await _todoService.AddTareaAsync(nuevo);
                await CargarTareasAsync();
            }
        }

        public async Task Eliminar(TodoItem item)
        {
            if (item != null && item.TareaCodigo > 0)
            {
                await _todoService.DeleteTareaAsync(item.TareaCodigo);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TodoItems.Remove(item);
                });
            }
        }

        public async void ActualizarEstadoTarea(TodoItem item)
        {
            if (item == null) return;

            if (item.Done)
            {
                if (item.FechaCompletado == null)
                    item.FechaCompletado = DateTime.Now;
            }
            else
            {
                item.FechaCompletado = null;
            }

            item.NotificarCambio();
            await _todoService.UpdateTareaAsync(item);
        }

        public void GuardarEnStorage()
        {
            string tiposTodo = System.Text.Json.JsonSerializer.Serialize(TiposTodo);
            Preferences.Default.Set("TiposTodo", tiposTodo);
        }

        public void CargarTiposDesdeStorage()
        {
            string tiposTodo = Preferences.Default.Get("TiposTodo", "");
            if (!string.IsNullOrEmpty(tiposTodo))
            {
                var tipos = System.Text.Json.JsonSerializer.Deserialize<List<string>>(tiposTodo);
                TiposTodo = new ObservableCollection<string>(tipos ?? new List<string>());
            }
        }

        public async Task ActualizarTareaAsync(TodoItem item)
        {
            if (item != null)
            {
                await _todoService.UpdateTareaAsync(item);
            }
        }
    }
}