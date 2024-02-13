using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Auth.Domain
{
    public class LoginHistory
    {
        public int Id { get; set; }
        public Guid UserUuid { get; set; }
        public string IpAddress { get; set; }
        public string Device { get; set; }
        public DateTime Date { get; set; }
    }
}
