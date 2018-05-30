using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YellowAntDemo.Models;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using yellowantSDK;
using Newtonsoft.Json.Linq;
using YellowAntDemo.CommandCenter;

namespace YellowAntDemo.Controllers
{
    public class UserIntegrationController : Controller
    {

        private DefaultConnectionContext db = new DefaultConnectionContext();
        // GET: UserIntegration
        
        [Authorize]
        public ActionResult Integrate()
        {
            return View(db.UserIntegrationContext.ToList());
        }

        [Authorize]
        public RedirectResult NewIntegration()
        {
            string ClientID = "FvbTB2WePePZH3Zz7IEEvzPpe84FSosINSG67bus";
            YellowAntUserState yau = new YellowAntUserState();
            yau.UserState = Guid.NewGuid().ToString();
            yau.UserUniqueID = User.Identity.GetUserId();

            db.YellowAntUserStatesContext.Add(yau);
            db.SaveChanges();

            string url = "http://yellowant.yellowant.com/api/oauth2/authorize/?state=" + yau.UserState;
            url = url + "&client_id=" + ClientID + "&response_type=code&redirect_url=" + "http://f0008a02.ngrok.io/userintegration/oauthredirect";
            return Redirect(url);
        }


        [Authorize]
        public ActionResult oauthredirect(string state, string code)
        {
            var user = db.YellowAntUserStatesContext.Where(a => a.UserState == state).FirstOrDefault();
            if (user.UserUniqueID != User.Identity.GetUserId())
            {
                return Redirect("http://f0008a02.ngrok.io/userintegration/integrate/");
            }

            Yellowant ya = new Yellowant
            {
                AppKey = "FvbTB2WePePZH3Zz7IEEvzPpe84FSosINSG67bus",
                AppSecret = "6YMYY9oB9sU8imWBcYM3Z0MCjbnhCBCWbGHDICODyTLPmKXlqCeanEZrL9xNSuhZ9Eja54Mye5OfAPS2ZrJF1trT0Ag2byh31bMGXpFMQsvc2w5loBLuhmpK5q1d8HeT",
                RedirectURI = "http://f0008a02.ngrok.io/userintegration/oauthredirect",
                AccessToken = ""
            };
            dynamic AccessToken = ya.GetAccessToken(code);

            string token = AccessToken.access_token;
            Yellowant yan = new Yellowant
            {
                AccessToken = token
            };

            dynamic user_integration = yan.CreateUserIntegration();
            dynamic user_profile = yan.GetUserProfile();
            UserIntegration integration = new UserIntegration
            {
                YellowantUserID = user.UserUniqueID,
                IntegrationID = user_integration["user_application"],
                InvokeName = user_integration["user_invoke_name"],
                YellowantIntegrationToken = token,
                YellowantTeamSubdomain = "temp"
            };

            db.UserIntegrationContext.Add(integration);
            db.SaveChanges();
            return Redirect("http://f0008a02.ngrok.io/userintegration/integrate/");
        }

        [HttpPost]
        public ActionResult Api()
        {
            //In case you are "NOT using RTM" (Socket connection) if not comment this section
            var data = Request.Form["data"];
            dynamic command = JsonConvert.DeserializeObject(data);

            //In case you are "using RTM" Sockets, 
            /*System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string data = new System.IO.StreamReader(req).ReadToEnd();
            dynamic rtm_data = JsonConvert.DeserializeObject(data);
            dynamic command = rtm_data["data"]; */
            

            if (command["verification_token"] != "f9HWFrRCCzMY9MY9fl5AHFD9jIvGJuvPCkWTn5t6nIGZ0zLYiCO1NgwuhhBLCGJlU2vKGpqmysc6dRntFgnRFvCgP9QQ2cZl2pR9XlN9jSFNalkshesN4Zx4bL8jrRbE")
            {
                JObject er = new JObject();
                er.Add("Error", "Wrong Verification");
                return Content(JsonConvert.SerializeObject(er), "application/json");
            }

            Commands cmd = new Commands()
            {
                FunctionName = command["function_name"],
                FunctionID = command["function"],
                IntegrationID = command["application"],
                Args = command["args"]
            };
            string result = cmd.Process();

            return this.Content(result, "application/json");
        }


    }
}