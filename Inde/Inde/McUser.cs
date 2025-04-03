using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndeService;

    public class McUser
    {
        public int UserKey { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string UserRole { get; set; }
        public DateTime PasswordExpirationDate { get; set; }
        public bool IsNewUser { get; set; }
        public List<ClientAccess> ClientAccessList { get; set; }
        public int QueryFilterGuestTypeKey { get; set; }
        public Guid UserToken { get; set; }
        public DateTime UserTokenExpiration { get; set; }
    }


