using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDA.ViewModels.Users
{
    public class UserInfoViewModel
    {
        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<int> GroupIds { get; set; }
        public bool DeliverPushNotification { get; set; }
        public bool DeliverEmail { get; set; }
    }
}
