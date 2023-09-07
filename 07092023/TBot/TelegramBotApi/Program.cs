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
using Serilog.Core;
using TelegramBotApi.Interfaces;
using TelegramBotApi.Services;
using Polly;
using Serilog;
using Serilog.Events;
using YandexTask.UserYandex;
using System.Diagnostics.Eventing.Reader;
using TelegramBotApi.Entityes;

namespace TelegramBotExperiments
{

    class Program
    {
        private static List<TaskElement> _tasks;
        private static List<UserYandexTracker> _usersYandex;
        private static Serilog.ILogger logger;
        private static Object _SyncObject = new();
        private static bool getinnevent;
        private static ISetToDataBase _dataBase;
        static Dictionary<int, string> userInputs = new Dictionary<int, string>();
        static ITelegramBotClient bot = new TelegramBotClient("6371768721:AAGjELeYOaCBqtz4VFhUdyC5nHenGLJd15c");
        static string ConnectionString = "data source=localhost\\SQLEXPRESS;initial catalog=TelegramBotData;User Id=TelegramBotUser;Password=1587panda;MultipleActiveResultSets=True;trustServerCertificate=true;App=EntityFramework";
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            logger.Information(Newtonsoft.Json.JsonConvert.SerializeObject(update));
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
                            isfirstcontact = _dataBase.FirstCallContact(message.From.Id);
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
                                InlineKeyboardButton.WithCallbackData("Перейти в меню!", "GoToMenu"),
                            },
                        });
                            await bot.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать!", replyMarkup: ikm);
                        }
                        return;
                    }               
                    else if (message.Text.Contains("@yandex"))
                    {
                        var post = message.Text;
                        bool posttodatabase;
                        lock (_SyncObject)
                        {
                            posttodatabase = _dataBase.CheckPostToDataBase(post, message.Chat.Id);
                        }
                            
                        if (!posttodatabase)
                        {
                            bool setsuccess;
                            lock (_SyncObject)
                            {

                                setsuccess = _dataBase.SetPostToDataBase(post, message.Chat.Id);

                            }
                            if (setsuccess)
                            {
                                await bot.SendTextMessageAsync(message.Chat.Id, "Ваша почта добавлена в базу данных");
                                var ikm = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
                                {
                                    new[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("Перейти в меню", "GoToMenu"),
                                    },
                                });
                                await bot.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать!", replyMarkup: ikm);
                            }
                            else
                            {
                                await bot.SendTextMessageAsync(message.Chat.Id, "Произошла ошибка при записи, обратитесь к администратору");
                            }
                            
                        }
                        else
                        {
                           await bot.SendTextMessageAsync(message.Chat.Id, "Ваша почта уже есть в базе данных");
                            var ikm = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
                                {
                                    new[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("Перейти в меню", "GoToMenu"),
                                    },
                                });
                            await bot.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать!", replyMarkup: ikm);

                        }
                    }
                    if (getinnevent)
                    {
                        bool isadmin;
                        lock (_SyncObject)
                        {
                            isadmin = _dataBase.isAdmin(message.From.Id);
                        }
                        if (isadmin)
                        {
                            string INN = message.Text;// update.Message.Text;

                            if(INN.Length == 10 || INN.Length == 12)
                            {
                                UserDatas user = new UserDatas();
                                lock (_SyncObject)
                                {
                                    user = _dataBase.GetUserDatas(INN);
                                }
                                var ikm = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
                                {
                                    new[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("Вернуться в меню!", "GoToMenu"),
                                    },
                                });
                                await botClient.SendTextMessageAsync(message.From.Id, $"Пользователь {user.UserFirtsName} \n {user.UserLastName} имеет корпоративные данные:\n Логин от сервера: {user.LoginServer} \n Пароль от сервера: {Decrypter.DecryptValue(user.PasswordServer)}\n Логин от Яндекса: {user.LoginYandex} \n Пароль от яндекса: {Decrypter.DecryptValue(user.PasswordYandex)}\n Логин от mail: {user.LoginMail} \n Пароль от mail: {Decrypter.DecryptValue(user.PasswordMail)}\n Логин от Битрикса: {user.LoginBitrix} \n Пароль от битрикса: {Decrypter.DecryptValue(user.PasswordBitrix)}", replyMarkup: ikm);
                                getinnevent = false;
                            }
                            else
                            {
                                var ikm = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
                                {
                                    new[]
                                    {
                                        InlineKeyboardButton.WithCallbackData("Вернуться в меню!", "GoToMenu"),
                                    },
                                });
                                await botClient.SendTextMessageAsync(message.From.Id, "Неправильно введен ИНН пользователя, пожалуйста повторите попытку", replyMarkup: ikm);
                            }
                        }
                      

                    }

                }
                if(update.Message.ReplyToMessage!= null)
                {
                    if (update.Message.ReplyToMessage.Text == "Нажми кнопку для передачи контакта")
                    {
                        bool numberisbase;
                        lock (_SyncObject)
                        {
                            //_dataBase.SetNummerPhoneToDataBase(message.Chat.Username, message.Contact.PhoneNumber, message.Chat.Id, message.Chat.LastName, message.Chat.FirstName, ConnectionString, logger);
                            numberisbase = _dataBase.CheckNumberPhoneToDataBase(message.Contact.PhoneNumber, logger);
                            DeleteButtonUserData(botClient, update);
                        }
                        if (numberisbase)
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, "Вы успешно авторизованы, вам открыт доступ к корпоративному чат боту!");
                            lock (_SyncObject)
                            {
                                _dataBase.SetTelegramIdToDataBase(message.Chat.Id,message.Contact.PhoneNumber,logger);
                            }
                        }
                        else
                        {
                            lock (_SyncObject)
                            {
                                _dataBase.SetUnregistredUserToDataBase(message.Chat.Username, message.Contact.PhoneNumber, message.Chat.Id, message.Chat.LastName, message.Chat.FirstName);
                            }
                            //await SendMessageAdministrator(1478566984,String.Format("Незарегистрированный в базе сотрудник {0} {1} обратился к телеграмм боту. Просьба назначить ему данные для входа в корпоративные мессенджеры","@"+message.Chat.Username,message.Contact.PhoneNumber));
                            await SendMessageAdministrator(5933414364, String.Format("Незарегистрированный в базе сотрудник {0} {1} обратился к телеграмм боту. Просьба назначить ему данные для входа в корпоративные мессенджеры", "@" + message.Chat.Username, message.Contact.PhoneNumber));

                            await bot.SendTextMessageAsync(message.Chat.Id, "Вы не найдены в нашей базе данных, информация будет передана администраторам!");
                        }

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
                            //userEntity = _dataBase.GetUserData("'" + callbackQuery.From.Username + "'", logger);
                        }
                        await botClient.SendTextMessageAsync(callbackQuery.From.Id, "Ваш логин " + userEntity.UserLogin +"\n"+  "Ваш пароль " + userEntity.UserPassword );
                    }
     
                    else if(callbackQuery.Data == "UserDatasEntry")
                    {
                        var ikm = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
                       {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Получить данные", "GetDataUsersFromINN"),
                                },
                            });
                        var messages = await botClient.SendTextMessageAsync(callbackQuery.From.Id, "Введите ИНН пользователя:");
                        getinnevent = true;
                        userInputs.Add(messages.MessageId, callbackQuery.From.Id.ToString());
                    }
                    else if(callbackQuery.Data == "GoToMenu")
                    {
                        bool isadmin;
                        lock (_SyncObject)
                        {
                            isadmin = _dataBase.isAdmin(callbackQuery.From.Id);
                        }
                        if(isadmin)
                        {
                          
                           var ikm = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
                        {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Учетные данные пользователей", "UserDatasEntry"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Список забаненных", "myCommand2"),
                        },
                    }
                                   
                           );
                            await bot.SendTextMessageAsync(callbackQuery.From.Id, "Пожалуйста подождите ...", replyMarkup: ikm);
                            return;
                        }


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
    
        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            logger.Error(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
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
            //await bot.SendTextMessageAsync(message.Chat.Id, "У нас нет вашего почтового адреса, пожалуйста введите свой адрес электронной почты");
        }
        static async Task BotStart()
        {
           
            //Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
            logger.Information("Запущен бот " + bot.GetMeAsync().Result.FirstName);

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
            await complite.GetUserDataForYandex();

            _tasks = complite.GetTasks();
            _usersYandex = complite.GetUsers();

            foreach (var task in _tasks)
            {
                bool taskiswrite;
                lock (_SyncObject)
                {
                    taskiswrite = _dataBase.SetTaskToDataBase(task.summary, task.status.display);
                }
                if (taskiswrite)
                {
                    Console.WriteLine("Данные по задаче " + task.summary + " записаны или обновлены.");
                }
            }

            foreach(var task in _usersYandex)
            {
                bool useriswrite;
                lock (_SyncObject)
                {

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

        static async Task SendMessageAdministrator(ChatId chatId, string message)
        {
            await bot.SendTextMessageAsync(chatId, message);
        }

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
             .WriteTo.Console()
             .CreateBootstrapLogger();
                        Log.Information("Starting up");

            logger = Log.Logger;

            _dataBase = new WriteToDataBase(logger);


            var builder =  WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TelegebBotDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });


            builder.Host.UseSerilog((ctx, conf) =>
            {
                conf
                    .MinimumLevel.Debug()
                    .WriteTo.Conditional(
                        evt => evt.Level == LogEventLevel.Information,
                        wt => wt.File("Logs\\log-.txt", rollingInterval: RollingInterval.Day))
                    .WriteTo.Conditional(
                        evt => evt.Level == LogEventLevel.Debug,
                        wt => wt.File("Logs\\log-technical.txt", rollingInterval: RollingInterval.Day))
                    .WriteTo.Conditional(
                        evt => evt.Level == LogEventLevel.Error,
                        wt => wt.File("Logs\\log-error.txt", rollingInterval: RollingInterval.Day))
                    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                    .ReadFrom.Configuration(ctx.Configuration);
            });

            logger = Log.Logger;

            var app = builder.Build();

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();

            BotCall();
          
            app.Run();
           
        }
    }
}