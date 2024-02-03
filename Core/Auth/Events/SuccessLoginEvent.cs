using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Auth.Events
{
    public class SuccessLoginEvent : INotification
    {
        public Guid UserUuid { get; set; }
        public string IpAddress { get; set; }
        public string Device { get; set; }
    }
}
