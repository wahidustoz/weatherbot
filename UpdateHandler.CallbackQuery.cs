using Microsoft.Extensions.Caching.Distributed;
using Telegram.Bot;
using Telegram.Bot.Types;

public partial class UpdateHandler
{
    private string[] availableLanguages = { "uz", "ru", "en" };
    private async Task HandleCallbackQueryUpdateAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if(availableLanguages.Contains(callbackQuery.Data))
        {
            await PersistUserLanguageAsync(botClient, callbackQuery.From.Id, callbackQuery.Data, cancellationToken);
        }

        await RemoveMessageAsync(botClient, callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
    }

    private async Task PersistUserLanguageAsync(ITelegramBotClient botClient, long userId, string data, CancellationToken cancellationToken)
    {

        var languageCacheKey = $"Language:{userId}";
        var selectedQuery = data;

        var value = await distributedCache.GetOrCreateAsync(
            key: languageCacheKey, 
            callback: () => data,
            cancellationToken: cancellationToken);
    }
}