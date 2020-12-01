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


## Using Donate QIWI API
**Create:**
```C#
QIWI.Donation donation = new QIWI.Donation("donate.qiwi.com token", OnDonate);
```
**Get new donate:**
```C#
private void OnDonate(string nickname, double ammount, string currency, string message)
{

}
```
