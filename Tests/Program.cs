using QIWIAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    class Program
    {
        static QIWIAPI.QIWI.Donation donation = null;
        static void Main(string[] args)
        {
            donation = new QIWIAPI.QIWI.Donation("", NewDonates, true);
            Console.ReadKey(true);
        }
        static void NewDonates(QIWI.Donation.DonateResponse response)
        {
           Console.WriteLine("Token: " + response.Token);
           Console.WriteLine("ID: " + response.MessageId);
           Console.WriteLine("Nickname: " + response.Nickname);
           Console.WriteLine("Ammount: " + response.Ammount);
           Console.WriteLine("Currency: " + response.Currency);
           Console.WriteLine("Message: " + response.Message);
        }
    }
}
