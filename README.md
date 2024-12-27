## Karoo-ext binding project for Android .net
#### Tested

- Connect to Karoo system service
- Dispatch effects
- Register events

#### Example
```csharp
Action<bool> act = delegate (bool connected)
{
    if (connected && karooSystemService != null)
    {
        Console.WriteLine("Connected to Karoo System Service !");

        //Dispatch simple notification
        var style = IO.Hammerhead.Karooext.Models.SystemNotification.Style.Event;
        var notif = new IO.Hammerhead.Karooext.Models.SystemNotification("ktpe", "Service Running", null, "KPlus", style, null, null);
        karooSystemService.Dispatch(notif);

        //Doing http request
        var getParams = new Dictionary<string, string>();
        var reqUrl = "https://gg-plus-test.com/pullInfos.php?Id=87nd750r&status=1";
        var http = new OnHttpResponse.MakeHttpRequest("GET", reqUrl, getParams, null, true);
        var resp = (OnHttpResponse r) =>
        {
            if (r.State is HttpResponseState.Complete)
            {
                Console.WriteLine("Completed !");
                var complete = r.State as HttpResponseState.Complete;
                if (complete != null)
                {
                    var bodyArray = complete.GetBody();
                    if (bodyArray != null)
                    {
                        var body = Encoding.Default.GetString(bodyArray);
                        Console.WriteLine("body : " + body);
                    }
                }
            }
            else
            {
                Console.WriteLine("Not completed !");
            }
        };
        karooSystemService.AddConsumer(resp, http);
    }
    else
    {
        Console.WriteLine("Karoo system connection error !");
    }
};
karooSystemService = new KarooSystemService(this);
karooSystemService.Connect(act);
```
