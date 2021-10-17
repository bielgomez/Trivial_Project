using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Sockets;

namespace Trivial
{
    public partial class Registro : Form
    {
        Socket server;
        public Registro()
        {
            InitializeComponent();
            Bitmap img = new Bitmap(Application.StartupPath + @"\fondonegro.png");
            this.BackgroundImage = img;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
       
        private void Registro_Load(object sender, EventArgs e)
        {

        }

        private void Registrar_Click(object sender, EventArgs e)
        {
            //Datos del servidor
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9080);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                string mensaje = "2/" + userBox.Text+ "/"+ passwordBox.Text + "/" + mailBox.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                //Procesamos la respuesta
                if (mensaje == "0")
                {
                    MessageBox.Show("Se ha registrado correctamente.");
                    mensaje = "0/";
                    msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    this.Close();
                }

                else if (mensaje=="1")
                {
                    MessageBox.Show("Este nombre de usuario ya existe.");
                }

                else
                    MessageBox.Show("Error de consulta, pruebe otra vez.");

                //Desconectamos del servidor
                server.Shutdown(SocketShutdown.Both);
                server.Close();

            }
            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }
        }
    }
}
