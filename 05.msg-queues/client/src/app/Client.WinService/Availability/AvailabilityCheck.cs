using System;
using System.Threading;
using Client.Data.Configuration;
using Client.MessageQueue;
using Client.MessageQueue.Messages;
using Client.MessageQueue.Messages.Enums;
using Serilog;

namespace Client.ScanService.Availability
{
    public class AvailabilityCheck : IAvailabilityCheck
    {
        private readonly IMessageSender _messageSender;

        private readonly ScanServiceContext _context;

        private readonly string _clientId;

        private readonly int _checkFrequencySeconds;

        private Timer _timer;

        public AvailabilityCheck(
            IMessageSender messageSender,
            ScanServiceContext context,
            string clientId,
            int checkFrequencySeconds)
        {
            _messageSender = messageSender;
            _context = context;
            _clientId = clientId;
            _checkFrequencySeconds = checkFrequencySeconds;
        }

        public void StartChecks()
        {
            Log.Debug($"Start availability with interval '{_checkFrequencySeconds}' seconds.");

            _timer = new Timer(
                _ => CheckAvailability(),
                state: null,
                dueTime: 0,
                period: (int)TimeSpan.FromSeconds(_checkFrequencySeconds).TotalMilliseconds);

        }

        private void CheckAvailability()
        {
            try
            {
                if (_context.Database.CanConnect())
                {
                    _messageSender.SendAsync(GetMessage(ClientServiceStatus.Running))
                        .GetAwaiter().GetResult();
                }
                else
                {
                    _messageSender.SendAsync(
                        GetMessage(ClientServiceStatus.Degraded, "Cannot connect to database."))
                        .GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                _messageSender.SendAsync(
                    GetMessage(ClientServiceStatus.Unavailable, e.Message))
                    .GetAwaiter().GetResult();
            }
        }

        private ClientAvailabilityMessage GetMessage(ClientServiceStatus status, string message = null)
        {
            return new ClientAvailabilityMessage
            {
                ClientId = _clientId,
                Message = message,
                Status = status,
                TimeStamp = DateTimeOffset.UtcNow
            };
        }
    }
}
