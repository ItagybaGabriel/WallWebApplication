using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;

namespace WallWebApplication.Domain.Models
{
    public class Post
    {
        public int Id { get; set; }
        public String Titulo { get; set; }
        public String Conteudo { get; set; }
        public DateTime Date { get; set; }
        public int LikesCount { get; set; }
        public ICollection<Like> Likes {get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
