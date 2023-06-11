using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _6_5ApiHomework.Data
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<UserLikedJokes> UserLikedJokes { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

    }
}
