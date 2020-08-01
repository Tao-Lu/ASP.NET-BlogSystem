using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DTO
{
    public class UserInformationDTO
    {
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string BlogName { get; set; }
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }
    }
}
