using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using yellowantSDK;

namespace YellowAntDemo.CommandCenter
{
    public class DefaultReply
    {
        public string Process()
        {
            MessageClass message = new MessageClass();
            message.MessageText = "I didn't catch that message";
            string ToReturn = JsonConvert.SerializeObject(message);
            return ToReturn;
        }
    }
}