using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class Login : Form
    {
        private string usuario = "";

        public Login()
        {
            InitializeComponent();
            AcceptButton = button1;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string linea;
            string[] campos; //usuario y contraseña
            string[] separador = new string[] { "::" };
            bool loginOK = false;
            string validacion;

            try
            {
                usuario = textBox1.Text;
                System.IO.StreamReader fichero = new System.IO.StreamReader(@".\usuarios.txt");
                while ((linea = fichero.ReadLine()) != null)
                {
                    campos = linea.Split(separador, StringSplitOptions.None);

                    if (Hash(textBox1.Text) == campos[0] && Hash(textBox2.Text) == campos[1])
                    {
                        loginOK = true;
                        inputLog("Acceso al sistema", "");
                        Control control = new Control(textBox1.Text, Hash(textBox1.Text) + "::" + Hash(textBox2.Text));
                        this.Visible = false;
                        control.Show();
                    }
                }
                if(!loginOK)
                {
                    label3.Visible = true;
                    inputLog("Intento fallido de acceso al sistema", "");
                }
                textBox2.Text = "";
                fichero.Close();
            }
            catch(Exception excepcion)
            {
                MessageBox.Show("No se pudo acceder a la lista de usuarios: " + excepcion);
            }
            
        }

        private void inputLog(string accion, string descripcion)
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
            input = fechaActual.ToString() + ", " + ipLocal + ", " + usuario + ", " + accion;
            if(descripcion != "")
            {
                input += ", " + descripcion;
            }
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string linea;
            string[] campos; //usuario y contraseña
            string[] separador = new string[] { "::" };
            bool registroOK = true;

            try
            {
                usuario = textBox1.Text;
                System.IO.StreamReader fichero = new System.IO.StreamReader(@".\usuarios.txt");
                while ((linea = fichero.ReadLine()) != null)
                {
                    campos = linea.Split(separador, StringSplitOptions.None);
                    if (textBox1.Text == campos[0])
                    {
                        registroOK = false;
                    }
                }
                fichero.Close();

                if(registroOK)
                {
                    //almacenar contraseña en sha ->> escribir en fichero ->> exito!
                    try
                    {
                        string input = Hash(textBox1.Text) + "::" + Hash(textBox2.Text);
                        StreamWriter file = new StreamWriter(@".\usuarios.txt", true);
                        file.WriteLine(input);
                        file.Close();
                        inputLog("Registro de usuario", "Usuario " + textBox1.Text + " registrado correctamente");
                        Sonda.Sonda sonda = new Sonda.Sonda();
                        sonda.creaUsuario(input);
                        MessageBox.Show("Usuario registrado correctamente", "Registro de usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error registrando usuario: \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        inputLog("Registro de usuario", "Fallo al registrar el usuario: " + textBox1.Text);
                    }
                }
                else
                {
                    inputLog("Registro de usuario", "Error registrando usuario: usuario ya registrado");
                    MessageBox.Show("El usuario ya está registrado:", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception excepcion)
            {
                MessageBox.Show("No se pudo acceder a la lista de usuarios: " + excepcion);
                Console.WriteLine("No se pudo acceder a la lista de usuarios");
            }
        }

        public string Hash(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            byte[] hashBytes;
            using (var algorithm = new System.Security.Cryptography.SHA512Managed())
            {
                hashBytes = algorithm.ComputeHash(bytes);
            }
            return Convert.ToBase64String(hashBytes);
        }
    }
}
