using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace ToDo2.Models;

public class TodoItem : BindableObject
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    public int TareaCodigo { get; set; }
    public string? Name { get; set; }
    public string? Nota { get; set; }
    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            _done = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Progreso));
            OnPropertyChanged(nameof(FechaCompletado));
        }
    }
    public string? Tipo { get; set; }
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

    [JsonIgnore]
    public double Progreso
    {
        get
        {
            if (Subtareas == null || Subtareas.Count == 0)
            {
                return Done ? 1.0 : 0.0;
            }
            return (double)Subtareas.Count(s => s.Completada) / Subtareas.Count;
        }
    }

    public void NotificarCambio()
    {
        OnPropertyChanged(nameof(Progreso));
        OnPropertyChanged(nameof(Done));
        OnPropertyChanged(nameof(FechaCompletado));
    }
}

public class Subtarea : BindableObject
{
    public string? Nombre { get; set; }
    public bool Completada { get; set; }
}