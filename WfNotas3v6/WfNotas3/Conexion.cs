using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WfNotas3
{
    public partial class Form1 : Form
    {
        SqlConnection conexion;
        DataSet dataset1;

        private void ConexionDb()
        {
            conexion = new SqlConnection();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Usuario-PC\\SQLEXPRESS";
            builder.InitialCatalog = "Instituto";
            builder.IntegratedSecurity = true;
            conexion.ConnectionString = builder.ConnectionString;
        }

        private bool abrirConexion()
        {
            bool error = true;
            try
            {
                conexion.Open();
            }
            catch (SqlException)
            {
                error = false;
                //MessageBox.Show(sqlex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return error;           //devuelve bool para comprobar conexion
        }

        private void cerrarConexion()
        {
            conexion.Close();
        }
    }
}