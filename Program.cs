using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ClickerBot.Game.Clicker.Callbacks;
using ClickerBot.Game.Clicker.Commands;
using ClickerBot.Game.Clicker.Items;
using ClickerBot.Game.Clicker.Profile;
using ClickerBot.Game.Clicker.Shop;
using ClickerBot.Game.Start;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7371147310:AAEwln2CDIWVzYNTFHMdUwbzyzHod1qgDDQ", cancellationToken: cts.Token);
var me = await bot.GetMe();
var startCommand = new StartCommand();
var profileCommand = new Profile();
var userNameCall = new ChangeNameCallback();
var clickerCall = new ClickerCallback();
var shopCommand = new Shop();
var helpCommand = new HelpCommand();
var dailyCommand = new DailyRewardCommand();
var topCommand = new TopCommand();
var itemsCommand = new ItemsCommand();
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
    var argument = commandParts.Length >= 2 ? commandParts[1] : null;
    var defArgument = commandParts.Length >= 3 ? commandParts[2] : null;
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
                    await clickerCall.ClickCallback(bot, msg, true)
                );
                break;

            case "/shop":
                await Task.Run(async () =>
                    await shopCommand.ShopCmd(bot, msg)
                );
                break;

            case "/setname":
                if (argument is not null)
                {
                    await userNameCall.ChangeNameAsync(bot, msg, argument);
                }

                /*else if (defArgument is not null)
                {
                    switch (commandParts[1])
                    {
                    }
                }*/
                break;
            case "/profile":
                await Task.Run(async () =>
                    await profileCommand.ProfileCmdAsync(bot, msg)
                );
                break;
            case "/help":
                await Task.Run(async () =>
                    await helpCommand.HelpCmd(bot, msg)
                );
                break;
            case "/daily":
                await Task.Run(async () =>
                    await dailyCommand.DailyRewardCmd(bot, msg)
                );
                break;
            case "/top":
                await Task.Run(async () =>
                    await topCommand.TopCmd(bot, msg, 1)
                );
                break;
            case "/items":
                await Task.Run(async () =>
                    await itemsCommand.ItemList(bot, msg)
                );
                break;
            case "/upgrade":
                if (argument is not null)
                {
                    try
                    {
                        await Task.Run(async () =>
                                await itemsCommand.ItemUpgrade(bot, msg, Convert.ToInt32(argument))
                        );
                    } catch (FormatException)
                    {
                        await Task.Run(async () =>
                                await bot.SendMessage(msg.Chat.Id, "Неверно указан ID предмета. Пример: /upgrade 1", ParseMode.Html)
                        );
                    }
                }
                break;
            case "/craft":
                if (argument is not null && defArgument is not null)
                {
                    try
                    {
                        if (int.Parse(argument) >= 100 && int.Parse(defArgument) >= 10)
                        {
                            await Task.Run(async () =>
                                await itemsCommand.ItemCraft(bot, msg, int.Parse(argument),
                                    int.Parse(defArgument))
                            );
                        }
                        else
                        {
                            await Task.Run(async () =>
                                await bot.SendMessage(msg.Chat.Id,
                                    "Вы указали нулевое значение для крафта. Минимальные значения: 100 монет и 10 кристалов",
                                    ParseMode.Html)
                            );
                        }
                    } catch (FormatException)
                    {
                        await Task.Run(async () =>
                            await bot.SendMessage(msg.Chat.Id, "Неверно указана форма комманды. Пример: /craft money diamonds | /craft 1500 25", ParseMode.Html)
                        );
                    }
                }
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
            await Task.Run(async () =>
                await clickerCall.ClickCallback(bot, update.CallbackQuery.Message ?? new Message(), false)
            );
            break;
        case "Profile":
            await Task.Run(async () =>
                await profileCommand.ProfileCmdAsync(bot, update.CallbackQuery.Message ?? new Message())
            );
            break;
        case "ChangeName":
            await Task.Run(async () =>
                await bot.SendMessage(update.CallbackQuery?.Message.Chat.Id,
                    "<blockquote>Чтобы изменить имя вам необохдимо написать:\n/setname Nick | /setname lvd.\n⚠️ Максимальная длинна ника: 15 символов</blockquote>", ParseMode.Html)
            );
            break;
        case "Shop":
            await Task.Run(async () =>
                await shopCommand.ShopCmd(bot, update.CallbackQuery.Message ?? new Message())
            );
            break;
        case "Shop1":
            await Task.Run(async () =>
                await shopCommand.ShopCallback(bot, update.CallbackQuery.Message ?? new Message(), 1)
            );
            break;
        case "Shop2":
            await Task.Run(async () =>
                await shopCommand.ShopCallback(bot, update.CallbackQuery.Message ?? new Message(), 2)
            );
            break;
        case "Shop3":
            await Task.Run(async () =>
                await shopCommand.ShopCallback(bot, update.CallbackQuery.Message ?? new Message(), 3)
            );
            break;
        case "Shop4":
            await Task.Run(async () =>
                await shopCommand.ShopCallback(bot, update.CallbackQuery.Message ?? new Message(), 4)
            );
            break;
        case "Daily":
            await Task.Run(async () =>
                await dailyCommand.DailyRewardCmd(bot, update.CallbackQuery.Message ?? new Message())
            );
            break;
        case "Top":
            await Task.Run(async () =>
                await topCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 1)
            );
            break;
        case "TopByMoney":
            await Task.Run(async () =>
                await topCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 2)
            );
            break;
        case "TopByCashiers":
            await Task.Run(async () =>
                await topCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 3)
            );
            break;
        case "TopByKills":
            await Task.Run(async () =>
                await topCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 4)
            );
            break;
        case "TopByDamage":
            await Task.Run(async () =>
                await topCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 5)
            );
            break;
        case "TopByElo":
            await Task.Run(async () =>
                await topCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 6)
            );
            break;
        case "ItemsList":
            await Task.Run(async () =>
                await itemsCommand.ItemList(bot, update.CallbackQuery.Message ?? new Message())
            );
            break;
        case "ItemsUpgrade":
            await Task.Run(async () =>
                await bot.SendMessage(update.CallbackQuery?.Message.Chat.Id,
                    "<blockquote>Чтобы улучшить предмет вам необохдимо написать:\n/upgrade itemId | /upgrade 1</blockquote>", ParseMode.Html)
            );
            break;
        case "ItemsCraft":
            await Task.Run(async () =>
                await bot.SendMessage(update.CallbackQuery?.Message.Chat.Id,
                    "<blockquote>Чтобы изменить имя вам необохдимо написать:\n/craft money diamonds | /craft 1500 25\n⚠️ Минимальное кол-во для крафта: 100 монет, 10 алмазов</blockquote>", ParseMode.Html)
            );
            break;
        case "BackToMain":
            await Task.Run(async () =>
                await startCommand.BackCmd(bot, update.CallbackQuery.Message ?? new Message())
            );
            break;
    }
}

async Task OnError(Exception exception, HandleErrorSource handler)
{
    Console.WriteLine(exception);
    await Task.Delay(2000, cts.Token);
}