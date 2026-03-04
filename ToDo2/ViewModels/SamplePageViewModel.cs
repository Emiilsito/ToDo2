using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace ToDo2.ViewModels;

public class SamplePageViewModel : BindableObject
{
    private readonly TodoViewModel _todoViewModel;
    private Chart _tareasPorTipo;

    public Chart TareasPorTipo { get => _tareasPorTipo; set { _tareasPorTipo = value; OnPropertyChanged(); } }
    public int CompletadasEsteMes { get; set; }
    public int CompletadasMesAnterior { get; set; }
    public string TipoMasCompletado { get; set; } = "---";
    public ObservableCollection<Models.TodoItem> MisTareas => _todoViewModel.TodoItems;

    public SamplePageViewModel(TodoViewModel todoViewModel)
    {
        _todoViewModel = todoViewModel;
    }

    public void ActualizarGrafica()
    {
        var todas = _todoViewModel.TodoItems.ToList();
        var hoy = DateTime.Now;
        var mesAnterior = hoy.AddMonths(-1);

        CompletadasEsteMes = todas.Count(t =>
            t.Done &&
            t.FechaCompletado?.Month == hoy.Month &&
            t.FechaCompletado?.Year == hoy.Year);

        CompletadasMesAnterior = todas.Count(t =>
            t.Done &&
            t.FechaCompletado?.Month == mesAnterior.Month &&
            t.FechaCompletado?.Year == mesAnterior.Year);

        var completadas = todas.Where(t => t.Done).ToList();
        if (completadas.Any())
        {
            TipoMasCompletado = completadas.GroupBy(t => t.Tipo)
                .OrderByDescending(g => g.Count()).First().Key;
        }

        string[] paleta = { "#512BD4", "#30D5C8", "#FF5733", "#FFC300", "#DAF7A6", "#9B59B6" };
        int i = 0;

        var entries = todas.GroupBy(t => t.Tipo ?? "General")
            .Select(g => new ChartEntry(g.Count())
            {
                Label = g.Key,
                ValueLabel = g.Count().ToString(),
                Color = SKColor.Parse(paleta[i++ % paleta.Length]),
                TextColor = SKColors.Black,
                ValueLabelColor = SKColors.DarkSlateGray
            }).ToArray();

        TareasPorTipo = new DonutChart
        {
            Entries = entries,
            LabelTextSize = 30,
            HoleRadius = 0.5f,
            BackgroundColor = SKColors.Transparent
        };

        OnPropertyChanged(nameof(CompletadasEsteMes));
        OnPropertyChanged(nameof(CompletadasMesAnterior));
        OnPropertyChanged(nameof(TipoMasCompletado));
    }
}