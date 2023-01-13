using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDatos_ElCorteCheco
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = PC-FERNANDO\\SQLEXPRESS ; database = elcortecheco ; integrated security = true");
            conexion.Open();

            String seleccionar = "Select Identificador,Nombre,Plataforma,Precio from Juegos";
            SqlDataAdapter adapter = new SqlDataAdapter(seleccionar, conexion);

            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataJuego1.DataSource = dt;
        }
    }
}
