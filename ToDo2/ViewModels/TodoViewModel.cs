using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo2.Models;

namespace ToDo2.ViewModels
{
    public class TodoViewModel
    {
        public ObservableCollection<TodoItem> TodoItems { get; set; } = new();

        public void Agregar(string tarea, string nota, DateTime fechaFin)
        {
            if (!string.IsNullOrWhiteSpace(tarea))
            {
                TodoItems.Add(new TodoItem { 
                    Id = TodoItems.Count + 1,
                    CreadoEn = DateTime.Now,
                    Nota = nota,
                    Done = false,
                    Name = tarea,
                    FinalizadoEn = fechaFin.Date
                });
            }
        }

        public void Eliminar(TodoItem item)
        {
            if (item != null && TodoItems.Contains(item))
            {
                TodoItems.Remove(item);
            }
        }
    }
}
