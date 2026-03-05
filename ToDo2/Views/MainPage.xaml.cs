using ToDo2.Models;
using ToDo2.ViewModels;

namespace ToDo2.Views;

public partial class MainPage : ContentPage
{
    TodoViewModel _vm;
    bool usarFecha = false;

    public MainPage(TodoViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
        ListaTareasView.ItemsSource = _vm.TodoItems;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private void OnUsarFechaClicked(object sender, EventArgs e)
    {
        usarFecha = !usarFecha;
        FechaFin.IsVisible = usarFecha;
    }

    private void OnAgregarClicked(object sender, EventArgs e)
    {
        DateTime? fechaSeleccionada = usarFecha ? FechaFin.Date : null;
        string tipoTarea = TipoTarea.SelectedItem?.ToString() ?? "Tarea sin tipo";

        _vm.Agregar(Tarea.Text, TareaNota.Text, tipoTarea, usarFecha, fechaSeleccionada);
        Tarea.Text = string.Empty;
        TareaNota.Text = string.Empty;
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        var boton = (Button)sender;
        var tareaAEliminar = (TodoItem)boton.BindingContext;
        await _vm.Eliminar(tareaAEliminar);
    }

    private void OnTareaEstadoChanged(object sender, CheckedChangedEventArgs e)
    {
        var cb = (CheckBox)sender;
        var tarea = (TodoItem)cb.BindingContext;
        if (tarea != null)
        {
            _vm.ActualizarEstadoTarea(tarea);
        }
    }

    private async void OnAgregarSubtareaClicked(object sender, EventArgs e)
    {
        var boton = (Button)sender;
        var tareaPrincipal = (TodoItem)boton.CommandParameter;

        string? nombreSubtarea = await DisplayPromptAsync("Nueva Subtarea", "Escribe el nombre de la subtarea:", "Agregar", "Cancelar");

        if (!string.IsNullOrWhiteSpace(nombreSubtarea))
        {
            if (tareaPrincipal.Subtareas == null)
            {
                tareaPrincipal.Subtareas = new System.Collections.ObjectModel.ObservableCollection<Subtarea>();
            }

            tareaPrincipal.Subtareas.Add(new Subtarea
            {
                Nombre = nombreSubtarea,
                Completada = false
            });

            await _vm.ActualizarTareaAsync(tareaPrincipal);
        }
    }

    private async void OnSubtareaEstadoChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = (sender as CheckBox);
        Element parent = checkBox.Parent;

        while (parent != null && parent is not Border)
        {
            parent = parent.Parent;
        }

        if (parent is Border border && border.BindingContext is TodoItem tarea)
        {
            bool todasCompletadas = tarea.Subtareas.Count > 0 && tarea.Subtareas.All(s => s.Completada);

            if (todasCompletadas)
            {
                tarea.Done = true;
                tarea.FechaCompletado = DateTime.Now;
            }
            else
            {
                tarea.Done = false;
                tarea.FechaCompletado = null;
            }

            tarea.NotificarCambio();
            await _vm.ActualizarTareaAsync(tarea);
        }
    }

    private async void OnEliminarSubtareaClicked(object sender, EventArgs e)
    {
        var boton = (Button)sender;
        if (boton.CommandParameter is Subtarea subtarea)
        {
            Element parent = boton.Parent;
            while (parent != null && parent is not Border)
            {
                parent = parent.Parent;
            }

            if (parent is Border border && border.BindingContext is TodoItem tarea)
            {
                bool confirmar = await DisplayAlert("Eliminar", $"¿Borrar '{subtarea.Nombre}'?", "Sí", "No");

                if (confirmar)
                {
                    tarea.Subtareas.Remove(subtarea);
                    tarea.NotificarCambio();
                    await _vm.ActualizarTareaAsync(tarea);
                }
            }
        }
    }
}