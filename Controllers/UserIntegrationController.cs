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
        string redirecturl = "https://0c948591.ngrok.io";
        string clientID = "b3vHCSHVrphV0Bf9Kqsw3oGloMU9q2wJivaixTT8";
        string clientSecret = "KA3fh2fyJBOi4zrLBgSLmAxuLj4x5GvgBgF5jcmzgalSbwcs6zsXu5bs7Qg7ir6lcVCd4a6diyv82ONAhdM1AOgbDcVNKWfVDaDE90CCBNGn4gUNYo3iMuDgC9ydrB2l";
        string verificationToken = "yNIwbO1zAPMJrFhsKUVvPcgL2XEIzp5UXItcLFwxsxvbRKFsEstZ3putclmp2Zo9LLFN3KM5Xo1CnJqWF4SKgJkLQU0zE4m5tvicOhBVZhjwlPu03ndtGlWt0mtsgc92";

        private DefaultConnectionContext db = new DefaultConnectionContext();
        // GET: UserIntegration

        [Authorize]
        public ActionResult Integrate()
        {
            return View(db.UserIntegrationContext.ToList());
        }

        [Authorize]
        public ActionResult NewIntegration()
        {
            string ClientID = clientID;
            YellowAntUserState yau = new YellowAntUserState();
            yau.UserState = Guid.NewGuid().ToString();
            yau.UserUniqueID = User.Identity.GetUserId();

            db.YellowAntUserStatesContext.Add(yau);
            db.SaveChanges();

            string url = "http://yellowant.yellowant.com/api/oauth2/authorize/?state=" + yau.UserState;
            url = url + "&client_id=" + ClientID + "&response_type=code&redirect_url=" + redirecturl+"/userintegration/oauthredirect";
            return Redirect(url);
        }


        [Authorize]
        public ActionResult oauthredirect(string state, string code)
        {
            var user = db.YellowAntUserStatesContext.Where(a => a.UserState == state).FirstOrDefault();
            if (user.UserUniqueID != User.Identity.GetUserId())
            {
                return Redirect(redirecturl+"/userintegration/integrate/");
            }

            Yellowant ya = new Yellowant
            {
                AppKey =clientID,
                AppSecret = clientSecret,
                RedirectURI = redirecturl+"/userintegration/oauthredirect",
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
            return Redirect(redirecturl+"/userintegration/integrate/");
        }

        [HttpPost]
        public ActionResult Api()
        {
            //In case you are "NOT using RTM" (Socket connection) if not comment this sectionC:\yellowant\dotnetapp\Controllers\UserIntegrationController.cs
            var data = Request.Form["data"];
            dynamic command = JsonConvert.DeserializeObject(data);
           

            //In case you are "using RTM" Sockets, 
            /*System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string data = new System.IO.StreamReader(req).ReadToEnd();
            dynamic rtm_data = JsonConvert.DeserializeObject(data);
            dynamic command = rtm_data["data"]; */


            if (command["verification_token"] != verificationToken)
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