using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDo2.Models;

public class TodoItem : BindableObject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Nota { get; set; }
    public bool Done { get; set; }
    public string Tipo { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? FinalizadoEn { get; set; }

    private DateTime? _FechaCompletado;

    public DateTime? FechaCompletado
    {
        get => _FechaCompletado;
        set
        {
            _FechaCompletado = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Subtarea> Subtareas { get; set; } = new();

    public double Progreso => Subtareas.Count == 0 ?
        (Done ? 1.0 : 0.0) :
        (double)Subtareas.Count(s => s.Completada) / Subtareas.Count;

    public void NotificarCambio()
    {
        OnPropertyChanged(nameof(Progreso));
        OnPropertyChanged(nameof(Done));
        OnPropertyChanged(nameof(FechaCompletado));
    }
}

public class Subtarea
{
    public string Nombre { get; set; }
    public bool Completada { get; set; }
}