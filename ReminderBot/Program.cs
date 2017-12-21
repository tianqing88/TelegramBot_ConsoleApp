using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ReminderBot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("417221523:AAGwuDkXIGSILuxrwcu7PEN8JuPqkGUhpTg");
        private static ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup();
        public static string type;
        public static string description;
        public static long time;

        static void Main(string[] args)
        {
            Bot.StartReceiving();

            Bot.OnMessage += Bot_OnMessage;
            Bot.OnMessageEdited += Bot_OnMessage;
            
            Console.ReadLine();
            Bot.StopReceiving();
        }
        

        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var txt = e.Message.Text;
            var cid = e.Message.Chat.Id;
            var name = e.Message.From.FirstName + " " + e.Message.From.LastName;
            var uid = e.Message.From.Id;
            if (txt == "/start" || txt == "Cancel")
            {
                SetUpStartKeyboard();

                var time = CheckTimeOfTheDay();

                var message = String.Format("Good {0} {1}, how can I help you today?", time, name);

                await Bot.SendTextMessageAsync(cid, message, ParseMode.Default, false, false, 0, markup);
            }
            else if (txt == "General Reminder")
            {
                type = "General Reminder";
                markup.Keyboard = new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Cancel")
                    }
                };

                await Bot.SendTextMessageAsync(cid, "Please enter the description for your reminder.", ParseMode.Default, false, false, 0, markup);
            }
            else if (e.Message.Type == MessageType.TextMessage && !IsValidTime(e.Message.Text))
            {
                long hour = GetCurrentDatetime();
                description = e.Message.Text;
                time = hour;
                markup.Keyboard = new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton(String.Format("{0}:00", hour)),
                        new KeyboardButton(String.Format("{0}:00", hour + 1))
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton(String.Format("{0}:00", hour + 2)),
                        new KeyboardButton(String.Format("{0}:00", hour + 3))
                    }
                };

                await Bot.SendTextMessageAsync(cid, "Please choose a time for your reminder.", ParseMode.Default, false, false, 0, markup);
            }

            if (IsValidTime(e.Message.Text))
            {
                ReplyKeyboardRemove remove = new ReplyKeyboardRemove()
                {
                    RemoveKeyboard = true
                };

                string message = String.Format("You are all set! \nReminding for: {0} \nTime: {1}:00", description, time);
                Console.WriteLine(message);
                await Bot.SendTextMessageAsync(cid, message, ParseMode.Default, false, false, 0, remove);
            }

            if (e.Message.Type == MessageType.TextMessage)
                Console.WriteLine(e.Message.Text);
        }

        private static string CheckTimeOfTheDay()
        {
            if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 12)
            {
                return "Morning";
            }
            else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
            {
                return "Afternoon";
            }
            else
            {
                return "Evening";
            }
        }

        private static void SetUpStartKeyboard()
        {
            markup.ResizeKeyboard = true;
            markup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("General Reminder")
                }
            };
        }

        private static bool CheckTimeFormat(string time)
        {
            TimeSpan timeSpan;

            return TimeSpan.TryParse(time, out timeSpan);
        }

        private static long GetCurrentDatetime()
        {
            return DateTime.Now.Hour;
        }

        private static bool IsValidTime(string time)
        {
            Regex checktime = new Regex(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
            return checktime.IsMatch(time);
        }
    }
}
