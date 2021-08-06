using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WallWebApplication.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]
        public ICollection<Post> Posts { get; set; }
        public ICollection<Chat> SenderChats { get; set; }
        public ICollection<Chat> RecipientChat { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
 }
