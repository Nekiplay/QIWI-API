using QIWIAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    static class Program
    {
        static QIWIAPI.QIWI.Donation donation = null;
        static void Main(string[] args)
        {
            donation = new QIWIAPI.QIWI.Donation("", NewDonates, true);
            string link = donation.GetDonateLink("Neki_play1", "Neki", "Test", 25);
            Console.ReadKey(true);
        }
        static void NewDonates(QIWI.Donation.DonateResponse response)
        {
           Console.WriteLine("ID: " + response.MessageId);
           Console.WriteLine("Nickname: " + response.Nickname);
           Console.WriteLine("Ammount: " + response.Ammount);
           Console.WriteLine("Currency: " + response.Currency);
           Console.WriteLine("Message: " + response.Message);
        }
    }
}
