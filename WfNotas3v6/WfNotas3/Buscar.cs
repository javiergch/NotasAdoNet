using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WfNotas3
{
    public partial class Form1 : Form
    {
        private void tPbuscar_Enter(object sender, EventArgs e)
        {
            if (tBnombre.Text != "" || tBapellidos.Text != "")
            {
                lInformacion.Visible = false;
                tBnombre.Text = "";
                tBapellidos.Text = "";
                dGbuscarAlumnos.Enabled = true;
            }
            vistaBuscarAlumnos.RowFilter = "COD_ALU = '" + dGbuscar.Rows[0].Cells["COD_ALU"].Value.ToString() + "'";
            dGbuscarAlumnos.Columns[0].Visible = false; // cod_cur
        }

        private void cargarBuscar()
        {
            sqlBuscar = new SqlCommand();
            daBuscar = new SqlDataAdapter();


            sqlBuscar.CommandText = @"select alu.COD_ALU, alu.COD_CUR, alu.DNI, alu.APELLIDOS, alu.NOMBRE
                                      from alumnos as alu
                                      where alu.NOMBRE like '%" + tBnombre.Text + "%' COLLATE Modern_Spanish_CI_AI and alu.APELLIDOS like '%" + tBapellidos.Text + "%' COLLATE Modern_Spanish_CI_AI";
            sqlBuscar.Connection = conexion;

            daBuscar.SelectCommand = sqlBuscar;
            daBuscar.Fill(dataset1, "Buscar");
            

            vistaBuscar = new DataView(dataset1.Tables["Buscar"]);
            dGbuscar.DataSource = vistaBuscar;


            vistaBuscarAlumnos = new DataView(dataset1.Tables["Notas"]);
            dGbuscarAlumnos.DataSource = vistaBuscarAlumnos;

            dGbuscarAlumnos.Columns[0].Visible = false; // cod_cur
            dGbuscarAlumnos.Columns[1].Visible = false; // descripcion curso
            dGbuscarAlumnos.Columns[2].Visible = false; // cod_asi

            dGbuscarAlumnos.Columns[3].Visible = true;  // descripcion asignaturas
            dGbuscarAlumnos.Columns[3].ReadOnly = true;
            dGbuscarAlumnos.Columns[3].HeaderText = "DESCRIPCION";

            dGbuscarAlumnos.Columns[4].Visible = true;  // cod_alu
            dGbuscarAlumnos.Columns[4].ReadOnly = true;

            dGbuscarAlumnos.Columns[5].Visible = false; // dni
            dGbuscarAlumnos.Columns[6].Visible = false; // apellidos
            dGbuscarAlumnos.Columns[7].Visible = false; // nombre
            
        }

        private void tBnombre_TextChanged(object sender, EventArgs e)
        {
            if (chBbuscar.Checked == true)
            {
                // usar consulta
                sqlBuscar.CommandText = @"select alu.COD_ALU, alu.COD_CUR, alu.DNI, alu.APELLIDOS, alu.NOMBRE
                                      from alumnos as alu
                                     where alu.NOMBRE like '%" + tBnombre.Text + "%' COLLATE Modern_Spanish_CI_AI and alu.APELLIDOS like '%" + tBapellidos.Text + "%' COLLATE Modern_Spanish_CI_AI";

                dataset1.Tables["Buscar"].Clear();
                daBuscar.Fill(dataset1, "Buscar");
            }

            else
            {
                //usar filtros
                try
                {
                    vistaBuscar.RowFilter = "NOMBRE like '%" + tBnombre.Text + "%' and APELLIDOS like '%" + tBapellidos.Text + "%'";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (dGbuscar.CurrentRow == null)
            {
                lInformacion.Text = "El Alumno no existe";
                lInformacion.Visible = true;
                dGbuscarAlumnos.Enabled = false;
                bMediaBuscar.Enabled = false;
                bGrabaBuscar.Enabled = false;
            }
            else
            {
                lInformacion.Visible = false;
                dGbuscarAlumnos.Enabled = true;
                bMediaBuscar.Enabled = true;
                bGrabaBuscar.Enabled = true;
            }
        }

        private void tBapellidos_TextChanged(object sender, EventArgs e)
        {
            if (chBbuscar.Checked == true)
            {
                sqlBuscar.CommandText = @"select alu.COD_ALU, alu.COD_CUR, alu.DNI, alu.APELLIDOS, alu.NOMBRE
                                      from alumnos as alu
                                      where alu.NOMBRE like '%" + tBnombre.Text + "%' COLLATE Modern_Spanish_CI_AI and alu.APELLIDOS like '%" + tBapellidos.Text + "%' COLLATE Modern_Spanish_CI_AI";

                dataset1.Tables["Buscar"].Clear();
                daBuscar.Fill(dataset1, "Buscar");
            }
            else
            {
                //usar filtros
                try
                {
                    vistaBuscar.RowFilter = "NOMBRE like '%" + tBnombre.Text + "%' and APELLIDOS like '%" + tBapellidos.Text + "%'";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (dGbuscar.CurrentRow == null)
            {
                lInformacion.Text = "El Alumno no existe";
                lInformacion.Visible = true;
                dGbuscarAlumnos.Enabled = false;
                bMediaBuscar.Enabled = false;
                bGrabaBuscar.Enabled = false;
            }
            else
            {
                lInformacion.Visible = false;
                dGbuscarAlumnos.Enabled = true;
                bMediaBuscar.Enabled = true;
                bGrabaBuscar.Enabled = true;
            }
        }

        private void dGbuscar_SelectionChanged(object sender, EventArgs e)
        {
            if (dGbuscar.CurrentRow != null && dGbuscarAlumnos.CurrentRow != null)
                vistaBuscarAlumnos.RowFilter = "COD_ALU = '" + dGbuscar.CurrentRow.Cells["COD_ALU"].Value.ToString() + "'";
        }

        private void chBbuscar_CheckedChanged(object sender, EventArgs e)
        {
            tBnombre.Text = "";
            tBapellidos.Text = "";
            lInformacion.Visible = false;
            bMediaBuscar.Enabled = true;
            bGrabaBuscar.Enabled = true;
            dGbuscarAlumnos.Enabled = true;

            vistaBuscar.RowFilter = "NOMBRE like '%'";

            sqlBuscar.CommandText = @"select alu.COD_ALU, alu.COD_CUR, alu.DNI, alu.APELLIDOS, alu.NOMBRE
                                      from alumnos as alu
                                     where alu.NOMBRE like '%'";

            dataset1.Tables["Buscar"].Clear();
            daBuscar.Fill(dataset1, "Buscar");

        }

        private void bMediaBuscar_Click(object sender, EventArgs e)
        {
            double media;

            for (int i = 0; i < dGbuscarAlumnos.Rows.Count; i++)
            {
                media = ((Convert.ToDouble(dGbuscarAlumnos.Rows[i].Cells["NOTA1"].Value) + Convert.ToDouble(dGbuscarAlumnos.Rows[i].Cells["NOTA2"].Value) + Convert.ToDouble(dGbuscarAlumnos.Rows[i].Cells["NOTA3"].Value)) / 3);
                media = Math.Round(media);

                dGbuscarAlumnos.Rows[i].Cells["MEDIA"].Value = media;
            }
        }

        private void bGrabaBuscar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dGcursos.Rows.Count; i++)
            {
                try
                {
                    sqlUpdateCommand.Parameters["@Curso"].Value = dGbuscarAlumnos.Rows[i].Cells["COD_CUR"].Value.ToString();
                    sqlUpdateCommand.Parameters["@Asignatura"].Value = dGbuscarAlumnos.Rows[i].Cells["COD_ASI"].Value.ToString();
                    sqlUpdateCommand.Parameters["@Alumno"].Value = dGbuscarAlumnos.Rows[i].Cells["COD_ALU"].Value.ToString();

                    sqlUpdateCommand.Parameters["@Nota1"].Value = dGbuscarAlumnos.Rows[i].Cells["NOTA1"].Value;
                    sqlUpdateCommand.Parameters["@Nota2"].Value = dGbuscarAlumnos.Rows[i].Cells["NOTA2"].Value;
                    sqlUpdateCommand.Parameters["@Nota3"].Value = dGbuscarAlumnos.Rows[i].Cells["NOTA3"].Value;
                    sqlUpdateCommand.Parameters["@Media"].Value = dGbuscarAlumnos.Rows[i].Cells["MEDIA"].Value;

                    abrirConexion();
                    sqlUpdateCommand.ExecuteNonQuery();
                    cerrarConexion();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private string quitarAcentos(string texto)
        {
            //using System.Text.RegularExpressions;

            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            texto = replace_a_Accents.Replace(texto, "a");
            texto = replace_e_Accents.Replace(texto, "e");
            texto = replace_i_Accents.Replace(texto, "i");
            texto = replace_o_Accents.Replace(texto, "o");
            texto = replace_u_Accents.Replace(texto, "u");
            return texto;
        }

        private void bTildes_Click(object sender, EventArgs e)
        {
            tBnombre.Text = quitarAcentos(tBnombre.Text);
            tBapellidos.Text = quitarAcentos(tBapellidos.Text);
        }
    }
}