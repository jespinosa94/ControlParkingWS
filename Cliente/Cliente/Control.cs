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
using System.Security.Cryptography;

namespace Cliente
{
    public partial class Control : Form
    {
        private Dictionary <string, string> sondas = new Dictionary<string, string>();
        private String usuario;
        private String ip_solicitada = "";
        private String puerto_solicitado = "";
        private int numero_sondas = 0;
        private string key = "OFPcPBjssp4l1dXKcl17QA==";

        public Control(String p_usuario)
        {
            usuario = p_usuario;
            InitializeComponent();
            AcceptButton = button1;
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
                        sondas.Add("Sonda " + numero_sondas.ToString(), sonda.Url.ToString());
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

        private void button2_Click(object sender, EventArgs e)
        {
            string resultado = "";
            Sonda.Sonda sonda = new Sonda.Sonda();

            try
            {
                sonda.Url = sondas[comboBox1.Text];
                switch(comboBox2.Text)
                {
                    //Volumen, Fecha actual, Ultima fecha registrada, Led
                    case "Volumen":
                        resultado = sonda.getVolumen();
                        break;
                    case "Fecha actual":
                        resultado = sonda.getFechaActual();
                        break;
                    default:
                        MessageBox.Show("El valor especificado no es un sensor de la Sonda", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
                try
                {
                    resultado = Decrypt(resultado, key);
                    //richTextBox1.Text = resultado;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error al desencriptar el valor recibido de la Sonda: \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //d
                richTextBox1.Text = resultado;
            }
            catch (KeyNotFoundException keyError)
            {
                MessageBox.Show("La sonda indicada no está disponible en este momento: \n\n" + keyError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /**********************************************/
        /**********************CIFRADO*****************/
        /**********************************************/

        /*
         * Sobrecarga de la clase RijndaelManaged
         * */
        public RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        public byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        public String Encrypt(String plainText, String key)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(key)));
        }

        public String Decrypt(String encryptedText, String key)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(key)));
        }

    }
}
