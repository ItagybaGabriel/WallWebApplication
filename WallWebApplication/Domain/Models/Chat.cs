using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WallWebApplication.Domain.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public String SenderUserID { get; set; }
        [JsonIgnore]
        public ApplicationUser SenderUser { get; set; }
        public String RecipientUserID { get; set; }
        [JsonIgnore]
        public ApplicationUser RecipientUser { get; set; }
        public List<Message> Messages { get; set; }

    }
}
