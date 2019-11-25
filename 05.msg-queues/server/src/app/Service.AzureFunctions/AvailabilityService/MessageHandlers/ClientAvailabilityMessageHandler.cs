using System.Threading.Tasks;
using Service.Data;
using Service.Data.Entities;
using Service.MessageQueue.Messages;
using Service.MessageQueue.Processing;

namespace Service.AzureFunctions.AvailabilityService.MessageHandlers
{
    public class ClientAvailabilityMessageHandler : IMessageHandler<ClientAvailabilityMessage>
    {
        private readonly IAvailabilityServiceContext _context;

        public ClientAvailabilityMessageHandler(IAvailabilityServiceContext context)
        {
            _context = context;
        }

        public Task HandleAsync(ClientAvailabilityMessage message)
        {
            var availability = new ClientAvailability
            {
                ClientId = message.ClientId,
                Message = message.Message ?? string.Empty,
                Status = message.Status.ToString(),
                TimeStamp = message.TimeStamp,
            };

            _context.Availabilities.Add(availability);
            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
