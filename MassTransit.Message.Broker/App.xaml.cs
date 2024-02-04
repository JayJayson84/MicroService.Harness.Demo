using System.Windows;
using MassTransit.Message.Broker.Models;
using MassTransit.Message.Broker.Services;
using MassTransit.Message.Broker.Services.Implementation;
using MassTransit.Message.Broker.ViewModels;
using MassTransit.Message.Broker.Views;
using MicroService.Security.Encryption.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MassTransit.Message.Broker;

public partial class App : Application
{

    private readonly IHost _host;

    public App()
    {
        _host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                //MassTransit
                services.AddMassTransit(x =>
                {
                    x.AddRequestClient<EncryptionContract>();

                    x.SetKebabCaseEndpointNameFormatter();

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(Connection.Host, host =>
                        {
                            host.Username(Connection.Credentials.Username);
                            host.Password(Connection.Credentials.Password);
                        });


                        cfg.ConfigureEndpoints(context);
                    });
                });

                // ViewModels
                services.AddSingleton<MainWindowViewModel>();

                // Services
                services.AddSingleton<IRabbitClientService, RabbitClientService>();
                services.AddSingleton<IMessagingService, MassTransitMessagingService>();

                // Windows
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        ((MainWindowViewModel)MainWindow.DataContext).OnRequestClose += (s, e) => MainWindow.Close();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();

        base.OnExit(e);
    }

}
