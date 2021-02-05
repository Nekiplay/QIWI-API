using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QIWIAPI
{
    public class QIWI
    {
        public class Wallet
        {
            public _Balance_ Balance;
            public _Identification_ Identification;

            private string token;
            public Wallet(string token)
            {
                this.token = token;
                this.Balance = new _Balance_(this.token);
                this.Identification = new _Identification_(this.token);
            }

            public class _Identification_
            {
                private string token;
                public _Identification_(string token)
                {
                    this.token = token;
                }
                public string Nickname(string phone)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        wc.Headers.Set("authorization", "Bearer " + this.token);
                        string response = wc.DownloadString("https://edge.qiwi.com//qw-nicknames/v1/persons/" + phone + "/nickname");
                        string First_Name = Regex.Match(response, "\"nickname\":\"(.*)\",\"canChange\":(.*)").Groups[1].Value;
                        if (First_Name != "")
                        {
                            return First_Name;
                        }
                    }
                    return "";
                }
                public string First_Name(string phone)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        wc.Headers.Set("authorization", "Bearer " + this.token);
                        string response = wc.DownloadString("https://edge.qiwi.com/identification/v1/persons/" + phone + "/identification");
                        string First_Name = Regex.Match(response, "\"firstName\":\"(.*)\",\"middleName\":\"(.*)\",\"lastName\":\"(.*)\"").Groups[1].Value;
                        if (First_Name != "")
                        {
                            return First_Name;
                        }
                    }
                    return "";
                }
                public string Middle_Name(string phone)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        wc.Headers.Set("authorization", "Bearer " + this.token);
                        string response = wc.DownloadString("https://edge.qiwi.com/identification/v1/persons/" + phone + "/identification");
                        string Middle_Name = Regex.Match(response, "\"firstName\":\"(.*)\",\"middleName\":\"(.*)\",\"lastName\":\"(.*)\"").Groups[2].Value;
                        if (Middle_Name != "")
                        {
                            return Middle_Name;
                        }
                    }
                    return "";
                }
                public string Last_Name(string phone)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        wc.Headers.Set("authorization", "Bearer " + this.token);
                        string response = wc.DownloadString("https://edge.qiwi.com/identification/v1/persons/" + phone + "/identification");
                        string Last_Name = Regex.Match(response, "\"lastName\":\"(.*)\",\"birthDate\":\"(.*)\"").Groups[1].Value;
                        if (Last_Name != "")
                        {
                            return Last_Name;
                        }
                    }
                    return "";
                }
                public string Phone()
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Set("authorization", "Bearer " + this.token);
                        string response = wc.DownloadString("https://edge.qiwi.com/person-profile/v1/profile/current?authInfoEnabled=true&contractInfoEnabled=true&userInfoEnabled=true");
                        string phone = Regex.Match(response, "\"personId\":(.*),\"registrationDate\":\"(.*)\"").Groups[1].Value;
                        if (phone != "")
                        {
                            return phone;
                        }
                    }
                    return "";
                }
                public string Mail()
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Set("authorization", "Bearer " + this.token);
                        string response = wc.DownloadString("https://edge.qiwi.com/person-profile/v1/profile/current?authInfoEnabled=true&contractInfoEnabled=true&userInfoEnabled=true");
                        string mail = Regex.Match(response, "\"boundEmail\":\"(.*)\",\"emailSettings\"").Groups[1].Value;
                        if (mail != "")
                        {
                            return mail;
                        }
                    }
                    return "";
                }
            }
            public class _Balance_
            {
                private string token;
                public _Transfer_ Transfer;
                
                public _Balance_(string token)
                {
                    this.token = token;
                    this.Transfer = new _Transfer_(this.token);
                }
                public class _Transfer_
                {
                    private string token;
                    public _Transfer_(string token)
                    {
                        this.token = token;
                    }

                    public bool QIWI_RUB_Phone(string phone, double ammount, string comment)
                    {
                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        string jsonv2 = "{\"id\":\"" + 1000 * unixTimestamp + "\",\"sum\":{\"amount\":" + ammount + ",\"currency\":\"" + "643" + "\"},\"paymentMethod\":{\"type\":\"Account\",\"accountId\":\"643\"},\"comment\":\"" + comment + "\",\"fields\":{\"account\":\"" + phone + "\"}}";

                        /* Отправка */
                        try
                        {
                            WebRequest request = WebRequest.Create("https://edge.qiwi.com/sinap/api/v2/terms/99/payments");
                            request.Method = "POST";
                            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(jsonv2);
                            request.ContentType = "application/json";
                            request.Headers["Authorization"] = "Bearer " + this.token;
                            request.ContentLength = byteArray.Length;

                            //записываем данные в поток запроса
                            using (Stream dataStream = request.GetRequestStream())
                            {
                                dataStream.Write(byteArray, 0, byteArray.Length);
                            }

                            WebResponse response = request.GetResponse();
                            using (Stream stream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string read = reader.ReadToEnd();
                                    Console.WriteLine(read);
                                    return true;
                                }
                            }
                            response.Close();
                        } 
                        catch (WebException ex)
                        {
                            Console.WriteLine(ex.Message);
                            return false;
                        }
                        return false;
                    }
                    public enum CardType
                    {
                        Visa,
                        MasterCard,
                        MIR,
                        QIWI_Virtual,
                    }
                    public bool QIWI_RUB_Nickname(string phone, double ammount, string comment)
                    {
                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        string jsonv2 = "{\"id\":\"" + unixTimestamp * 1000 + "\",\"sum\":{\"amount\":1,\"currency\":\"643\"},\"paymentMethod\":{\"accountId\":\"643\",\"type\":\"Account\"},\"fields\":{\"account\":\"Nekiplay\",\"accountType\":\"nickname\",\"comment\":\"1\"}}";

                        /* Отправка */
                        try
                        {
                            WebRequest request = WebRequest.Create("https://edge.qiwi.com/sinap/api/v2/terms/99999/payments");
                            request.Method = "POST";
                            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(jsonv2);
                            request.ContentType = "application/json";
                            request.Headers["Authorization"] = "Bearer " + this.token;
                            request.ContentLength = byteArray.Length;

                            //записываем данные в поток запроса
                            using (Stream dataStream = request.GetRequestStream())
                            {
                                dataStream.Write(byteArray, 0, byteArray.Length);
                            }

                            WebResponse response = request.GetResponse();
                            using (Stream stream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string read = reader.ReadToEnd();
                                    Console.WriteLine(read);
                                    return true;
                                }
                            }
                            response.Close();
                        }
                        catch (WebException ex)
                        {
                            Console.WriteLine(ex.Message);
                            return false;
                        }
                        return false;
                    }
                    public bool QIWI_RUB_Card(CardType card, string cardadres, double ammount, string comment)
                    {
                        cardadres = cardadres.Replace(" ", "");
                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        string jsonv2 = "{\"id\":\"" + 1000 * unixTimestamp + "\",\"sum\":{\"amount\":" + ammount + ",\"currency\":\"" + "643" + "\"},\"paymentMethod\":{\"type\":\"Account\",\"accountId\":\"643\"},\"comment\":\"" + comment + "\",\"fields\":{\"account\":\"" + cardadres + "\"}}";

                        /* Отправка */
                        try
                        {
                            string id = "1963";
                            if (card == CardType.MasterCard)
                                id = "21013";
                            else if (card == CardType.Visa)
                                id = "1963";
                            else if (card == CardType.MIR)
                                id = "31652";
                            else if (card == CardType.QIWI_Virtual)
                                id = "22351";
                            WebRequest request = WebRequest.Create("https://edge.qiwi.com/sinap/api/v2/terms/" + id + "/payments");
                            request.Method = "POST";
                            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(jsonv2);
                            request.ContentType = "application/json";
                            request.Headers["Authorization"] = "Bearer " + this.token;
                            request.ContentLength = byteArray.Length;

                            //записываем данные в поток запроса
                            using (Stream dataStream = request.GetRequestStream())
                            {
                                dataStream.Write(byteArray, 0, byteArray.Length);
                            }

                            WebResponse response = request.GetResponse();
                            using (Stream stream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string read = reader.ReadToEnd();
                                    return true;
                                }
                            }
                            response.Close();
                        }
                        catch (WebException ex)
                        {
                            return false;
                        }
                        return false;
                    }
                }
                public string History(string phone)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        wc.Headers.Set("authorization", "Bearer " + this.token);
                        string response = wc.DownloadString("https://edge.qiwi.com/payment-history/v2/persons/" + phone  + "/payments?rows=10");
                        Console.WriteLine(response);
                        return "";
                    }
                }

                public double RUB(string phone)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Set("authorization", "Bearer " + this.token);
                        string response = wc.DownloadString("https://edge.qiwi.com/funding-sources/v2/persons/" + phone + "/accounts");
                        string rubles = Regex.Match(response, "\"balance\":{\"amount\":(.*),\"currency\":643}").Groups[1].Value;
                        if (rubles != "")
                        {
                            rubles = rubles.Replace(".", ",").Replace(" ", "");
                            double rub = double.Parse(rubles);
                            return rub;
                        }
                        else
                            return 0.0;
                    }
                }
                public double USD(string phone)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Set("authorization", "Bearer " + this.token);
                        string response = wc.DownloadString("https://edge.qiwi.com/funding-sources/v2/persons/" + phone + "/accounts");
                        //Console.WriteLine(response);
                        string usds = Regex.Match(response, "{\"alias\":\"qw_wallet_usd\",\"fsAlias\":\"qb_wallet\",\"bankAlias\":\"QIWI\",\"title\":\"Qiwi Account\",\"type\":{\"id\":\"WALLET\",\"title\":\"Visa QIWI Wallet\"},\"hasBalance\":(.*),\"balance\":{\"amount\":(.*),\"currency\":840},\"currency\":840,\"defaultAccount\":(.*)}").Groups[2].Value;
                        //Console.WriteLine("Доларров: " + usds);
                        if (usds != "")
                        {
                            usds = usds.Replace(" ", "").Replace(".", ",");
                            //Console.WriteLine("Доларров: " + usds);
                            double usd = double.Parse(usds);
                            return usd;
                        }
                        else
                            return 0.0;
                    }
                    return 0.0;
                }
            }
        }
        public class Donation
        {
            private string token;

            private List<Action<DonateResponse>> onDonation { get; set; }
            public Donation(string token, List<Action<DonateResponse>> onDonation, bool updater = true)
            {
                this.token = token;
                this.onDonation = onDonation;
                if (updater)
                    StartAsync();
            }
            public Donation(string token, Action<DonateResponse> onDonation, bool updater = true)
            {
                this.token = token;
                this.onDonation = new List<Action<DonateResponse>>();
                this.onDonation.Add(onDonation);
                if (updater)
                    StartAsync();
            }
            public class DonateResponse
            {
                public string MessageId;
                public string Nickname;
                public string Currency;
                public double Ammount;
                public string Message;
            }
            public DonateResponse GetLastMessage()
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    string response = wc.DownloadString("https://donate.qiwi.com/api/stream/v1/statistics/" + token + "/last-messages?&limit=1");
                    //Console.WriteLine(response);
                    // {"widgetGroupExtId":"faffc6f3-6da1-4582-bbde-5fb22d2515dd","limit":1,"messages":[{"messageExtId":"1bd87237-6618-4108-be0f-255e50d60f76","amount":{"value":5.00,"currency":"RUB"},"senderName":"dives_wg","message":"Ладно еще разок, прикольная акция кстати"}]}
                    string donatetoken = Regex.Match(response, "{\"widgetGroupExtId\":\"(.*)\",\"limit\":1").Groups[1].Value;
                    string messageId = Regex.Match(response, "\"messageExtId\":\"(.*)\",\"amount\"").Groups[1].Value;

                    string ammountst = Regex.Match(response, "\"amount\":{\"value\":(.*),\"currency\":\"(.*)\"}").Groups[1].Value;
                    double ammount = 0;
                    if (ammountst != "")
                    {
                        ammountst = ammountst.Replace(".", ",").Replace(" ", "");
                        ammount = double.Parse(ammountst);
                    }

                    string currency = Regex.Match(response, "\"amount\":{\"value\":(.*),\"currency\":\"(.*)\"},\"senderName\"").Groups[2].Value;

                    string nickname = Regex.Match(response, "\"senderName\":\"(.*)\",\"message\"").Groups[1].Value;

                    string message = Regex.Match(response, "\"message\":\"(.*)\"}]}").Groups[1].Value;

                    DonateResponse donateResponse = new DonateResponse();
                    donateResponse.Nickname = nickname;
                    donateResponse.MessageId = messageId;
                    donateResponse.Ammount = ammount;
                    donateResponse.Currency = currency;
                    donateResponse.Message = message;
                    return donateResponse;
                }
            }
            public string GetDonateLink(string login, string senderName, string message, double ammount, string currency = "RUB")
            {
                try
                {
                    string ammounts = (ammount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    using (WebClient wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        wc.Headers.Set(HttpRequestHeader.ContentType, "application/json;charset=utf-8");
                        wc.Headers.Set(HttpRequestHeader.ContentEncoding, "br");
                        wc.Headers.Set(HttpRequestHeader.Cookie, "token-tail=" + (unixTimestamp * 1000) + "");
                        string link = UnicodeToUTF8(wc.UploadString("https://donate.qiwi.com/api/payment/v1/streamers/" + login + "/payments", "{\"amount\":{\"value\":\"" + ammounts + "\",\"currency\":\"" + currency + "\"},\"login\":\"" + login + "\",\"senderName\":\"" + senderName + "\",\"message\":\"" + message + "\"}"));
                        string linkdone = Regex.Match(link, "{\"redirectUrl\":\"(.*)\"}").Groups[1].Value;
                        return linkdone;
                    }
                } catch { }
                return "";
            }
            private string UnicodeToUTF8(string strFrom)
            {
                byte[] bytes = Encoding.Default.GetBytes(strFrom);

                return Encoding.UTF8.GetString(bytes);

            }

            public class Attributes
            {
                public string DONATION_CURRENCY { get; set; }
                public string DONATION_SENDER { get; set; }
                public string DONATION_MESSAGE { get; set; }
                public double DONATION_AMOUNT { get; set; }
            }

            public class Event
            {
                public string eventExtId { get; set; }
                public string type { get; set; }
                public string status { get; set; }
                public DateTime donateDatetime { get; set; }
                public Attributes attributes { get; set; }
                public List<object> voteResults { get; set; }
                public DateTime createDatetime { get; set; }
            }

            public class Root
            {
                public int limit { get; set; }
                public string queuePriority { get; set; }
                public List<Event> events { get; set; }
            }

            private void StartAsync()
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        using (WebClient wc = new WebClient())
                        {
                            wc.Encoding = Encoding.UTF8;
                            string response = wc.DownloadString("https://donate.qiwi.com/api/stream/v1/widgets/" + this.token + "/events?&limit=1");
                            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response);;
                            if (myDeserializedClass.events.First().type == "DONATION")
                            {
                                string nickname = myDeserializedClass.events.First().attributes.DONATION_SENDER;
                                double ammount = myDeserializedClass.events.First().attributes.DONATION_AMOUNT;
                                string MsgId = myDeserializedClass.events.First().eventExtId;
                                string currency = myDeserializedClass.events.First().attributes.DONATION_CURRENCY;
                                string message = myDeserializedClass.events.First().attributes.DONATION_MESSAGE;
                                if (nickname != "")
                                {
                                    foreach (Action<DonateResponse> action in onDonation)
                                    {
                                        DonateResponse donateresponse = new DonateResponse { Nickname = nickname, Currency = currency, Ammount = ammount, Message = message };
                                        donateresponse.MessageId = MsgId;
                                        action(donateresponse);
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }
    }
}

