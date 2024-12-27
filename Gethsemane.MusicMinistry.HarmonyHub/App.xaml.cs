using Gethsemane.MusicMinistry.HarmonyHub.Configuration;
using Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Authentication;
using Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Repositories;
using Gethsemane.MusicMinistry.HarmonyHub.Infrastructure.Repositories.Inventory;
using Uno.Resizetizer;

namespace Gethsemane.MusicMinistry.HarmonyHub;

public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    protected Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            // Add navigation support for toolkit controls such as TabBar and NavigationView
            .UseToolkitNavigation()
#if MAUI_EMBEDDING
            .UseMauiEmbedding<MauiControls.App>(
                maui => maui
                    .UseMauiControls())
#endif
            .Configure(
                host => host
#if DEBUG
                    // Switch to Development environment when running in DEBUG
                    .UseEnvironment(Environments.Development)
#endif
                    .UseLogging(
                        configure: (context, logBuilder) =>
                        {
                            // Configure log levels for different categories of logging
                            logBuilder
                                .SetMinimumLevel(
                                    context.HostingEnvironment.IsDevelopment()
                                        ? LogLevel.Information
                                        : LogLevel.Warning)

                                // Default filters for core Uno Platform namespaces
                                .CoreLogLevel(LogLevel.Warning);

                            // Uno Platform namespace filter groups
                            // Uncomment individual methods to see more detailed logging
                            //// Generic Xaml events
                            //logBuilder.XamlLogLevel(LogLevel.Debug);
                            //// Layout specific messages
                            //logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
                            //// Storage messages
                            //logBuilder.StorageLogLevel(LogLevel.Debug);
                            //// Binding related messages
                            //logBuilder.XamlBindingLogLevel(LogLevel.Debug);
                            //// Binder memory references tracking
                            //logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
                            //// DevServer and HotReload related
                            //logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
                            //// Debug JS interop
                            //logBuilder.WebAssemblyLogLevel(LogLevel.Debug);
                        },
                        enableUnoLogging: true)
                    .UseSerilog(
                        consoleLoggingEnabled: true,
                        fileLoggingEnabled: true)
                    .UseConfiguration(
                        configure: configBuilder =>
                            configBuilder
                                .EmbeddedSource<App>()
                                .Section<AppConfig>()
                                .Section<InventoryConfig>())
                    // Register Json serializers (ISerializer and ISerializer)
                    .UseSerialization(
                        (context, services) => services
                            .AddContentSerializer(context)
                            .AddJsonTypeInfo(
                                WeatherForecastContext.Default
                                    .IImmutableListWeatherForecast))
                    .UseHttp(
                        (context, services) => services
                            // Register HttpClient
#if DEBUG
                            // DelegatingHandler will be automatically injected into Refit Client
                            .AddTransient<DelegatingHandler, DebugHttpHandler>()
#endif
                            .AddSingleton<IWeatherCache, WeatherCache>()
                            .AddRefitClient<IApiClient>(context)
                            .AddRefitClient<ISecretsClient>(context))
                    .UseAuthentication(
                        builder =>
                            AddCustomAuthenticationBuilder(builder))
                    .ConfigureServices(
                        (context, services) =>
                        {
                            // TODO: Register your services
                            //services.AddSingleton<IMyService, MyService>();
                            services.AddSingleton<IAuthZeroClient, AuthZeroClient>();
                            services
                                .AddSingleton<IMongoClientProvider,
                                    MongoClientProviderProvider>();
                            services.AddSingleton<IInventoryRepository, InventoryRepository>();
                        })
                    .UseNavigation(
                        ReactiveViewModelMappings.ViewModelMappings,
                        RegisterRoutes)
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.UseStudio();
#endif
        MainWindow.SetWindowIcon();

        Host = await builder.NavigateAsync<Shell>();
    }

    private static IAuthenticationBuilder
        AddCustomAuthenticationBuilder(
            IAuthenticationBuilder authenticationBuilder) =>
        authenticationBuilder.AddCustom(
            builder =>
            {
                builder.Login(
                    async (provider, dispatcher, credentials, ct) =>
                    {
                        var client =
                            provider.GetRequiredService<IAuthZeroClient>();
                        var authenticationResult =
                            await client.LoginAsync(cancellationToken: ct);

                        var token = authenticationResult.AccessToken;
                        var refreshToken = authenticationResult.RefreshToken;
                        var idToken = authenticationResult.IdentityToken;

                        if (token is not null)
                        {
                            var creds = new Dictionary<string, string>
                            {
                                { TokenCacheExtensions.AccessTokenKey, token }
                            };
                            if (refreshToken is not null)
                            {
                                creds[TokenCacheExtensions.RefreshTokenKey] =
                                    refreshToken;
                            }

                            if (idToken is not null)
                            {
                                creds[TokenCacheExtensions.IdTokenKey] =
                                    idToken;
                            }

                            return creds;
                        }

                        return default;
                    });

                builder.Logout(
                    async (provider, dispatcher, credentials, ct) =>
                    {
                        var client =
                            provider.GetRequiredService<IAuthZeroClient>();
                        await client.LogoutAsync(cancellationToken: ct);
                        return true;
                    });

                builder.Refresh(
                    async (provider, dispatcher, credentials, ct) =>
                    {
                        var client =
                            provider.GetRequiredService<IAuthZeroClient>();
                        var tokens = provider.GetRequiredService<ITokenCache>();
                        var token = await tokens.RefreshTokenAsync(ct);
                        if (string.IsNullOrWhiteSpace(token))
                        {
                            return default;
                        }

                        var result = await client.RefreshTokenAsync(token, ct);
                        var accessToken = result.AccessToken;
                        var refreshToken = result.RefreshToken;
                        var idToken = result.IdentityToken;

                        if (token is not null)
                        {
                            var creds = new Dictionary<string, string>
                            {
                                {
                                    TokenCacheExtensions.AccessTokenKey,
                                    accessToken
                                }
                            };
                            if (refreshToken is not null)
                            {
                                creds[TokenCacheExtensions.RefreshTokenKey] =
                                    refreshToken;
                            }

                            if (idToken is not null)
                            {
                                creds[TokenCacheExtensions.IdTokenKey] =
                                    idToken;
                            }

                            return creds;
                        }

                        return default;
                    });
            },
            name: "Auth0");

    private static void RegisterRoutes(IViewRegistry views,
        IRouteRegistry routes)
    {
        views.Register(
            new ViewMap(ViewModel: typeof(ShellModel)),
            new ViewMap<MainPage, MainViewModel>(),
            new ViewMap<InventoryPage, InventoryViewModel>()
        );

        routes.Register(
            new RouteMap(
                "",
                View: views.FindByViewModel<ShellModel>(),
                Nested:
                [
                    new RouteMap(
                        "Main",
                        View: views.FindByViewModel<MainViewModel>(),
                        IsDefault: true),
                    new RouteMap(
                        "Inventory",
                        View: views.FindByViewModel<InventoryViewModel>(),
                        Nested:
                        [
                            new RouteMap("All Items"),
                            new RouteMap("Borrowed"),
                            new RouteMap("Available")
                        ]),
                ]
            )
        );
    }
}
