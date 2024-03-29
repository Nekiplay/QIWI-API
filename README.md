# QIWI-API

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/ed4b8c515ade49bd8fa5e4932f1eee40)](https://www.codacy.com/gh/Nekiplay/QIWI-API/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Nekiplay/QIWI-API&amp;utm_campaign=Badge_Grade)

## Using Wallet API

**Create:**
```C#
QIWI.Wallet wallet = new QIWI.Wallet("QIWI API Key");
```
**Get Balance and get phone:**
```C#
string phone = wallet.Identification.Phone();
double balrub = wallet.Balance.RUB(phone);
double balusd = wallet.Balance.USD(phone);
```
**Send money for other QIWI Wallet:**
```C#
bool done = wallet.Balance.Transfer.QIWIRUB("phone", 1.0, "comment");
```

## Using Donate QIWI API
**Create:**
```C#
QIWI.Donation donation = new QIWI.Donation("donate.qiwi.com token", NewDonates);
```
**Get link for donate:**
```C#
string link = donation.GetDonateLink("nickname", "senderName", "Message", ammount);
```
**Get new donate:**
```C#
private void NewDonates(QIWI.Donation.Event response)
{
    Console.WriteLine("ID: " + response.eventExtId);
    Console.WriteLine("Nickname: " + response.attributes.DONATION_SENDER);
    Console.WriteLine("Ammount: " + response.attributes.DONATION_AMOUNT);
    Console.WriteLine("Currency: " + response.attributes.DONATION_CURRENCY);
    Console.WriteLine("Message: " + response.attributes.DONATION_MESSAGE);
}
```
