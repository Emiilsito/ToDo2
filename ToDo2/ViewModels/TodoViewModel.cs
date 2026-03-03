using System.Collections.ObjectModel;
using System.Text.Json;
using ToDo2.Models;

namespace ToDo2.ViewModels
{
    public class TodoViewModel : BindableObject
    {
        public ObservableCollection<TodoItem> TodoItems { get; set; } = new();
        public ObservableCollection<string> TiposTodo { get; set; } = new();

        public TodoViewModel()
        {
            CargarDesdeStorage();
        }

        public void Agregar(string tarea, string nota, string tipo, bool agregarFechaLimite, DateTime? fechaFin = null)
        {
            if (!string.IsNullOrWhiteSpace(tarea))
            {
                TodoItems.Add(new TodoItem
                {
                    Id = TodoItems.Count + 1,
                    CreadoEn = DateTime.Now,
                    Nota = nota,
                    Done = false,
                    Name = tarea,
                    Tipo = tipo,
                    FinalizadoEn = agregarFechaLimite ? fechaFin : null
                });
                GuardarEnStorage();
            }
        }

        public void Eliminar(TodoItem item)
        {
            if (item != null && TodoItems.Contains(item))
            {
                TodoItems.Remove(item);
                GuardarEnStorage();
            }
        }

        public void ActualizarEstadoTarea(TodoItem item)
        {
            if (item == null) return;

            if (item.Done)
            {
                item.FechaCompletado = DateTime.Now;
            }
            else
            {
                item.FechaCompletado = null;
            }

            item.NotificarCambio();
            GuardarEnStorage();
        }

        public void GuardarEnStorage()
        {
            string todoItems = JsonSerializer.Serialize(TodoItems);
            Preferences.Default.Set("TodoItems", todoItems);

            string tiposTodo = JsonSerializer.Serialize(TiposTodo);
            Preferences.Default.Set("TiposTodo", tiposTodo);
        }

        public void CargarDesdeStorage()
        {
            string todoItems = Preferences.Default.Get("TodoItems", "");
            string tiposTodo = Preferences.Default.Get("TiposTodo", "");

            if (!string.IsNullOrEmpty(todoItems))
            {
                var items = JsonSerializer.Deserialize<List<TodoItem>>(todoItems);
                TodoItems = new ObservableCollection<TodoItem>(items ?? new List<TodoItem>());
            }

            if (!string.IsNullOrEmpty(tiposTodo))
            {
                var tipos = JsonSerializer.Deserialize<List<string>>(tiposTodo);
                TiposTodo = new ObservableCollection<string>(tipos ?? new List<string>());
            }
        }
    }
}