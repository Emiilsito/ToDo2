using ToDo2.Models;
using ToDo2.ViewModels;

namespace ToDo2.Views;

public partial class MainPage : ContentPage
{
	TodoViewModel vm = new TodoViewModel();

	public MainPage()
	{
		InitializeComponent();

		ListaTareasView.ItemsSource = vm.TodoItems;
	}

	private void OnAgregarClicked(object sender, EventArgs e)
	{
		DateTime fechaSeleccionada = FechaFin.Date;

		vm.Agregar(Tarea.Text, TareaNota.Text, fechaSeleccionada);
		Tarea.Text = string.Empty;
		TareaNota.Text = string.Empty;
	}

	private void OnEliminarClicked(object sender, EventArgs e)
	{
		var boton = (Button)sender;

		var tareaAEliminar = (TodoItem)boton.BindingContext;

		vm.Eliminar(tareaAEliminar);
	}

}
