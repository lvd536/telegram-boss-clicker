using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ClickerBot.Game.Clicker.Callbacks;
using ClickerBot.Game.Start;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7371147310:AAEwln2CDIWVzYNTFHMdUwbzyzHod1qgDDQ", cancellationToken: cts.Token);
var me = await bot.GetMe();
var startCommand = new StartCommand();
var clickerCall = new ClickerCallback();
bot.OnMessage += OnMessage;
bot.OnUpdate += OnCallbackQuery;
bot.OnError += OnError;
Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();

async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is null) return;
    var commandParts = msg.Text.Split(' ');
    var command = commandParts[0];
    var argument = commandParts.Length == 2 ? commandParts[1] : null;
    var defArgument = commandParts.Length == 3 ? commandParts[2] : null;
    if (msg.Text.StartsWith('/'))
    {
        switch (command)
        {
            case "/start":
                await Task.Run(async () => 
                    await startCommand.StartCmd(bot, msg)
                );
                break;

            case "/click":
                await Task.Run(async () => 
                    await clickerCall.ClickCallback(bot, msg)
                );
                break;
            
            case "test":
                if (argument is not null)
                {
                    
                }
                else if (defArgument is not null)
                {
                    switch (commandParts[1])
                    {
                    }
                }
                break;
            case "/profile":

                break;
        }
    }
    Console.WriteLine($"[Debug] Received {type} '{msg.Text}' in {msg.Chat}");
}

async Task OnCallbackQuery(Update update)
{
    if (update.Type != UpdateType.CallbackQuery) return;
    switch (update.CallbackQuery?.Data)
    {
        case "OnClick":
            Task backgroundStartCallBackTask =  Task.Run(async () => 
                await clickerCall.ClickCallback(bot, update.CallbackQuery.Message ?? new Message())
            );
            break;
    }
}

async Task OnError(Exception exception, HandleErrorSource handler)
{
    Console.WriteLine(exception);
    await Task.Delay(2000, cts.Token);
}