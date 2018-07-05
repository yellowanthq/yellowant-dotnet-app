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
    public class UpdateItem
    {
        string myConnectionString = "server=localhost;database=todo;uid=root;pwd=root;";


        public dynamic Args { get; set; }
        public int IntegrationID { get; set; }
        private DefaultConnectionContext db = new DefaultConnectionContext();

        public UpdateItem(dynamic Args, int IntegrationID)
        {
            this.Args = Args;
            this.IntegrationID = IntegrationID;


        }

        public string Process()
        {
            MySqlConnection con;
            con = new MySqlConnection(myConnectionString);
            con.Open();
            string title = Args["Old-Title"];
            string newtitle = "";
            string description = "";
            if (Args["New-Title"] != null)
                newtitle = Args["New-Title"];
            else
                newtitle = title;
            if(Args["Description"]!=null)
            {
                description = Args["Description"];
            }
            
            MessageClass m = new MessageClass();
            MessageAttachmentsClass attach = new MessageAttachmentsClass();
            AttachmentFieldClass field = new AttachmentFieldClass();
            field.Title = "Updated Item";
            field.Value = title;
            attach.AttachField(field);
            m.Attach(attach);
            string query = "";
            if (description != "")
            {
                query = "UPDATE todo_list SET title =\"" + newtitle + "\",description=\"" + description + "\" WHERE integ_id=" + IntegrationID.ToString() + " AND " + " title=\"" + title + "\"";
            }
            else
            {
                query = "UPDATE todo_list SET title =\"" + newtitle + "\" WHERE integ_id=" + IntegrationID.ToString() + " AND " + " title=\"" + title + "\"";
            }
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.ExecuteNonQuery();

            con.Close();



            return JsonConvert.SerializeObject(m);
        }
    }
}