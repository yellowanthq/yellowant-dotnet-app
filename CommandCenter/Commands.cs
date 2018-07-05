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
                case "get_list":
                    {
                        GetList cmd = new GetList(Args = Args, IntegrationID = IntegrationID);
                        cmd.Args = Args;
                        cmd.IntegrationID = IntegrationID;
                        return cmd.Process();
                    }
                case "create_item":
                    {
                        CreateItem cmd = new CreateItem(Args = Args, IntegrationID = IntegrationID);
                        cmd.Args = Args;
                        cmd.IntegrationID = IntegrationID;
                        return cmd.Process();
                    }
                case "getitem":
                    {
                        GetItem cmd = new GetItem(Args = Args, IntegrationID = IntegrationID);
                        cmd.Args = Args;
                        cmd.IntegrationID = IntegrationID;
                        return cmd.Process();
                    }
                case "deleteitem":
                    {
                        DeleteItem cmd = new DeleteItem(Args = Args, IntegrationID = IntegrationID);
                        cmd.Args = Args;
                        cmd.IntegrationID = IntegrationID;
                        return cmd.Process();

                    }
                case "updateitem":
                    {
                        UpdateItem cmd = new UpdateItem(Args = Args, IntegrationID = IntegrationID);
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
