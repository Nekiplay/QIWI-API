# QIWI-API

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
QIWI.Donation donation = new QIWI.Donation("donate.qiwi.com token", OnDonate);
```
**Get link for donate:**
```C#
string link = donation.GetDonateLink("nickname", "senderName", "Message", ammount);
```
**Get new donate:**
```C#
private void OnDonate(string nickname, double ammount, string currency, string message)
{

}
```
