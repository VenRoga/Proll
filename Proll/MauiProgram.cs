using Microsoft.Extensions.Logging;
using Proll.Apis;
using Refit;

namespace Proll
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            ConfigureRefit(builder.Services);
            return builder.Build();
        }

        private static void ConfigureRefit(IServiceCollection services)
        {
            const string baseApiUrl = "https://mjx06psd-7263.eun1.devtunnels.ms";//можно через локал хост

            services.AddRefitClient<IProductApi>()
                .ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IAuthApi>()
                .ConfigureHttpClient(SetHttpClient);

            //так как нужна авторизация, нужно передать токен
            services.AddRefitClient<IOrderApi>(GetRefitSettings)
                .ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IUserApi>(GetRefitSettings)
                .ConfigureHttpClient(SetHttpClient);

            static void SetHttpClient(HttpClient httpClient)
                => httpClient.BaseAddress = new Uri(baseApiUrl);


            static RefitSettings GetRefitSettings(IServiceProvider sp)
            {
                var settings = new RefitSettings();

                settings.AuthorizationHeaderValueGetter = (_, __) => ValueTask.FromResult("alskdhfaisghfoiyuAWERGFidajsbvflagtfhrwqw7tryia890y389-4aropiuhgfioawhgf7q3g78934thqk234nff2094h924rh2k4");
                return settings; 
            }
        }
    }
}
