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

    public class CreateItem
    {
        string myConnectionString = "server=localhost;database=todo;uid=root;pwd=root;";


        public dynamic Args { get; set; }
        public int IntegrationID { get; set; }
        private DefaultConnectionContext db = new DefaultConnectionContext();

        public CreateItem(dynamic Args, int IntegrationID)
        {
            this.Args = Args;
            this.IntegrationID = IntegrationID;


        }

        public string Process()
        {
            MySqlConnection con;
            con = new MySqlConnection(myConnectionString);
            con.Open();
            string title = Args["Title"];
            string description = "";
            if(Args["Description"]!=null)
                 description = Args["Description"];
            MessageClass m = new MessageClass();
            m.MessageText = "Created item\n";
            string query = "INSERT INTO todo_list (title,description,integ_id) values (\'"+title+"\',\'"+description+"\',\'"+IntegrationID+"\')";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();

            con.Close();



            return JsonConvert.SerializeObject(m);
        }
    }
}