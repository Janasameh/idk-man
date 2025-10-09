using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hamada.RequestModels
{
    public class RegisterRequestModel
    {
        public required string Username { set; get; }
        public required string Password { set; get; }
        
        public required string Email { set; get; }
        public required string Phone { set; get; }

    }
}