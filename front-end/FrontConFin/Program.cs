using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FrontConFin
{
    internal static class Program
    {
        public static IConfiguration Configuration;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();

            ApplicationConfiguration.Initialize();

            using (FrmLogin form = new FrmLogin())
            {
                var result = form.ShowDialog();

                // Se clicou em "Cancelar" ou fechou o login, encerra o programa
                if (result != DialogResult.OK)
                {
                    return; // termina o Main(), encerrando a aplicação
                }
            }

            Application.Run(new FrmPrincipal());
        }
    }
}