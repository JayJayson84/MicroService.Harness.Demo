## Example .Net Core MicroService Demo
This demo contains the following end-to-end examples for the [MicroService.Harness](https://github.com/JayJayson84/MicroService.Harness).
- WebPage integration for the Producer API.
- WPF desktop application for the Consumer.

## Getting started
1. Clone the repository.
1. Edit `Webpage/microservice-api.html`.
  1. Update the API url on `line 148` with the **Protocol**, **IP Address** and **Port** configured in the [MicroService.Harness ReadMe](https://github.com/JayJayson84/MicroService.Harness?tab=readme-ov-file#readme).
1. Open `MassTransit.Message.Broker/MassTransit.Message.Broker.sln` in Visual Studio.
  1. Edit `C:\git\MicroService.Harness.Demo\MassTransit.Message.Broker\Resources\Settings\Connection.resx`.
  1. Update the **HostName** value with the IP Address for the `eth0` adapter of your WSL Ubuntu host, installed during the [MicroService.Harness ReadMe](https://github.com/JayJayson84/MicroService.Harness?tab=readme-ov-file#readme). (Run `ifconfig` in the bash terminal to get the IP).
  1. Update the **Password** with the unencrypted `Broker` password configured in the `Generate RabbitMQ password hashes` section of the [MicroService.Harness ReadMe](https://github.com/JayJayson84/MicroService.Harness?tab=readme-ov-file#readme).
> [!IMPORTANT]
> The credentials are intended for local development only. Take care to avoid committing passwords into public and/or shared repositories and follow recommended practices for securely implementing a password store in a real world application.

## WebPage Usage
1. Open `Webpage/microservice-api.html` in your browser.
1. Fill in and submit the form.
1. A POST request will be sent to the producer API and the response written to the page.

## WPF Usage
1. Open `MassTransit.Message.Broker/MassTransit.Message.Broker.sln` in Visual Studio.
1. Debug the startup project.
1. Fill in the form in the side panel.
1. Click one of the submission methods below the form in the side panel.
1. A request will be sent to the consumer and the payload and response written to the text boxes on the page.
1. You can see the different ways of sending a request by opening `MassTransit.Message.Broker\ViewModels\MainWindowViewModel.cs` and looking at the methods in the `Private Methods` region.
> [!NOTE]
> The `Publish` and `Send` methods **do not** subscribe to the exchange for a response and can be debugged by adding breakpoints to the consumer in the [MicroService.Harness](https://github.com/JayJayson84/MicroService.Harness). The Send/Receive methods **do** subscribe and the response will include the result from the service.