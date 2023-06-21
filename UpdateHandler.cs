using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public partial class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> logger;
    private readonly WeatherService weatherService;

    public UpdateHandler(
        ILogger<UpdateHandler> logger,
        WeatherService weatherService)
    {
        this.logger = logger;
        this.weatherService = weatherService;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Error while Bot polling.");
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var handleTask = update.Type switch
        {
            UpdateType.Message => HandleMessageUpdateAsync(botClient, update.Message, cancellationToken),
            UpdateType.EditedMessage => HandleEditedMessageUpdateAsync(botClient, update.EditedMessage, cancellationToken),
            _ => HandleUnknownUpdateAsync(botClient, update, cancellationToken)
        };

        try
        {
            await handleTask;
        }
        catch (Exception ex)
        {
            await HandlePollingErrorAsync(botClient, ex, cancellationToken);
        }
    }

    private Task HandleUnknownUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received {updateType} update.", update.Type);

        return Task.CompletedTask;
    }
}