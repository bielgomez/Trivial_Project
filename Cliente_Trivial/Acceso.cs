using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Sockets;


namespace Trivial
{
    public partial class Acceso : Form
    {
        Socket server;
        int c = 0;


        public Acceso()
        {

            InitializeComponent();
            Bitmap img = new Bitmap(Application.StartupPath + @"\portada.png");
            this.BackgroundImage = img;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void Acceso_Load(object sender, EventArgs e)
        {
            consultaBox.Visible = false;
            candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");
            candadoBox.SizeMode = PictureBoxSizeMode.StretchImage;
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9020);


            //Creamos el socket 
            this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                luz.BackColor = Color.Green;
                conexion.Text = "Desconectar";
                c = 1;


            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }

        }


        //Botón de conexión/desconexión.
        private void conexion_Click(object sender, EventArgs e)
        {
            if (c == 0)
            {
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, 9020);


                //Creamos el socket 
                this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);//Intentamos conectar el socket
                    luz.BackColor = Color.Green;
                    conexion.Text = "Desconectar";
                    c = 1;



                }
                catch (SocketException ex)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("No he podido conectar con el servidor");
                    return;
                }
            }

            else
            {
                try
                {
                    string mensaje = "0/";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Desconexión del servidor
                    this.BackColor = Color.DarkSlateGray;
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    luz.BackColor = Color.DarkSlateGray;
                    conexion.Text = "Conectar";
                    consultaBox.Visible = false;
                    NameBox.Clear();
                    PasswordBox.Clear();
                    c = 0;
                }
                catch (Exception)
                {
                    MessageBox.Show("Ya estás desconectado.");
                }
            }

        }


        //Botón para acceder al usuario.
        private void Login_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = "1/" + NameBox.Text + "/" + PasswordBox.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


                if (mensaje == "0")
                {
                    consultaBox.Visible = true;

                }
                else if (mensaje == "1")
                    MessageBox.Show("Este usuario no existe");
                else if (mensaje == "2")
                    MessageBox.Show("Contraseña incorrecta");
                else
                    MessageBox.Show("Error de consulta. Pruebe otra vez.");
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: Compruebe que está conectado al servidor.");
            }

        }


        //Botón para registrarse.
        private void Registrarme_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = "2/" + userBox.Text + "/" + password2Box.Text + "/" + mailBox.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


                if (mensaje == "0")
                {
                    MessageBox.Show("Se ha registrado correctamente.");


                }

                else if (mensaje == "1")
                {
                    MessageBox.Show("Este nombre de usuario ya existe.");
                }

                else
                    MessageBox.Show("Error de consulta, pruebe otra vez.");

                userBox.Clear();
                password2Box.Clear();
                mailBox.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: Compruebe que está conectado al servidor.");
            }

        }


        //Botón para hacer consultas a la base de datos.
        private void Preguntar_Click(object sender, EventArgs e)
        {
            try
            {


                if (Contraseña.Checked)
                {
                    string mensaje = "3/" + NameBox.Text;
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
                        MessageBox.Show("La partida más larga ha sido la número " + mensaje + ".");

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
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: Compruebe que está conectado al servidor.");
            }
        }

       
        //Mostrar y ocultar las contraseñas.
        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            PasswordBox.UseSystemPasswordChar = true;
            candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");
        }

        private void candadoBox_Click(object sender, EventArgs e)
        {
            if (PasswordBox.UseSystemPasswordChar == true)
            {
                PasswordBox.UseSystemPasswordChar = false;
                candadoBox.Image = Image.FromFile(".\\candadoAbierto.jpg");

            }
            else
            {
                PasswordBox.UseSystemPasswordChar = true;
                candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");

            }
        }
    }
}
