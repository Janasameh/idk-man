using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hamada.ResponseModels
{
    public class LoginResponseModel
    {
         public required string Id { set; get; }
        public required string Username { set; get; }
        public required string Token { set; get; }
    }
}