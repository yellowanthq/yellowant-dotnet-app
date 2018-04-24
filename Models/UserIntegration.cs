using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YellowAntDemo.Models
{
    public class UserIntegration
    {
        public int ID { get; set; }
        public string YellowantUserID { get; set; }
        public string YellowantTeamSubdomain { get; set; }
        public int IntegrationID { get; set; }
        public string InvokeName { get; set; }
        public string YellowantIntegrationToken { get; set; }

    }
}