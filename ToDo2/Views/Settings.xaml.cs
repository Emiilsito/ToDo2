using ToDo2.ViewModels;

namespace ToDo2.Views;

public partial class Settings : ContentPage
{
	TodoViewModel _vm;
    public Settings(TodoViewModel vm)
	{
		InitializeComponent();

		_vm = vm;
		BindingContext = _vm;
    }

	public void OnAgregarTipoClicked(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(NombreTarea.Text))
		{
			_vm.TiposTodo.Add(NombreTarea.Text);
			_vm.GuardarEnStorage();
            NombreTarea.Text = string.Empty;
        }
    }

	public void OnEliminarTipoClicked(object sender, EventArgs e)
	{
		var boton = (Button)sender;
		var tipoAEliminar = (string)boton.BindingContext;
		_vm.TiposTodo.Remove(tipoAEliminar);
		_vm.GuardarEnStorage();
    }
}