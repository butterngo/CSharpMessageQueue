# CSharpMessageQueue
you using dotnet core version 2.2.0 and signalr core 1.1.0 
# Description
This is message queue server it's help you delivery message to client visa signlr. 
You can using javascript signlr or use signlr client from BE received message.
# Configuration server
You need to open appsettings.Development.json and change your configuration or keep it.
- {
  - "AllowedHosts": "*",
  - "EndPoint": "/c-sharp-message-queue",
  - "Host": "http://*:7500",
  - "EnableLog": true
- } 
- AllowedHosts: default all or you can limit host it's up to you.
- EndPoint: you can change your endpoint you want example :"abc...".
- Host: Port for server.
- EnableLog: if you want the system log message set true, else false.
# Configuration log
Open file log4net.config change your folder path inline "log4net.Util.PatternString".
```
  <file type="log4net.Util.PatternString" value="C:\inetpub\wwwroot\SiteOneServiceBus\\logs\\debugger\\%date{dd_MM_yyyy}.log" />
```
# Create Window service
You need create a publish from visual studio or use the command line, continue create file Environment.txt, make follow your Environment, then you just create services follow the article https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/sc-create.
# CSharpMessageQueueClient
you using dotnet core version 2.2.0 and signalr core 1.1.0 
# Description
I develop it by signlr core client, it's help us connect, received, send and tracking message between client and server.
# Documentation
- Configuration host server and client name reminder client name is unique.
```
 public static IServiceCollection RegisterCSharpClient(this IServiceCollection services,
            string host,
            string uniqueKey)
        {
            var signalrClientConnectionFactory = new CSharpClientConnectionFactory(host, uniqueKey);

            services.AddSingleton<ICSharpClientConnectionFactory>(signalrClientConnectionFactory);

            return services;
        }
```
- Start connect to server.
 ```
  public static ICSharpClientConnectionFactory StartConnection(this IApplicationBuilder app)
        {
            var signalr = app.ApplicationServices
                .GetRequiredService<ICSharpClientConnectionFactory>();

            signalr.StartConnection();

            return signalr;
        }
  ```
  # List Events handler
  1. OnHandleReceivedMessage => Received from server.
  ```
  app.StartConnection().OnHandleReceivedMessage += async (message) =>
            {
                //TODO your logic
            };
  ```
  2. OnHandleCompletedMessageReceived => Server return another client have been received message.
  ```
  app.StartConnection().OnHandleCompletedMessageReceived += async (message) => 
            {
                //TODO your logic
            };
  ```
  3. OnNotifyUserConnect => server notify when client connect success.
  ```
    app.StartConnection().OnNotifyUserConnect += async (sender, message) =>
            {
                //TODO your logic
            };
  ```
  4. OnNotifyUserDisconnect => server notify when client disconnect.
   ```
    app.StartConnection().OnNotifyUserDisconnect += async (sender, message) =>
            {
                //TODO your logic
            };
  ```
  5. OnNotifyUserDuplicated => server notify when you initialize same client name.
   ```
    app.StartConnection().OnNotifyUserDuplicated += async (sender, message) =>
            {
                //TODO your logic
            };
  ```
  6. OnLostConnection => when server is death or server have the problem, it will be noftify to this event below
  ```
    app.StartConnection().OnLostConnection += async (sender, message) =>
            {
                //TODO your logic
            };
  ```
  # Method
   1. SendAsync => send message to server.
   ```
    private readonly ICSharpClientConnectionFactory _client;

        public NotificationsController(ICSharpClientConnectionFactory client)
        {
            _client = client;
        }

        [Route("{message}")]
        public async Task<IActionResult> Post(string message)
        {
            await _client.SendAsync(new CSharpMessage
            {
                Body = Encoding.UTF8.GetBytes(message),
                Tos = new string[] { "client2" },
                Label = "test send to client 2"
            });

            return Ok("Done");
        }
   ```
  You can access to my blog we have a lot of example fot it http://www.c-sharp.vn/.
