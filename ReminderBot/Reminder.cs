using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ReminderBot
{
    public static class Reminder
    {
        private static ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup();
        //General Reminder
        public static void GeneralReminder()
        {
            markup.ResizeKeyboard = true;
            markup.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Call GirlFriend")
                }
            };
        }
    }
}
