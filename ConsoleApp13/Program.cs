using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5168524944:AAGBoEEzKWplkxuUAPD73Yb6l9ka9-hUonk");

using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};
Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Ошибка телеграм АПИ:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

botClient.StartReceiving(
    HandleUpdatesAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"К вашим услугам Бот  @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdatesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.Message && update?.Message?.Text != null)
    {
        await HandleMessage(botClient, update.Message);
        return;
    }

    if (update.Type == UpdateType.CallbackQuery)
    {
        await HandleCallbackQuery(botClient, update.CallbackQuery);
        return;
    }
}

async Task HandleMessage(ITelegramBotClient botClient, Message message)
{
    if (message.Text == "/start")
    {

        InlineKeyboardMarkup keyboard = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Все мероприятия ", "allmp"),
            },
        });
        await botClient.SendTextMessageAsync(message.Chat.Id, "Выберете", replyMarkup: keyboard);
        return;
    }
}
async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    
        if (callbackQuery.Data.StartsWith("allmp"))
        {
            InlineKeyboardMarkup keyboard = new(new[]
              {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("It-Fest ", "fest"),
                InlineKeyboardButton.WithCallbackData("Хакатон", "Hack"),
            },
        });
            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберете в клавиатуре:", replyMarkup: keyboard);
            return;
        }
        if (callbackQuery.Data.StartsWith("Hack"))
        {
            InlineKeyboardMarkup keyboard = new(new[]
              {
                new[]
            {
                    InlineKeyboardButton.WithCallbackData("Контактные данные ", "Phonehack"),
                    InlineKeyboardButton.WithCallbackData("Подписаться на мероприятие", "Subhack")
                },
            });
            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберете в клавиатуре:", replyMarkup: keyboard);
            return;
        }
        if (callbackQuery.Data.StartsWith("Phonehack"))
        {
            await botClient.SendTextMessageAsync(
            callbackQuery.Message.Chat.Id,
            $"Организатор:https://stavdeti.online/hackathon-2022/"
        );
        }
        if (callbackQuery.Data.StartsWith("fest"))
        {
            InlineKeyboardMarkup keyboard = new(new[]
              {
                new[]
            {
                    InlineKeyboardButton.WithCallbackData("Контактные данные ", "Phonefest"),
                    InlineKeyboardButton.WithCallbackData("Подписаться на мероприятие", "SubFest")
                },
            });
            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберете в клавиатуре:", replyMarkup: keyboard);
            return;
        }
        if (callbackQuery.Data.StartsWith("SubHack"))
        {
            await botClient.SendTextMessageAsync(
            callbackQuery.Message.Chat.Id,
            $"Вы подписались на мероприятие Хакатон"
        );
        }
    if (callbackQuery.Data.StartsWith("SubFest"))
    {
        await botClient.SendTextMessageAsync(
        callbackQuery.Message.Chat.Id,
        $"Вы подписались на мероприятие IT-FEST"
    );
    }
    if (callbackQuery.Data.StartsWith("Phonefest"))
        {
            await botClient.SendTextMessageAsync(
            callbackQuery.Message.Chat.Id,
            $"Организатор:https://www.xn--80aqmb5ay.online/it-fest-2022"
        );
        }

    }