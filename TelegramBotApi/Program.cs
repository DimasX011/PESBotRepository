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
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using TelegramBotApi;

namespace TelegramBotExperiments
{

    class Program
    {

        static ITelegramBotClient bot = new TelegramBotClient("5222140612:AAHfBdFbCoZheGo6tu5FCP6g_g68hjKsm6E");
        static string ConnectionString = "data source=localhost\\SQLEXPRESS;initial catalog=TelegramBotData;User Id=TelegramBotUser;Password=1587panda;MultipleActiveResultSets=True;trustServerCertificate=true;App=EntityFramework";
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;


                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать в ПЭС - бот!");
                    var ikm = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Начать", "Start"),
                        },
                    });

                    await bot.SendTextMessageAsync(message.Chat.Id, "Для продолжения нажмите начать!", replyMarkup: ikm);

                    return;
                }
                
            }
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                CallbackQuery callbackQuery = update.CallbackQuery;
                var message = update.Message;
                if (message != null || callbackQuery.Data != null)
                {
                    if (callbackQuery.Data == "UserData")
                    {
                        UserEntityLogin userEntity = GetUserData("'" + callbackQuery.From.Username +"'");
                        await botClient.SendTextMessageAsync(callbackQuery.From.Id, "Ваш логин " + userEntity.UserLogin +"\n"+  "Ваш пароль " + userEntity.UserPassword );
                    }
                    else if (callbackQuery.Data == "Start")
                    {
                        var ikm = new InlineKeyboardMarkup(new[]
                        {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Учетные данные", "UserData"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("скрыть", "myCommand2"),
                        },
                    });

                        await bot.SendTextMessageAsync(callbackQuery.From.Id, "Пожалуйста подождите ...", replyMarkup: ikm);
                        return;
                    }
                }
            }

        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


       static async Task BotStart()
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

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

        static UserEntityLogin GetUserData(string Username)
        {
            UserEntityLogin userEntity = new();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString)) 
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand(String.Format("  SELECT [UserLogin], [UserPassword] FROM [TelegramBotData].[dbo].[userData] WHERE UserName = {0}", Username), sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();

                        while (reader.Read())
                        {
                            userEntity.UserLogin = reader["UserLogin"].ToString();
                            userEntity.UserPassword = reader["UserPassword"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
               

            }
            return userEntity;
            
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