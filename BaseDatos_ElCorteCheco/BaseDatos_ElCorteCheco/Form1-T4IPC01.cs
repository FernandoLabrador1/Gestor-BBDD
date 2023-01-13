using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Image = System.Drawing.Image;

namespace BaseDatos_ElCorteCheco
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        /*
         * Método para el mensaje de cerrar el programa.
         */
        public void dialogoSalir()
        {
            DialogResult result = MessageBox.Show("¿Desea cerrar del programa?", "Salír", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }


        /*
         * Métodos para que solo se puedan introducir números en el identificador y en el precio junto con sus decimales.
         */
        private void textElec1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) | (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se pueden usar números para el Identificador.", "Valor incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void textElec4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 43) | (e.KeyChar >= 58 && e.KeyChar <= 255) | e.KeyChar == 45 | e.KeyChar == 47)
            {
                MessageBox.Show("Solo se pueden usar números y las separaciones para decimales (',' y '.').", "Valor incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void textJuego1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) | (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se pueden usar números para el Identificador.", "Valor incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void textJuego4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 43) | (e.KeyChar >= 58 && e.KeyChar <= 255) | e.KeyChar == 45 | e.KeyChar == 47)
            {
                MessageBox.Show("Solo se pueden usar números y las separaciones para decimales (',' y '.').", "Valor incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void textModa1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) | (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se pueden usar números para el Identificador.", "Valor incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void textModa4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 43) | (e.KeyChar >= 58 && e.KeyChar <= 255) | e.KeyChar == 45 | e.KeyChar == 47)
            {
                MessageBox.Show("Solo se pueden usar números y las separaciones para decimales (',' y '.').", "Valor incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }


        /*
         * Métodos booleano para comprobar que no se repitan los ID's.
         */
        public Boolean comprobarIdElectronica()
        {
            SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
            conexion.Open();

            SqlCommand comando = new SqlCommand("select Identificador from Electronica where Identificador='" + textElec1.Text + "'",conexion);

            SqlDataReader leer = comando.ExecuteReader();

            Boolean repetidos = leer.Read();

            return repetidos;
        }

        public Boolean comprobarIdJuegos()
        {
            SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
            conexion.Open();

            SqlCommand comando = new SqlCommand("select Identificador from Juegos where Identificador='" + textJuego1.Text + "'", conexion);

            SqlDataReader leer = comando.ExecuteReader();

            Boolean repetidos = leer.Read();

            return repetidos;
        }

        public Boolean comprobarIdModa()
        {

            SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
            conexion.Open();

            SqlCommand comando = new SqlCommand("select Identificador from Moda where Identificador='" + textModa1.Text + "'", conexion);

            SqlDataReader leer = comando.ExecuteReader();

            Boolean repetidos = leer.Read();

            return repetidos;
        }


        /*
         * Departamente de Electrónica.
         */
        private void consultarElec_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
            conexion.Open();

            String Id = textElec1.Text;
            String seleccionar = "Select Tipodedispositivo,Marca,Precio from Electronica where Identificador = '" + Id + "'";

            SqlCommand comando = new SqlCommand(seleccionar, conexion);
            SqlDataReader registros = comando.ExecuteReader();

            if (registros.Read())
            {

                textElec2.Text = registros["Tipodedispositivo"].ToString();
                textElec3.Text = registros["Marca"].ToString();
                textElec4.Text = registros["Precio"].ToString();

                SqlConnection conexion2 = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");

                string cadenaImagen = "select Imagen from Electronica where Identificador='" + Id + "'";

                SqlCommand comandoImagen = new SqlCommand(cadenaImagen, conexion2);
                conexion2.Open();

                SqlDataAdapter da = new SqlDataAdapter(comandoImagen);
                DataSet ds = new DataSet();

                da.Fill(ds, "Electronica");
                int c = ds.Tables["Electronica"].Rows.Count;

                if (!Convert.IsDBNull(ds.Tables["Electronica"].Rows[c - 1]["Imagen"]))
                {
                    //se evalua si se ha obtenido imagen.
                    Byte[] byteImagen = new Byte[0];
                    byteImagen = (Byte[])(ds.Tables["Electronica"].Rows[c - 1]["Imagen"]);
                    MemoryStream stmBLOBData = new MemoryStream(byteImagen);
                    pictureElec.Image = Image.FromStream(stmBLOBData);

                }

                else
                {
                    pictureElec.Image = null;
                }

                conexion2.Close();

            }

            else
            {
                MessageBox.Show("No existe un Artículo con el identificador introducido.", "Artículo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            conexion.Close();
        }

        private void insertarElec_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
            conexion.Open();

            String Id = textElec1.Text;
            String Dispositivo = textElec2.Text;
            String Marca = textElec3.Text;
            String Precio = textElec4.Text.Replace(',', '.');
            String insertar = "insert into Electronica(Identificador,Tipodedispositivo,Marca,Precio) values ('" + Id + "', '" + Dispositivo + "', '" + Marca + "', " + Precio + ")";

            if (Id.Equals("") | Dispositivo.Equals("") || Marca.Equals("") || Precio.Equals(""))
            {
                MessageBox.Show("Todos los campos (excepto imagen) deben rellenarse obligatoriamente.", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            
            else if (!comprobarIdElectronica())
            {

                SqlCommand comando = new SqlCommand(insertar, conexion);
                comando.ExecuteNonQuery();

                MessageBox.Show("Los datos se guardaron correctamente.", "Datos almacenados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textElec1.Text = "";
                textElec2.Text = "";
                textElec3.Text = "";
                textElec4.Text = "";

                if (pictureElec.Image != null)
                {

                    SqlCommand comandoMeterImagen = new SqlCommand();

                    comandoMeterImagen.Connection = conexion;
                    comandoMeterImagen.CommandText = "Update Electronica Set Imagen=@Foto where Identificador='" + Id + "'";
                    comandoMeterImagen.Parameters.Add("@Foto", System.Data.SqlDbType.Image);

                    //Imagen
                    MemoryStream stream = new MemoryStream();
                    pictureElec.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                    comandoMeterImagen.Parameters["@Foto"].Value = stream.GetBuffer();
                    comandoMeterImagen.ExecuteNonQuery();
                    conexion.Close();
                }

                else
                {
                    MessageBox.Show("No ha elegido la imagen.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                pictureElec.Image = null;

                conexion.Close();
            }

            else
            {
                MessageBox.Show("El Identificador introducido ya está en uso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conexion.Close();

        }

        private void modificarElec_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Está apunto de modificar un registro ¿Desea continuar?", "Modificar Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {

                //Variables
                int cant;

                SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
                conexion.Open();

                String Id = textElec1.Text;
                String Dispositivo = textElec2.Text;
                String Marca = textElec3.Text;
                String Precio = textElec4.Text.Replace(',', '.');
                String Modificar = "Update ELectronica set Tipodedispositivo= '" + Dispositivo + "', Marca= '" + Marca + "', Precio= " + Precio + ",Imagen= null where Identificador= '" + Id + "'";

                if (Id.Equals(""))
                {
                    MessageBox.Show("El Identificador no puede estár vacío", "Identificador Vacío", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conexion.Close();
                }

                else
                {
                    try
                    {
                        SqlCommand comando = new SqlCommand(Modificar, conexion);
                        cant = comando.ExecuteNonQuery();

                        if (cant == 1)
                        {

                            MessageBox.Show("Se modificaron correctamente los datos del Artículo.", "Datos modificados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textElec1.Text = "";
                            textElec2.Text = "";
                            textElec3.Text = "";
                            textElec4.Text = "";

                            if (pictureElec.Image != null)
                            {

                                SqlCommand comandoMeterImagen = new SqlCommand();

                                comandoMeterImagen.Connection = conexion;
                                comandoMeterImagen.CommandText = "Update Electronica Set Imagen=@Foto where Identificador='" + Id + "'";
                                comandoMeterImagen.Parameters.Add("@Foto", System.Data.SqlDbType.Image);

                                //Imagen
                                MemoryStream stream = new MemoryStream();
                                pictureElec.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                                comandoMeterImagen.Parameters["@Foto"].Value = stream.GetBuffer();
                                comandoMeterImagen.ExecuteNonQuery();
                                conexion.Close();
                            }

                            else
                            {
                                MessageBox.Show("No ha elegido la imagen.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            }

                            pictureElec.Image = null;
                        }
                        conexion.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("No existe un Artículo con el identificador introducido", "Artículo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conexion.Close();
                    }

                }
                
            }

        }

        private void eliminarElec_Click(object sender, EventArgs e)
        {


            DialogResult result = MessageBox.Show("Está apunto de eliminar un registro ¿Desea continuar?", "Eliminar Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {


                //Variables
                int cant;

                SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
                conexion.Open();

                String Id = textElec1.Text;
                String borrar = "delete from Electronica where Identificador = '" + Id + "'";

                SqlCommand comando = new SqlCommand(borrar, conexion);
                cant = comando.ExecuteNonQuery();

                if (cant == 1)
                {

                    MessageBox.Show("Se borró correctamente el Artículo de la base de datos.", "Artículo eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textElec1.Text = "";
                    textElec2.Text = "";
                    textElec3.Text = "";
                    textElec4.Text = "";
                    pictureElec.Image = null;

                }

                else
                {
                    MessageBox.Show("No existe un Artículo con el identificador introducido.", "Artículo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                conexion.Close();
            }
        }


        /*
         * Departamente de Juegos.
         */
        private void consultarJuego_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
            conexion.Open();

            String Id = textJuego1.Text;
            String seleccionar = "Select Nombre,Plataforma,Precio from Juegos where Identificador = '" + Id + "'";

            SqlCommand comando = new SqlCommand(seleccionar, conexion);
            SqlDataReader registros = comando.ExecuteReader();

            if (registros.Read())
            {

                textJuego2.Text = registros["Nombre"].ToString();
                textJuego3.Text = registros["Plataforma"].ToString();
                textJuego4.Text = registros["Precio"].ToString();

                SqlConnection conexion2 = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");

                string cadenaImagen = "select Imagen from Juegos where Identificador='" + Id + "'";

                SqlCommand comandoImagen = new SqlCommand(cadenaImagen, conexion2);
                conexion2.Open();

                SqlDataAdapter da = new SqlDataAdapter(comandoImagen);
                DataSet ds = new DataSet();

                da.Fill(ds, "Juegos");
                int c = ds.Tables["Juegos"].Rows.Count;

                if (!Convert.IsDBNull(ds.Tables["Juegos"].Rows[c - 1]["Imagen"]))
                {
                    //se evalua si se ha obtenido imagen.
                    Byte[] byteImagen = new Byte[0];
                    byteImagen = (Byte[])(ds.Tables["Juegos"].Rows[c - 1]["Imagen"]);
                    MemoryStream stmBLOBData = new MemoryStream(byteImagen);
                    pictureJuego.Image = Image.FromStream(stmBLOBData);

                }

                else
                {
                    pictureJuego.Image = null;
                }

                conexion2.Close();
            }

            else
            {
                MessageBox.Show("No existe un Artículo con el identificador introducido.", "Artículo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            conexion.Close();
        }

        private void insertarJuego_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
            conexion.Open();

            String Id = textJuego1.Text;
            String Nombre = textJuego2.Text;
            String Plataforma = textJuego3.Text;
            String Precio = textJuego4.Text.Replace(',', '.');
            String insertar = "insert into Juegos(Identificador,Nombre,Plataforma,Precio) values ('" + Id + "', '" + Nombre + "', '" + Plataforma + "', " + Precio + ")";

            if (Id.Equals("") | Nombre.Equals("") || Plataforma.Equals("") || Precio.Equals(""))
            {
                MessageBox.Show("Todos los campos (excepto imagen) deben rellenarse obligatoriamente.", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            else if (!comprobarIdJuegos()) 
            {
                SqlCommand comando = new SqlCommand(insertar, conexion);
                comando.ExecuteNonQuery();

                MessageBox.Show("Los datos se guardaron correctamente.", "Datos almacenados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textJuego1.Text = "";
                textJuego2.Text = "";
                textJuego3.Text = "";
                textJuego4.Text = "";

                if (pictureJuego.Image != null)
                {

                    SqlCommand comandoMeterImagen = new SqlCommand();

                    comandoMeterImagen.Connection = conexion;
                    comandoMeterImagen.CommandText = "Update Juegos Set Imagen=@Foto where Identificador='" + Id + "'";
                    comandoMeterImagen.Parameters.Add("@Foto", System.Data.SqlDbType.Image);

                    //Imagen
                    MemoryStream stream = new MemoryStream();
                    pictureJuego.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                    comandoMeterImagen.Parameters["@Foto"].Value = stream.GetBuffer();
                    comandoMeterImagen.ExecuteNonQuery();
                    conexion.Close();
                }

                else
                {
                    MessageBox.Show("No ha elegido la imagen.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                pictureJuego.Image = null;

                conexion.Close();
            }

            else
            {
                MessageBox.Show("El Identificador introducido ya está en uso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conexion.Close();

        }

        private void modificarJuego_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Está apunto de modificar un registro ¿Desea continuar?", "Modificar Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {

                //Variables
                int cant;

                SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
                conexion.Open();

                String Id = textJuego1.Text;
                String Nombre = textJuego2.Text;
                String Plataforma = textJuego3.Text;
                String Precio = textJuego4.Text.Replace(',', '.');
                String Modificar = "Update Juegos set Nombre= '" + Nombre + "', Plataforma= '" + Plataforma + "', Precio= " + Precio + ",Imagen= null where Identificador= '" + Id + "'";

                if (Id.Equals(""))
                {
                    MessageBox.Show("El Identificador no puede estár vacío.", "Identificador Vacío", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conexion.Close();
                }

                else
                {
                    try
                    {
                        SqlCommand comando = new SqlCommand(Modificar, conexion);
                        cant = comando.ExecuteNonQuery();


                        if (cant == 1)
                        {

                            MessageBox.Show("Se modificaron correctamente los datos del Artículo.", "Datos modificados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textJuego1.Text = "";
                            textJuego2.Text = "";
                            textJuego3.Text = "";
                            textJuego4.Text = "";

                            if (pictureJuego.Image != null)
                            {

                                SqlCommand comandoMeterImagen = new SqlCommand();

                                comandoMeterImagen.Connection = conexion;
                                comandoMeterImagen.CommandText = "Update Juegos Set Imagen=@Foto where Identificador='" + Id + "'";
                                comandoMeterImagen.Parameters.Add("@Foto", System.Data.SqlDbType.Image);

                                //Imagen
                                MemoryStream stream = new MemoryStream();
                                pictureJuego.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                                comandoMeterImagen.Parameters["@Foto"].Value = stream.GetBuffer();
                                comandoMeterImagen.ExecuteNonQuery();
                                conexion.Close();
                            }

                            else
                            {
                                MessageBox.Show("No ha elegido la imagen.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            }

                            pictureJuego.Image = null;
                        }
                        conexion.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("No existe un Artículo con el identificador introducido.", "Artículo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conexion.Close();
                    }
                }
                    
            }
        }

        private void eliminarJuego_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Está apunto de eliminar un registro ¿Desea continuar?", "Eliminar Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {

                //Variables
                int cant;

                SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
                conexion.Open();

                String Id = textJuego1.Text;
                String borrar = "delete from juegos where Identificador = '" + Id + "'";

                SqlCommand comando = new SqlCommand(borrar, conexion);
                cant = comando.ExecuteNonQuery();

                if (cant == 1)
                {

                    MessageBox.Show("Se borró correctamente el Artículo de la base de datos.", "Artículo eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textJuego1.Text = "";
                    textJuego2.Text = "";
                    textJuego3.Text = "";
                    textJuego4.Text = "";
                    pictureJuego.Image = null;

                }

                else
                {
                    MessageBox.Show("No existe un Artículo con el identificador introducido.", "Artículo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                conexion.Close();
            }
        }


        /*
         * Departamente de Moda.
         */
        private void consultarModa_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
            conexion.Open();

            String Id = textModa1.Text;
            String seleccionar = "Select Tipodeprenda,Talla,Precio from Moda where Identificador = '" + Id + "'";

            SqlCommand comando = new SqlCommand(seleccionar, conexion);
            SqlDataReader registros = comando.ExecuteReader();

            if (registros.Read())
            {

                textModa2.Text = registros["Tipodeprenda"].ToString();
                comboModa3.Text = registros["Talla"].ToString();
                textModa4.Text = registros["Precio"].ToString();

                SqlConnection conexion2 = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");

                string cadenaImagen = "select Imagen from Moda where Identificador='" + Id + "'";

                SqlCommand comandoImagen = new SqlCommand(cadenaImagen, conexion2);
                conexion2.Open();

                SqlDataAdapter da = new SqlDataAdapter(comandoImagen);
                DataSet ds = new DataSet();

                da.Fill(ds, "Moda");
                int c = ds.Tables["Moda"].Rows.Count;

                if (!Convert.IsDBNull(ds.Tables["Moda"].Rows[c - 1]["Imagen"]))
                {
                    //se evalua si se ha obtenido imagen.
                    Byte[] byteImagen = new Byte[0];
                    byteImagen = (Byte[])(ds.Tables["Moda"].Rows[c - 1]["Imagen"]);
                    MemoryStream stmBLOBData = new MemoryStream(byteImagen);
                    pictureModa.Image = Image.FromStream(stmBLOBData);

                }

                else
                {
                    pictureModa.Image = null;
                }

                conexion2.Close();
            }

            else
            {
                MessageBox.Show("No existe un Artículo con el identificador introducido.", "Artículo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            conexion.Close();
        }

        private void insertarModa_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
            conexion.Open();

            String Id = textModa1.Text;
            String Tipodeprenda = textModa2.Text;
            String Talla = comboModa3.Text;
            String Precio = textModa4.Text.Replace(',', '.');
            String insertar = "insert into Moda(Identificador,Tipodeprenda,Talla,Precio) values ('" + Id + "', '" + Tipodeprenda + "', '" + Talla + "', " + Precio + ")";

            SqlCommand comando = new SqlCommand(insertar, conexion);

            if (Id.Equals("") | Precio.Equals("") || Talla.Equals("") || Precio.Equals(""))
            {
                MessageBox.Show("Todos los campos (excepto imagen) deben rellenarse obligatoriamente.", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            else if (!comprobarIdModa())
            {
                comando.ExecuteNonQuery();

                MessageBox.Show("Los datos se guardaron correctamente.", "Datos almacenados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textModa1.Text = "";
                textModa2.Text = "";
                comboModa3.Text = "";
                textModa4.Text = "";

                if (pictureModa.Image != null)
                {

                    SqlCommand comandoMeterImagen = new SqlCommand();

                    comandoMeterImagen.Connection = conexion;
                    comandoMeterImagen.CommandText = "Update Moda Set Imagen=@Foto where Identificador='" + Id + "'";
                    comandoMeterImagen.Parameters.Add("@Foto", System.Data.SqlDbType.Image);

                    //Imagen
                    MemoryStream stream = new MemoryStream();
                    pictureModa.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                    comandoMeterImagen.Parameters["@Foto"].Value = stream.GetBuffer();
                    comandoMeterImagen.ExecuteNonQuery();
                    conexion.Close();
                }

                else
                {
                    MessageBox.Show("No ha elegido la imagen.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                pictureModa.Image = null;

                conexion.Close();
            }

            else
            {
                MessageBox.Show("El Identificador introducido ya está en uso.", "Identificador duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conexion.Close();

        }

        private void modificarModa_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Está apunto de modificar un registro ¿Desea continuar?", "Modificar Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {


                //Variables
                int cant;

                SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
                conexion.Open();

                String Id = textModa1.Text;
                String Tipodeprenda = textModa2.Text;
                String Talla = comboModa3.Text;
                String Precio = textModa4.Text.Replace(',', '.');
                String Modificar = "Update Moda set Tipodeprenda= '" + Tipodeprenda + "', Talla= '" + Talla + "', Precio= " + Precio + ",Imagen= null where Identificador= '" + Id + "'";

                if (Id.Equals(""))
                {
                    MessageBox.Show("El Identificador no puede estár vacío.", "Identificador Vacío", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conexion.Close();
                }

                else
                {
                    try
                    {
                        SqlCommand comando = new SqlCommand(Modificar, conexion);
                        cant = comando.ExecuteNonQuery();

                        if (cant == 1)
                        {

                            MessageBox.Show("Se modificaron correctamente los datos del Artículo.", "Datos modificados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textModa1.Text = "";
                            textModa2.Text = "";
                            comboModa3.Text = "";
                            textModa4.Text = "";

                            if (pictureModa.Image != null)
                            {

                                SqlCommand comandoMeterImagen = new SqlCommand();

                                comandoMeterImagen.Connection = conexion;
                                comandoMeterImagen.CommandText = "Update Moda Set Imagen=@Foto where Identificador='" + Id + "'";
                                comandoMeterImagen.Parameters.Add("@Foto", System.Data.SqlDbType.Image);

                                //Imagen
                                MemoryStream stream = new MemoryStream();
                                pictureModa.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                                comandoMeterImagen.Parameters["@Foto"].Value = stream.GetBuffer();
                                comandoMeterImagen.ExecuteNonQuery();
                                conexion.Close();
                            }

                            else
                            {
                                MessageBox.Show("No ha elegido la imagen.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            }

                            pictureModa.Image = null;
                        }
                        conexion.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("No existe un Artículo con el identificador introducido.", "Artículo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conexion.Close();
                    }


                    
                }
            }
        }

        private void eliminarModa_Click(object sender, EventArgs e)
        {


            DialogResult result = MessageBox.Show("Está apunto de eliminar un registro ¿Desea continuar?", "Eliminar Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {


                //Variables
                int cant;

                SqlConnection conexion = new SqlConnection("server = T4IPC01\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
                conexion.Open();

                String Id = textModa1.Text;
                String borrar = "delete from Moda where Identificador = '" + Id + "'";

                SqlCommand comando = new SqlCommand(borrar, conexion);
                cant = comando.ExecuteNonQuery();

                if (cant == 1)
                {

                    MessageBox.Show("Se borró correctamente el Artículo de la base de datos.", "Artículo eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textModa1.Text = "";
                    textModa2.Text = "";
                    comboModa3.Text = "";
                    textModa4.Text = "";
                    pictureModa.Image = null;

                }

                else
                {
                    MessageBox.Show("No existe un Artículo con el identificador introducido.", "Artículo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                conexion.Close();
            }
        }

        /*
         * Botones para Subir y mostrar las imágenes en la base de datos.
         */
        private void subirElec_Click(object sender, EventArgs e)
        {
            // openFileDialog1.Filter = "Imagenes JPG|.JPG|Imagenes GIF|.gif|Imagenes PNG|.png|.Imagenes Bitmaps|.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureElec.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void quitarElec_Click(object sender, EventArgs e)
        {
            pictureElec.Image = null;
        }


        private void subirJuego_Click(object sender, EventArgs e)
        {
            // openFileDialog1.Filter = "Imagenes JPG|.JPG|Imagenes GIF|.gif|Imagenes PNG|.png|.Imagenes Bitmaps|.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureJuego.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void quitarJuego_Click(object sender, EventArgs e)
        {
            pictureJuego.Image = null;
        }


        private void subirModa_Click(object sender, EventArgs e)
        {
            // openFileDialog1.Filter = "Imagenes JPG|.JPG|Imagenes GIF|.gif|Imagenes PNG|.png|.Imagenes Bitmaps|.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureModa.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void quitarModa_Click(object sender, EventArgs e)
        {
            pictureModa.Image = null;
        }


        /*
         * Botones para limpiar registros.
         */
        private void buttonLimpiarElec_Click(object sender, EventArgs e)
        {
            textElec1.Text = "";
            textElec2.Text = "";
            textElec3.Text = "";
            textElec4.Text = "";
            pictureElec.Image = null;
        }

        private void buttonLimpiarJuegos_Click(object sender, EventArgs e)
        {
            textJuego1.Text = "";
            textJuego2.Text = "";
            textJuego3.Text = "";
            textJuego4.Text = "";
            pictureJuego.Image = null;
        }

        private void buttonLimpiarModa_Click(object sender, EventArgs e)
        {
            textModa1.Text = "";
            textModa2.Text = "";
            comboModa3.Text = "";
            textModa4.Text = "";
            pictureModa.Image = null;
        }


        /*
         * Botones para salir.
         */
        private void salirElec_Click(object sender, EventArgs e)
        {
            dialogoSalir();
        }

        private void salirJuego_Click(object sender, EventArgs e)
        {
            dialogoSalir();
        }

        private void salirModa_Click(object sender, EventArgs e)
        {
            dialogoSalir();
        }


        /*
         * Botones para el listado.
         */
        private void buttonListaElec_Click(object sender, EventArgs e)
        {
            Form2 listado = new Form2();
            listado.Show();
        }

        private void buttonListajuegos_Click(object sender, EventArgs e)
        {
            Form3 listado = new Form3();
            listado.Show();
        }

        private void buttonListaModa_Click(object sender, EventArgs e)
        {
            Form4 listado = new Form4();
            listado.Show();
        }


        /*
         * Método y funcionalidades para poder arrastrar la venta.
         */
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelPrincipal_MouseDown(object sender, MouseEventArgs e)
        {

            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        /*
         * Métodos para cerrar y minimizar la venta.
         */
        private void buttonMinim_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
