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
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Cliente
{
    public partial class Control : Form
    {
        private Dictionary<string, string> sondas = new Dictionary<string, string>();
        private String usuario;
        private String validacion;
        private int numero_sondas = 0;


        public Control(String p_usuario, string p_validacion)
        {
            usuario = p_usuario;
            validacion = p_validacion;
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
                        sonda.getVolumen(Encrypt(usuario), validacion);
                        numero_sondas += 1;
                        textBox3.Text += "Sonda " + numero_sondas + "= " + textBox1.Text + "\r\n";
                        comboBox1.Items.Add("Sonda " + numero_sondas);
                        comboBox3.Items.Add("Sonda " + numero_sondas);
                        sondas.Add("Sonda " + numero_sondas.ToString(), sonda.Url.ToString());
                        inputLog("Añadir sonda", "Parámetros Sonda añadida: " + textBox1.Text);
                        textBox1.Text = "IP_SONDA_X:PUERTO_SONDA_X";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Los parámetros indicados no se corresponden con ninguna sonda activa: \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        inputLog("Añadir sonda", "Error en los parámetros Sonda: " + textBox1.Text);
                    }
                }
                else
                {
                    MessageBox.Show("La sonda especificada ya se encuentra registrada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    inputLog("Añadir sonda", "Sonda ya registrada: " + textBox1.Text);
                }
            }
            catch
            {
                MessageBox.Show("El formato de la llamada es IP:PUERTO", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                inputLog("Añadir sonda", "Formato de llamada incorrecto: " + textBox1.Text);
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
                switch (comboBox2.Text)
                {
                    //Volumen, Fecha actual, Ultima fecha registrada, Led
                    case "Volumen":
                        resultado = sonda.getVolumen(Encrypt(usuario), validacion);
                        inputLog("Consultar sensor", "Consulta del volumen en " + comboBox1.Text);
                        break;
                    case "Fecha actual":
                        resultado = sonda.getFechaActual(Encrypt(usuario), validacion);
                        inputLog("Consultar sensor", "Consulta de la fecha actual en " + comboBox1.Text);
                        break;
                    case "Ultima fecha registrada":
                        resultado = sonda.getUltimaFecha(Encrypt(usuario), validacion);
                        inputLog("Consultar sensor", "Consulta de la ultima fecha registrada en " + comboBox1.Text);
                        break;
                    case "Led":
                        resultado = sonda.getLed(Encrypt(usuario), validacion);
                        inputLog("Consultar sensor", "Consulta del valor LED en " + comboBox1.Text);
                        break;
                    default:
                        MessageBox.Show("El valor especificado no es un sensor de la Sonda", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        inputLog("Consultar sensor", "Error en la consulta del sensor de " + comboBox1.Text);
                        break;
                }
                try
                {
                    resultado = Decrypt(resultado);
                    richTextBox1.Text = resultado;
                    richTextBox2.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al desencriptar el valor recibido de la Sonda: \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (KeyNotFoundException keyError)
            {
                MessageBox.Show("La sonda indicada no está disponible en este momento: \n\n" + keyError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                inputLog("Consultar sensor", "Error al conectar con la sonda: " + comboBox1.Text);
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            Sonda.Sonda sonda = new Sonda.Sonda();
            try
            {
                sonda.Url = sondas[comboBox3.Text];
                try
                {
                    int valorNumericoLED = Int32.Parse(textBox2.Text);
                    if(valorNumericoLED >= 0 && valorNumericoLED <= 65535)
                    {
                        mensaje = Encrypt(textBox2.Text);
                        try
                        {
                            sonda.setLed(mensaje, Encrypt(usuario), validacion);
                            richTextBox2.Text = "El valor de la " + comboBox3.Text + " se ha actualizado correctamente al valor: " + textBox2.Text;
                            richTextBox1.Text = "";
                            inputLog("Modificar LED", "Modificación del sensor de la " + comboBox3.Text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Se ha producido un error intentando cambiar el valor del LED de la Sonda: \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            inputLog("Modificar LED", "Error en la modificación del sensor de la " + comboBox3.Text);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El valor del LED debe estar entre 0 y 65535", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        inputLog("Modificar LED", "Error en la modificación del sensor de la " + comboBox3.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("El valor del LED debe estar entre 0 y 65535: \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    inputLog("Modificar LED", "Error en la modificación del sensor de la " + comboBox3.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("La sonda indicada no está disponible en este momento: \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                inputLog("Modificar LED", "Error intentando conectar con la " + comboBox3.Text);
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
            if (descripcion != "")
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

        /**********************************************/
        /**********************CIFRADO*****************/
        /**********************************************/

        public string readKey()
        {
            string key = "";
            try
            {
                System.IO.StreamReader fichero = new System.IO.StreamReader(@".\clave.txt");
                key = fichero.ReadLine();
                fichero.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error al leer el fichero donde está almacenada la clave: \n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return key;
        }

        public RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
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

        public String Encrypt(String plainText)
        {
            String key = readKey();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(key)));
        }

        public String Decrypt(String encryptedText)
        {
            String key = readKey();
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(key)));
        }
    }
}
