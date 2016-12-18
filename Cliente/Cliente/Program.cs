using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            inputLog();
            Application.Run(new Login());
        }

        static void inputLog()
        {
            string input = "";
            DateTime fechaActual = DateTime.UtcNow;
            string ipLocal = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                     ipLocal = ip.ToString();
                }
            }
            input = fechaActual.ToString() + ", " + ipLocal + ", " + "Inicio de la aplicación";
            try
            {
                StreamWriter file = new StreamWriter(@".\log.txt", true);
                file.WriteLine(input);
                file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error escribiendo en el Log: \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
