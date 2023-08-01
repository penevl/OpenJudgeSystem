namespace OJS.Services.Common.Implementations;

using System.Threading.Tasks;
using MassTransit;

public class PublisherService : IPublisherService
{
    private readonly IBus bus;

    public PublisherService(IBus bus)
        => this.bus = bus;

    public Task Publish<T>(T obj)
        where T : class
        => this.bus.Publish(obj);
}