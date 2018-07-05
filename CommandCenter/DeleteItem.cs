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
    public class DeleteItem
    {
        string myConnectionString = "server=localhost;database=todo;uid=root;pwd=root;";


        public dynamic Args { get; set; }
        public int IntegrationID { get; set; }
        private DefaultConnectionContext db = new DefaultConnectionContext();

        public DeleteItem(dynamic Args, int IntegrationID)
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
            MessageClass m = new MessageClass();
            MessageAttachmentsClass attach = new MessageAttachmentsClass();
            AttachmentFieldClass field = new AttachmentFieldClass();
            field.Title = "Deleted Item";
            field.Value = title;
            attach.AttachField(field);
            m.Attach(attach);
            string query = "DELETE FROM todo_list WHERE title=\"" + title + "\" and integ_id=" + IntegrationID.ToString();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();

            con.Close();



            return JsonConvert.SerializeObject(m);
        }
    }
}