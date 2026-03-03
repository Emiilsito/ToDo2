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

        ListaTareasView.ItemsSource = vm.TodoItems;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ListaTareasView.ItemsSource = _vm.TodoItems;
    }

    private void OnUsarFechaClicked(object sender, EventArgs e)
	{
		usarFecha = !usarFecha;

		if (usarFecha)
		{
			FechaFin.IsVisible = true;
		} else
		{
			FechaFin.IsVisible = false;
		}
	}

    private void OnAgregarClicked(object sender, EventArgs e)
	{
		DateTime? fechaSeleccionada = FechaFin.Date != null ? FechaFin.Date : null; 

		string tipoTarea = TipoTarea.SelectedItem?.ToString() ?? "Selecciona un tipo";

        _vm.Agregar(Tarea.Text, TareaNota.Text, tipoTarea, usarFecha, fechaSeleccionada);
		Tarea.Text = string.Empty;
		TareaNota.Text = string.Empty;
	}

	private void OnEliminarClicked(object sender, EventArgs e)
	{
		var boton = (Button)sender;

		var tareaAEliminar = (TodoItem)boton.BindingContext;

		_vm.Eliminar(tareaAEliminar);
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

}
