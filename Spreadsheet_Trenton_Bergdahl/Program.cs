// <copyright file="Program.cs" company="Trenton Bergdahl">
// Copyright (c) Trenton Bergdahl. All rights reserved.
// </copyright>

namespace Spreadsheet_Trenton_Bergdahl
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}