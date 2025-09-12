using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hamada.models
{
    public class User
    {
        [Key]
        public int Id { set; get; }
        public required string Username { set; get; }
        public required string Password { set; get; }

    }
}