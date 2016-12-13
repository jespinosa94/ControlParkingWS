using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Cliente
{
    public partial class Control : Form
    {
        private ArrayList sondas = new ArrayList();
        private String usuario;
        private String ip_solicitada = "";
        private String puerto_solicitado = "";

        public Control(String p_usuario)
        {
            usuario = p_usuario;
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String ip_peticion = "";
            String puerto_peticion = "";
            String idSonda = "-1";

            try
            {
                String[] peticion = textBox1.Text.Split(':');
                ip_peticion = peticion[0];
                puerto_peticion = peticion[1];
                try
                {
                    Sonda.Sonda sonda = new Sonda.Sonda(2);
                    sonda.Url = "http://" + ip_peticion + "/Sonda/services/Sonda.SondaHttpSoap11Endpoint/";
                    idSonda = sonda.GetId();
                    sondas.Add(idSonda);
                    textBox3.Text += textBox1.Text + "\r\n";
                    comboBox1.Items.Add(textBox1.Text);
                    textBox1.Text = "IP_SONDA_X PUERTO_SONDA_X";
                }
                catch
                {
                    MessageBox.Show("No se encuentra la sonda especificada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("Formato de la llamada IP:PUERTO", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
