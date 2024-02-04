using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MassTransit.Message.Broker.Commands;
using MassTransit.Message.Broker.Models;
using MassTransit.Message.Broker.Services;
using MassTransit.Message.Broker.Services.Implementation;
using MicroService.Security.Encryption.Contracts;
using Newtonsoft.Json;

namespace MassTransit.Message.Broker.ViewModels;

internal class MainWindowViewModel(
    IMessagingService messagingService,
    IRabbitClientService rabbitService,
    IClientFactory clientFactory) : ViewModelBase
{

    #region Fields

    readonly IMessagingService _messagingService = messagingService;
    readonly IRabbitClientService _rabbitService = rabbitService;
    readonly IClientFactory _clientFactory = clientFactory;

    #endregion

    #region Properties

    public string Request
    {
        get => _request;
        set
        {
            _request = value;
            OnPropertyChanged();
        }
    }
    private string _request = null!;

    public string Response
    {
        get => _response;
        set
        {
            _response = value;
            OnPropertyChanged();
        }
    }
    private string _response = null!;

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }
    private string _text = "Unencrypted Text";

    public string Key
    {
        get => _key;
        set
        {
            _key = value;
            OnPropertyChanged();
        }
    }
    private string _key = "ABC123!";

    public static IEnumerable<EncryptionMethod> EncryptionMethodSource => Enum.GetValues(typeof(EncryptionMethod)).Cast<EncryptionMethod>();
    public EncryptionMethod SelectedEncryptionMethod
    {
        get => _encryptionMethod;
        set
        {
            _encryptionMethod = value;
            OnPropertyChanged();
        }
    }
    private EncryptionMethod _encryptionMethod = EncryptionMethod.AES;

    public static IEnumerable<EncryptionOperation> EncryptionOperationSource => Enum.GetValues(typeof(EncryptionOperation)).Cast<EncryptionOperation>();
    public EncryptionOperation SelectedEncryptionOperation
    {
        get => _encryptionOperation;
        set
        {
            _encryptionOperation = value;
            OnPropertyChanged();
        }
    }
    private EncryptionOperation _encryptionOperation = EncryptionOperation.Encrypt;

    #endregion

    #region Commands

    public ICommand PublishAsTypedMessageWithDICommand => _publishAsTypedMessageWithDICommand ?? new RelayCommand(PublishAsTypedMessageWithDI);
    private readonly ICommand _publishAsTypedMessageWithDICommand = null!;

    public ICommand SendAsTypedMessageWithDICommand => _sendAsTypedMessageWithDICommand ?? new RelayCommand(SendAsTypedMessageWithDI);
    private readonly ICommand _sendAsTypedMessageWithDICommand = null!;

    public ICommand SendAsEnvelopeWithRabbitMqClientCommand => _sendAsEnvelopeWithRabbitMqClientCommand ?? new RelayCommand(SendAsEnvelopeWithRabbitMqClient);
    private readonly ICommand _sendAsEnvelopeWithRabbitMqClientCommand = null!;

    public ICommand SendAndReceiveAsTypedMessageWithDICommand => _sendAndReceiveAsTypedMessageWithDICommand ?? new RelayCommand(SendAndReceiveAsTypedMessageWithDI);
    private readonly ICommand _sendAndReceiveAsTypedMessageWithDICommand = null!;

    public ICommand SendAndReceiveAsTypedMessageWithoutDICommand => _sendAndReceiveAsTypedMessageWithoutDICommand ?? new RelayCommand(SendAndReceiveAsTypedMessageWithoutDI);
    private readonly ICommand _sendAndReceiveAsTypedMessageWithoutDICommand = null!;

    public ICommand CloseWindowCommand => _closeWindowCommand ?? new RelayCommand(CloseWindow);
    private readonly ICommand _closeWindowCommand = null!;

    #endregion

    #region Events

    public event EventHandler? OnRequestClose;

    #endregion

    #region Private Methods

    // 1. Publish as typed message with DI
    private async void PublishAsTypedMessageWithDI()
    {
        try
        {
            var payload = new
            {
                Value = Text,
                Key,
                EncryptionOperation = SelectedEncryptionOperation.ToString(),
                EncryptionMethod = SelectedEncryptionMethod.ToString()
            };

            await _messagingService.PublishAsync<EncryptionContract>(payload);

            Request = "This is a one way operation that can be debugged by adding a breakpoint in the Consumer.\r\n\r\n" + JsonConvert.SerializeObject(payload, Formatting.Indented);
            Response = "Request completed successfully.";
        }
        catch (Exception ex)
        {
            Response = ex.Message;
        }
    }

    // 2. Send as typed message with DI
    private async void SendAsTypedMessageWithDI()
    {
        try
        {
            var payload = new
            {
                Value = Text,
                Key,
                EncryptionOperation = SelectedEncryptionOperation.ToString(),
                EncryptionMethod = SelectedEncryptionMethod.ToString()
            };

            await _messagingService.SendAsync<EncryptionContract>(payload, Connection.Queue);

            Request = JsonConvert.SerializeObject(payload, Formatting.Indented);
            Response = "This is a one way operation that can be debugged by adding a breakpoint in the Consumer.\r\n\r\n" + "Request completed successfully.";
        }
        catch (Exception ex)
        {
            Response = ex.Message;
        }
    }

    // 3. Send as envelope with RabbitMq client
    private void SendAsEnvelopeWithRabbitMqClient()
    {
        try
        {
            var contract = new
            {
                Value = Text,
                Key,
                EncryptionOperation = SelectedEncryptionOperation.ToString(),
                EncryptionMethod = SelectedEncryptionMethod.ToString()
            };
            var contractType = typeof(EncryptionContract);

            var payload = new
            {
                DestinationAddress = $"{Connection.Host}/{contractType.Namespace}:{contractType.Name}",
                MessageType = new string[] { $"urn:message:{contractType.Namespace}:{contractType.Name}" },
                Message = contract,
                SentTime = DateTime.UtcNow
            };

            Request = "This is a one way operation that can be debugged by adding a breakpoint in the Consumer.\r\n\r\n" + JsonConvert.SerializeObject(payload, Formatting.Indented);

            _rabbitService.SendMessage(payload);

            Response = "Request completed successfully.";
        }
        catch (Exception ex)
        {
            Response = ex.Message;
        }
    }

    // 4. Send/Receive as typed message with DI
    private async void SendAndReceiveAsTypedMessageWithDI()
    {
        try
        {
            var payload = new
            {
                Value = Text,
                Key,
                EncryptionOperation = SelectedEncryptionOperation.ToString(),
                EncryptionMethod = SelectedEncryptionMethod.ToString()
            };

            Request = JsonConvert.SerializeObject(payload, Formatting.Indented);

            var client = _clientFactory.CreateRequestClient<EncryptionContract>();
            var response = await client.GetResponse<EncryptionResponse>(payload);

            Response = JsonConvert.SerializeObject(response.Message, Formatting.Indented);
        }
        catch (Exception ex)
        {
            Response = ex.Message;
        }
    }

    // 5. Send/Receive as typed message without DI
    private async void SendAndReceiveAsTypedMessageWithoutDI()
    {
        try
        {
            var massTransitTransport = new MassTransitRabbitMqTransport();

            var payload = new
            {
                Value = Text,
                Key,
                EncryptionOperation = SelectedEncryptionOperation.ToString(),
                EncryptionMethod = SelectedEncryptionMethod.ToString()
            };

            Request = JsonConvert.SerializeObject(payload, Formatting.Indented);

            var response = await massTransitTransport
                .BusControl
                .Request<EncryptionContract, EncryptionResponse>(new Uri($"{Connection.Host}/{Connection.Queue}"), payload);

            Response = JsonConvert.SerializeObject(response.Message, Formatting.Indented);
        }
        catch (Exception ex)
        {
            Response = ex.Message;
        }
    }

    private void CloseWindow()
    {
        OnRequestClose?.Invoke(this, EventArgs.Empty);
    }

    #endregion

}
