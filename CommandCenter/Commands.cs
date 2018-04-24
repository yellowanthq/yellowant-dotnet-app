using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YellowAntDemo.CommandCenter
{
    public class Commands
    {
        public string FunctionName { get; set; }
        public int FunctionID { get; set; }
        public int IntegrationID { get; set; }
        public dynamic Args { get; set; }

        public string Process()
        {
            switch (FunctionName)
            {
                case "hello":
                    {
                        HelloYellowAnt cmd = new HelloYellowAnt(Args = Args, IntegrationID = IntegrationID);
                        cmd.Args = Args;
                        cmd.IntegrationID = IntegrationID;
                        return cmd.Process();
                    }
                default:
                    {
                        DefaultReply cmd = new DefaultReply();
                        return cmd.Process();
                    }

            }
        }

    }
}
