﻿using System;
using System.Windows.Forms;
using FirstCardPlus.Forms;

namespace FirstCardPlus
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new f_Main());
        }
    }
}
