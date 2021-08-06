using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WallWebApplication.Domain.Models
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }
    }
}
