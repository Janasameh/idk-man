using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hamada.RequestModels
{
    public class LoginRequestModel
    {
        public required string username { get; set; }
        public required string password{ get; set; }
    }
}