using EasyNetQ;
using Polly;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Domain.Commands;
using TagAC.Domain.Events;

namespace TagAC.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private IBus _bus;
        private IAdvancedBus _advancedBus;

        private readonly string _connectionString;

        public MessageBus(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void TryConnect()
        {
            if (IsConnected) 
                return;

            var policy = Policy.Handle<EasyNetQException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            policy.Execute(() =>
            {
                _bus = RabbitHutch.CreateBus(_connectionString);
                _advancedBus = _bus.Advanced;
                _advancedBus.Disconnected += OnDisconnect;                
            });
            IsConnected = true;
        }

        private void OnDisconnect(object s, EventArgs e)
        {
            IsConnected = false;

            var policy = Policy.Handle<EasyNetQException>()
                .Or<BrokerUnreachableException>()
                .RetryForever();            

            policy.Execute(TryConnect);
        }

        public bool IsConnected { get; set; }

        public IAdvancedBus AdvancedBus => _bus?.Advanced;
        public void Dispose()
        {
            _bus.Dispose();
        }

        public async Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken) where T : Event
        {
            TryConnect();
            await _bus.PubSub.PublishAsync(message, topic, cancellationToken);
        }

        public async Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage, CancellationToken cancellationToken) where T : class
        {
            TryConnect();
            await _bus.PubSub.SubscribeAsync(subscriptionId, onMessage, cancellationToken);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : Event where TResponse : TagAC.Domain.Messages.Message
        {
            TryConnect();
            return await _bus.Rpc.RequestAsync<TRequest, TResponse>(request, cancellationToken);
        }

        public async Task RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder, CancellationToken cancellationToken)
            where TRequest : Event where TResponse : TagAC.Domain.Messages.Message
        {
            TryConnect();
            await _bus.Rpc.RespondAsync(responder, cancellationToken);
        }
    }
}
