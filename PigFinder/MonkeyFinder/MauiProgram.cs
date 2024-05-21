using Microsoft.Extensions.Logging;
using MonkeyFinder.View;
using MonkeyFinder.Services;
using CommunityToolkit.Maui;

namespace MonkeyFinder;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Roboto-Medium.ttf", "OpenSansRegular");
				fonts.AddFont("Roboto-Light.ttf", "robotolight");
			}).UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MonkeyService>();
		builder.Services.AddSingleton<MonkeysViewModel>();
		builder.Services.AddTransient<MonkeyDetailsViewModel>();
		builder.Services.AddTransient<DetailsPage>();
		builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
		builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
		builder.Services.AddSingleton<IMap>(Map.Default);

		return builder.Build();
	}
}
