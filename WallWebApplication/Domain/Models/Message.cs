using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WallWebApplication.Domain.Models
{
    public class Message
    {

        public int Id { get; set; }
        public int ChatId { get; set; }
        public DateTime Data { get; set; }
        [Required]
        public String MessageBody { get; set; }
        public String UserId { get; set; }
        [JsonIgnore]
        public Chat Chat { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }

    }
}