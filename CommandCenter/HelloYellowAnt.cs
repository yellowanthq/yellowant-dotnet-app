using System.Linq;
using YellowAntDemo.Models;
using yellowantSDK;
using Newtonsoft.Json;

namespace YellowAntDemo.CommandCenter
{
    public class HelloYellowAnt
    {
        public dynamic Args { get; set; }
        public int IntegrationID { get; set; }
        private DefaultConnectionContext db = new DefaultConnectionContext();

        public HelloYellowAnt(dynamic Args, int IntegrationID)
        {
            this.Args = Args;
            this.IntegrationID = IntegrationID;


        }


        public string Process()
        {
            MessageClass message = new MessageClass();
            message.MessageText = "Hello, {0}";
            message.MessageText = string.Format(message.MessageText, Args["Name"]);

            MessageAttachmentsClass attachment = new MessageAttachmentsClass();
            attachment.Title = "This is Attachment Title";
            attachment.Text = "This is the text of the Message. For more on this please visit 'https://docs.yellowant.com/' ";

            ButtonCommandsClass Bcc = new ButtonCommandsClass();
            Bcc.FunctionName = "hello";
            Bcc.ServiceApplication = IntegrationID;
            Command Cmd = new Command();
            Cmd.Name = "Button";
            Bcc.Data = Cmd;

            MessageButtonClass button = new MessageButtonClass();
            button.Value = "value";
            button.Name = "name";
            button.Text = "Test Button";
            button.Command = Bcc;

            attachment.AttachButton(button);
            message.Attach(attachment);
            string ToReturn = JsonConvert.SerializeObject(message);

            MessageClass ToSendMessage = new MessageClass();
            ToSendMessage.MessageText = "Sending Message";
            var user = db.UserIntegrationContext.Where(a => a.IntegrationID == IntegrationID).FirstOrDefault();
            Yellowant ya = new Yellowant
            {
                AccessToken = user.YellowantIntegrationToken
            };

            ya.SendMessage(IntegrationID, ToSendMessage);
            ToSendMessage.MessageText = "Webhook Message";
            var ExampleData = new {
                first_name = "FirstName",
                last_name = "LastName"

            };
            ToSendMessage.MessageData = ExampleData;
            //string testData = JsonConvert.SerializeObject(ToSendMessage);
            ya.SendWebhookMessage(IntegrationID, "webhook", ToSendMessage);



            return ToReturn;
        }
    }
}