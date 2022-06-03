using Api.Interfaces;
using Api.Models;
using MassTransit;
using System.Threading.Tasks;

namespace Api.Commands
{
    public class Api : IConsumer<Message>
    {
        private readonly IObservable _observable;

        public Api(IObservable observable)
        {
            _observable = observable;
        }

        public Task Consume(ConsumeContext<Message> context)
        {
            _observable.HandlerLog(context.Message);
            return Task.CompletedTask;
        }
    }
}