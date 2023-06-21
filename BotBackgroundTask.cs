using Telegram.Bot;
using Telegram.Bot.Polling;

public class BotBackgroundTask : BackgroundService
{
    private readonly ILogger<BotBackgroundTask> logger;
    private readonly ITelegramBotClient botClient;
    private readonly IUpdateHandler updateHandler;

    public BotBackgroundTask(
        ILogger<BotBackgroundTask> logger,
        ITelegramBotClient botClient,
        IUpdateHandler updateHandler)
    {
        this.logger = logger;
        this.botClient = botClient;
        this.updateHandler = updateHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var me = await botClient.GetMeAsync(stoppingToken);
        logger.LogInformation("Bot started: {username}", me.Username);

        botClient.StartReceiving(
            updateHandler: updateHandler,
            receiverOptions: default,
            cancellationToken: stoppingToken);
        
        // await botClient.ReceiveAsync(updateHandler.HandleUpdateAsync, updateHandler.HandlePollingErrorAsync, default);
    }
}