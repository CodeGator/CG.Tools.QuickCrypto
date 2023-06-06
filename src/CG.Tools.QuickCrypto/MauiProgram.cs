
using CommunityToolkit.Maui;
using MudBlazor.Services;

namespace CG.Tools.QuickCrypto;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddMudServices();
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        builder.Services.AddScoped<MainPage>();
        builder.Services.AddScoped<Cryptography>();

        builder.Services.AddScoped<IndexViewModel>();
        builder.Services.AddScoped<FilePageViewModel>();
        builder.Services.AddScoped<TextPageViewModel>();
        builder.Services.AddScoped<SettingsPageViewModel>();

        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
        builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);

        return builder.Build();
    }
}