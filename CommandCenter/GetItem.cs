using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YellowAntDemo.Models;
using yellowantSDK;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace YellowAntDemo.CommandCenter
{
    public class GetItem
    {
        string myConnectionString = "server=localhost;database=todo;uid=root;pwd=root;";
        public dynamic Args { get; set; }
        public int IntegrationID { get; set; }
        private DefaultConnectionContext db = new DefaultConnectionContext();


        public GetItem(dynamic Args, int IntegrationID)
        {
            this.Args = Args;
            this.IntegrationID = IntegrationID;


        }
        public string Process()
        {
            MySqlConnection con;
            con = new MySqlConnection(myConnectionString);
            con.Open();
            MessageClass m = new MessageClass();
            m.MessageText = "Getting List\n";
            string title = Args["Title"];
            
            string query = "SELECT * FROM todo_list WHERE title=\"" + title+"\" integ_id="+IntegrationID.ToString();
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine("Hello");
                //m.MessageText += IntegrationID + "  ";
                m.MessageText += dataReader["title"] + "  ";
                m.MessageText += dataReader["description"] + "\n";
            }



            return JsonConvert.SerializeObject(m);
        }
    }
}