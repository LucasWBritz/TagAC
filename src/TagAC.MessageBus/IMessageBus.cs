using EasyNetQ;
using System;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Domain.Events;

namespace TagAC.MessageBus
{
    public interface IMessageBus : IDisposable
    {
        bool IsConnected { get; }
        IAdvancedBus AdvancedBus { get; }

        Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken) where T : Event;

        Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage, CancellationToken cancellationToken) where T : class;

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) where TRequest : Event where TResponse : TagAC.Domain.Messages.Message;
        Task RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder, CancellationToken cancellationToken) where TRequest : Event where TResponse : TagAC.Domain.Messages.Message;
    }
}
