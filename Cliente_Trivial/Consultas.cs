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
    public partial class Consultas : Form
    {
        string username;
        Socket server;

        public void setUsername(string username)
        {
            this.username = username;
        }

        public Consultas()
        {
            InitializeComponent();
            Bitmap img = new Bitmap(Application.StartupPath + @"\fondonegro.png");
            this.BackgroundImage = img;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void Consultas_Load(object sender, EventArgs e)
        {
            
        }

        private void Registrar_Click(object sender, EventArgs e)

        {
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9080);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket


                if (Contraseña.Checked)
                {
                    string mensaje = "3/" + username;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                    if (mensaje == "-1")
                        MessageBox.Show("Error de consulta. Prueba otra vez.");
                    else
                        MessageBox.Show("Tu contraseña es: " + mensaje);

                }


                else if (duracion.Checked)
                {
                    string mensaje = "4/";
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


                    if (mensaje == "-1")
                        MessageBox.Show("Error de consulta. Prueba otra vez.");
                    else if (mensaje == "-2")
                        MessageBox.Show("No se ha encontrado ninguna partida en la base de datos");
                    else
                        MessageBox.Show("La partida más larga ha durado: " + mensaje + " segundos.");

                }
                else
                {
                    string mensaje = "5/";
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                    if (mensaje == "-1")
                        MessageBox.Show("Error de consulta. Prueba otra vez");
                    else if (mensaje == "-2")
                        MessageBox.Show("No se ha encontrado ningún jugador en la base de datos.");
                    else
                        MessageBox.Show("El jugador con más puntos es: " + mensaje + ".");
                }

                //Desconectamos del servidor
                server.Shutdown(SocketShutdown.Both);
                server.Close();

            }

            catch(SocketException)
            {
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }

        }
    }
}
