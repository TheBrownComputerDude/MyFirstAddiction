using System.Collections.Generic;

namespace api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public UserInfo UserInfo { get; set; }

        public IList<Video> Videos { get; set; }
    }
}