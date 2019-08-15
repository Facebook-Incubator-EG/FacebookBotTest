using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FacebookTest.Bot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {

        // POST api/values
        [HttpGet]
        public string Get(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.challenge")] string challenge,
            [FromQuery(Name = "hub.verify_token")] string verify_token)
        {
            if (verify_token.Equals("facebook_ok"))
            {
                return challenge;
            }
            else
            {
                return "";
            }
        }

        [HttpPost]
        public void Post()
        {
            var json = (dynamic)null;
            try
            {
                using (StreamReader sr = new StreamReader(this.Request.Body))
                {
                    json = sr.ReadToEnd();
                }
                dynamic data = JsonConvert.DeserializeObject(json);
                postToFB((string)data.entry[0].messaging[0].sender.id, (string)data.entry[0].messaging[0].message.text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private string fbToken = "EAAiuUdvwWOgBAPaLyCSPc2e45MYcNvoNUQGilFTmE1KHtEhLcnGpEyRookKNaaZA4ofthwTVHT659176QJeq4B66t0lG7kkqlSqmfe9y0LJ85nqQvGMYhZAoRFFJpmwES22qVdM0D9teZAriiHWW8Lp9j5OC0Nfe09d3g1ARAZDZD";
        private string postUrl = "https://graph.facebook.com/v2.6/me/messages";

        public void postToFB(string recipientId, string messageText)
        {
            //Post to ApiAi
            string messageTextAnswer = messageText;
            string postParameters = string.Format("access_token={0}&recipient={1}&message={2}", fbToken, "{ id:" + recipientId + "}", "{ text:\"" + messageTextAnswer + "\"}");
            //Response from ApiAI or answer to FB question from user post it to   FB back.
            var client = new HttpClient();
            client.PostAsync(postUrl, new StringContent(postParameters, Encoding.UTF8, "application/json"));
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
