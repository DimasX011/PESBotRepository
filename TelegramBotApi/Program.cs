using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;
using TelegramBotData;
using Microsoft.EntityFrameworkCore;

namespace TelegramBotExperiments
{

    class Program
    {

        static ITelegramBotClient bot = new TelegramBotClient("5222140612:AAHfBdFbCoZheGo6tu5FCP6g_g68hjKsm6E");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // ��������� ��������
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;


                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "����� ���������� �� ����, ������ ������!");
                    return;
                }
                else if (message.Text == "������")
                {
                    var ikm = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("�������", "myCommand1"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("������", "myCommand2"),
                        },
                    });

                    await bot.SendTextMessageAsync(message.Chat.Id, "������� ������ 1", replyMarkup: ikm);
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "������-������!!");
            }

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                CallbackQuery callbackQuery = update.CallbackQuery;
                var message = update.Message;
                if (message != null || callbackQuery.Data != null)
                {
                    if (callbackQuery.Data == "myCommand1")
                    {
                        await botClient.SendTextMessageAsync(callbackQuery.From.Id, "����� ������ 1!");
                    }
                }

            }

        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // ��������� ��������
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


       static async Task BotStart()
        {
            Console.WriteLine("������� ��� " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            while (true)
            {
                bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
                Console.ReadLine();
            }
        }

        static public async void BotCall()
        {
            await Task.Run(() => BotStart());
        }

        static  void Main(string[] args)
        {
            var builder =  WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TelegebBotDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });

            var app = builder.Build();

            BotCall();
            
            app.Run();

          
        }
    }
}