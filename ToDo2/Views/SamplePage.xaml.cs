using ToDo2.ViewModels;

namespace ToDo2.Views;

public partial class SamplePage : ContentPage
{
    private readonly SamplePageViewModel _vm;

    public SamplePage(SamplePageViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_vm != null)
        {
            _vm.ActualizarGrafica();

            graficaDonut.Chart = _vm.TareasPorTipo;
        }
    }
}