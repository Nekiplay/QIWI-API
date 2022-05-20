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
            Console.WriteLine("Ссылка на донат: " + link);
            while (true)
            {
                
            }
        }
        static void NewDonates(QIWI.Donation.Event response)
        {
           Console.WriteLine("ID: " + response.eventExtId);
           Console.WriteLine("Nickname: " + response.attributes.DONATION_SENDER);
           Console.WriteLine("Ammount: " + response.attributes.DONATION_AMOUNT);
           Console.WriteLine("Currency: " + response.attributes.DONATION_CURRENCY);
           Console.WriteLine("Message: " + response.attributes.DONATION_MESSAGE);
        }
    }
}
