using Facebook;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SJTWTest
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e, FormView a)
        {
            

        }

        private void PostToFacebookPage(string message, string path)
        {
            string app_id = "481670935272097";
            string app_secret = "f8178bd3fbc98ea5db684c09f7b08f29";
            string scope = "publish_stream,manage_pages,publish_stream";

            if (Request["code"] == null)
            {
                Response.Redirect(string.Format(
                    "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}",
                    app_id, Request.Url.AbsoluteUri, scope));
            }
            else
            {
                Dictionary<string, string> tokens = new Dictionary<string, string>();

                string url = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&scope={2}&code={3}&client_secret={4}",
                    app_id, Request.Url.AbsoluteUri, scope, Request["code"].ToString(), app_secret);

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string vals = reader.ReadToEnd();

                    foreach (string token in vals.Split('&'))
                    {
                        //meh.aspx?token1=steve&token2=jake&...
                        tokens.Add(token.Substring(0, token.IndexOf("=")),
                            token.Substring(token.IndexOf("=") + 1, token.Length - token.IndexOf("=") - 1));
                    }
                }

                
                string access_token = tokens["access_token"];

                //Facebook Page Request

                string PageAccessToken;
                url = String.Format("https://graph.facebook.com/100001689489099/accounts?access_token={0}",access_token);
                HttpWebRequest request2 = WebRequest.Create(url) as HttpWebRequest;

                using (HttpWebResponse response2 = request2.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response2.GetResponseStream());

                    string vals = reader.ReadToEnd();
                    JsonTextReader reader2 = new JsonTextReader(new StringReader(vals));

                    JObject o = JObject.Parse(vals);
                    var me = o["data"][0]["access_token"];
                    PageAccessToken = me.ToString();

                }


                var client = new FacebookClient(access_token);
                client.AppId = app_id;
                client.AppSecret = app_secret;

                FacebookMediaObject facebookUploader = new FacebookMediaObject { FileName = @path, ContentType = "image/jpg" };

                string pathscombined = Path.Combine(Server.MapPath("~"), facebookUploader.FileName);
                var bytes = System.IO.File.ReadAllBytes(pathscombined);
                facebookUploader.SetValue(bytes);

                var postInfo = new Dictionary<string, object>();
                postInfo.Add("message", message);
                postInfo.Add("image", facebookUploader);
               //var fbResult = client.Post("/superjugadores/photos", postInfo);
                var fbResult = client.Post("/groups/389713184506688/photos/", postInfo);
                dynamic result = (IDictionary<string, object>)fbResult;
                //do other works like successful messages/etc



            }
        }

        protected void Post(object sender, EventArgs e)
        {
            string Message = String.Format("{0}", Request.Form["Message"]);
            string Path = String.Format("{0}", Request.Form["myFile"]);
            PostToFacebookPage(Message, Path);
        }
    }
}