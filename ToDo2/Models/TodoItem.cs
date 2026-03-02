using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo2.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Nota { get; set; }

        public bool Done { get; set; }

        public DateTime CreadoEn {  get; set; }
        public DateTime FinalizadoEn {  get; set; }
       
    }
}
