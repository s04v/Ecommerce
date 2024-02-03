using Common.Data;
using Core.Auth.Domain;
using Core.Auth.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Auth.EventHandlers
{
    public class AddLoginHistoryHandler : INotificationHandler<SuccessLoginEvent>
    {
        private readonly IApplicationDbContext _dbContext;

        public AddLoginHistoryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(SuccessLoginEvent notification, CancellationToken cancellationToken)
        {
            var history = new LoginHistory
            {
                UserUuid = notification.UserUuid,
                Device = notification.Device,
                IpAddress = notification.IpAddress,
                Date = DateTime.UtcNow,
            };

            await _dbContext.LoginHistory
                .AddAsync(history);

            await _dbContext.SaveChangesAsync();
        }
    }
}
