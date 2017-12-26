using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MAIF
{
    static class Program
    {
        public static bool IsEnergy = false;
        
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length>0)
            {
                if(args[0].Trim().ToLower()=="energy")
                {
                    IsEnergy = true;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            LogHelper h = new LogHelper(true);
            h.Info("Запущена программа пользователем " + Environment.UserName);
        }
    }
}
