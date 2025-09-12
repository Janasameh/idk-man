using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hamada.models
{
    public class UserTask
    {
        [Key]
        public required int Id { set; get; }
        public required int UserId { set; get; }
        public string? Description { set; get; }
        public DateTime CreatedAt { set; get; }

    }
}