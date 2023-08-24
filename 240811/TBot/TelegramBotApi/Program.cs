using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bots.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;
using TelegramBotData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using TelegramBotApi;
using YandexTask;
using ConsoleApp1.Tasks;
using Telegram.Bots.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using TelegramBotApi.Interfaces;
using TelegramBotApi.Services;
using Polly;
using Serilog;
using Serilog.Events;

namespace TelegramBotExperiments
{

    class Program
    {
        private static List<TaskElement> _tasks;
       
        private static Object _SyncObject = new();
        private static ISetToDataBase _dataBase = new WriteToDataBase();
        static ITelegramBotClient bot = new TelegramBotClient("6371768721:AAGjELeYOaCBqtz4VFhUdyC5nHenGLJd15c");
        static string ConnectionString = "data source=localhost\\SQLEXPRESS;initial catalog=TelegramBotData;User Id=TelegramBotUser;Password=1587panda;MultipleActiveResultSets=True;trustServerCertificate=true;App=EntityFramework";
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;

                if (message.Text != null)
                {
                    if (message.Text.ToLower() == "/start")
                    {
                        bool isfirstcontact;

                        lock (_SyncObject)
                        {
                            isfirstcontact = _dataBase.FirstCallContact(message.From.Id, ConnectionString);
                        }
                      
                        if (isfirstcontact)
                        {
                             GetContactUserData(botClient, update);
                        }
                        else
                        {
                            var ikm = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Начинаем", "Start"),
                            },
                        });
                            await bot.SendTextMessageAsync(message.Chat.Id, "Начнем работать!", replyMarkup: ikm);
                        }
                        return;
                    }
                    
                }
                if(update.Message.ReplyToMessage!= null)
                {
                    if (update.Message.ReplyToMessage.Text == "Нажми кнопку для передачи контакта")
                    {
                        lock (_SyncObject)
                        {
                            _dataBase.SetNummerPhoneToDataBase(message.Chat.Username, message.Contact.PhoneNumber, message.Chat.Id, message.Chat.LastName, message.Chat.FirstName, ConnectionString);
                            DeleteButtonUserData(botClient, update);
                        }
                           
                        var ikm = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Начинаем", "Start"),
                            },
                        });
                        await bot.SendTextMessageAsync(message.Chat.Id, "Начнем работать!", replyMarkup: ikm);
                    }
                }
               
            }
           
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                Telegram.Bot.Types.CallbackQuery callbackQuery = update.CallbackQuery;
                var message = update.Message;
                if (message != null || callbackQuery.Data != null)
                {
                    if (callbackQuery.Data == "UserData")
                    {
                        UserEntityLogin userEntity = new();
                        lock (_SyncObject)
                        {
                            userEntity = _dataBase.GetUserData("'" + callbackQuery.From.Username + "'", ConnectionString);
                        }
                        await botClient.SendTextMessageAsync(callbackQuery.From.Id, "Ваш логин " + userEntity.UserLogin +"\n"+  "Ваш пароль " + userEntity.UserPassword );
                    }
                    else if (callbackQuery.Data == "Start")
                    {
                        var ikm = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
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
        public static bool FirstCallContact(long idContact)
        {
            bool addcontacttodatabase = false;
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        using (var sqlCommand = new SqlCommand(String.Format("  SELECT COUNT(*) AS CountUser FROM [TelegramBotData].[dbo].[userData] WHERE TelegramId = {0}", idContact), sqlConnection))
                        {
                            var reader = sqlCommand.ExecuteReader();
                            while (reader.Read())
                            {
                                int countsql = Convert.ToInt32(reader["CountUser"].ToString());
                                if (countsql == 0)
                                {
                                    addcontacttodatabase = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

            return addcontacttodatabase;
            
        }
        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
        public static async void GetContactUserData(ITelegramBotClient botClient, Telegram.Bot.Types.Update update)
        {
            var message = update.Message;
            await botClient.SendTextMessageAsync(message.Chat, "Здравствуйте, вас приветствует бот для сотрудников компании ПЭС, поделитесь своим номером, чтобы управлять своей учетной записью с помощью этого бота");

            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                            KeyboardButton.WithRequestContact("Поделиться контактом")
            });

            await botClient.SendTextMessageAsync(message.Chat, "Нажми кнопку для передачи контакта", replyMarkup: replyMarkup);
      



        }
        public static async void DeleteButtonUserData(ITelegramBotClient botClient, Telegram.Bot.Types.Update update)
        {
            var message = update.Message;
            var replyMarkup = new ReplyKeyboardRemove();

            await botClient.SendTextMessageAsync(message.Chat.Id, "Спасибо", replyMarkup: replyMarkup);
        }
        static async Task BotStart()
        {
           
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
          

            TimerCallback tm = new TimerCallback(Count);

            Timer timer = new Timer(tm, null, 0, 600000);


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
        public static void Count(object obj)
        {
            Task.Run(async () =>
            {
                await UpdateTasksToDataBase();
            });
        }
        static public async Task UpdateTasksToDataBase()
        {
            Complite complite = new Complite();
            await complite.CopliteTaskforYandex();
            _tasks = complite.GetTasks();
            foreach (var task in _tasks)
            {
                bool taskiswrite;
                lock (_SyncObject)
                {
                    taskiswrite = _dataBase.SetTaskToDataBase(task.summary, task.status.display, ConnectionString);
                }
                if (taskiswrite)
                {
                    Console.WriteLine("Данные по задаче " + task.summary + " записаны или обновлены.");
                }
            }

        }
        static public async void BotCall()
        {
            await Task.Run(() => BotStart());
        }
        static async Task ОтправитьСообщениВасе()
        {
            ChatId message = 1478566984;
            await bot.SendTextMessageAsync(message, "Василий здравствуй!");
        }
        static void Main(string[] args)
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