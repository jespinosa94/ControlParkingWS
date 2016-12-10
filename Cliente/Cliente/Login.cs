using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string linea;
            string[] campos; //usuario y contraseña
            string[] separador = new string[] { "::" };
            try
            {
                System.IO.StreamReader fichero = new System.IO.StreamReader(@".\usuarios.txt");
                while ((linea = fichero.ReadLine()) != null)
                {
                    campos = linea.Split(separador, StringSplitOptions.None);
                    if (textBox1.Text == campos[0] && textBox2.Text == campos[1])
                    {
                        Control control = new Control(textBox1.Text);
                        this.Visible = false;
                        control.Show();
                    }
                }
                textBox1.Text = "";
                textBox2.Text = "";
                label3.Visible = true;
                fichero.Close();
            }
            catch(Exception excepcion)
            {
                MessageBox.Show("No se pudo acceder a la lista de usuarios: " + excepcion);
                Console.WriteLine("No se pudo acceder a la lista de usuarios");
            }
            
        }
    }
}
