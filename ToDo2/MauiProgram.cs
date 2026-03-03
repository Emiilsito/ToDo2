using SkiaSharp.Views.Maui.Controls.Hosting;
using ToDo2.ViewModels;

namespace ToDo2;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
#if DEBUG
			.UseSkiaSharp()
			.UseDebugRainbows(new DebugRainbowsOptions { })
#endif
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
				fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
				fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton(CalendarStore.Default);
        builder.Services.AddSingleton<TodoViewModel>();
        builder.Services.AddSingleton<SamplePageViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Settings>();
        builder.Services.AddTransient<SamplePage>();

        return builder.Build();
	}
}
