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
        private int numero_sondas = 0;

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
                if (!textBox3.Text.Contains(textBox1.Text))
                {
                    try
                    {
                        Sonda.Sonda sonda = new Sonda.Sonda();
                        sonda.Timeout = 2000;

                        sonda.Url = "http://" + textBox1.Text + "/Sonda/services/Sonda.SondaHttpSoap11Endpoint/";
                        sonda.getVolumen();
                        numero_sondas += 1;    
                        textBox3.Text += "Sonda " + numero_sondas + "= " + textBox1.Text + "\r\n";
                        comboBox1.Items.Add("Sonda " + numero_sondas);
                        textBox1.Text = "IP_SONDA_X:PUERTO_SONDA_X";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Los parámetros indicados no se corresponden con ninguna sonda activa: \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("La sonda especificada ya se encuentra registrada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("El formato de la llamada es IP:PUERTO", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
