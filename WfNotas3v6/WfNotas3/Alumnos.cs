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
        private void tPnotasAlumnos_Enter(object sender, EventArgs e)
        {
            dataset1.Tables["Notas"].Clear();
            daNotas2.Fill(dataset1, "Notas");

            dGalumnos1.Columns[0].Visible = false;    // solo funciona despues de la vista?
            dGalumnos2.Columns[0].Visible = false;      // solo funciona despues de la vista?
        }

        private void cargarAlumnos()
        {
            //  sqlSelectCommand

            // ALUMNOS pestaña 3
            sqlSelectCommandAlumnos.CommandText = "select alu.COD_ALU, alu.COD_CUR, alu.DNI, alu.APELLIDOS, alu.NOMBRE from alumnos as alu";
            sqlSelectCommandAlumnos.Connection = conexion;


            // sqlDataAdapter
            daAlumnos.SelectCommand = sqlSelectCommandAlumnos;
            daAlumnos.Fill(dataset1, "Alumnos");


            // Relacion
            relacionCursoAlumnos();

            cBaluCursos.DataSource = bindingSourceCurso;
            cBaluCursos.DisplayMember = "DESCRIPCION";
            cBaluCursos.ValueMember = "COD_CUR";

            creaColumnasAlumnosdg1();
            dGalumnos1.DataSource = bindingSourceRelacionCursoAlumno;

            creaColumnasAlumnosdg2();
            dGalumnos2.DataSource = bindingSourceRelacionAlumnoNota;
        }

        private void bMediaAlumnos_Click(object sender, EventArgs e)
        {
            double media;

            for (int i = 0; i < dGalumnos2.Rows.Count; i++)
            {
                media = ((Convert.ToDouble(dGalumnos2.Rows[i].Cells["dg4NOTA1"].Value) + Convert.ToDouble(dGalumnos2.Rows[i].Cells["dg4NOTA2"].Value) + Convert.ToDouble(dGalumnos2.Rows[i].Cells["dg4NOTA3"].Value)) / 3);
                media = Math.Round(media);

                dGalumnos2.Rows[i].Cells["dg4MEDIA"].Value = media;
            }
        }

        private void bGrabaAlumnos_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dGalumnos2.Rows.Count; i++)
            {
                sqlUpdateCommand.Parameters["@Curso"].Value = dGalumnos2.Rows[i].Cells["dg4COD_CUR"].Value.ToString();
                sqlUpdateCommand.Parameters["@Asignatura"].Value = dGalumnos2.Rows[i].Cells["dg4COD_ASI"].Value.ToString();
                sqlUpdateCommand.Parameters["@Alumno"].Value = dGalumnos2.Rows[i].Cells["dg4COD_ALU"].Value.ToString();

                sqlUpdateCommand.Parameters["@Nota1"].Value = dGalumnos2.Rows[i].Cells["dg4NOTA1"].Value;
                sqlUpdateCommand.Parameters["@Nota2"].Value = dGalumnos2.Rows[i].Cells["dg4NOTA2"].Value;
                sqlUpdateCommand.Parameters["@Nota3"].Value = dGalumnos2.Rows[i].Cells["dg4NOTA3"].Value;
                sqlUpdateCommand.Parameters["@Media"].Value = dGalumnos2.Rows[i].Cells["dg4MEDIA"].Value;

                abrirConexion();
                sqlUpdateCommand.ExecuteNonQuery();
                cerrarConexion();

                //dataset1.AcceptChanges();
            }
        }

        private void creaColumnasAlumnosdg1()
        {
            dGalumnos1.AutoGenerateColumns = false;
            dGalumnos1.ColumnCount = 5;

            dGalumnos1.Columns[0].Name = "dg3COD_CUR";
            dGalumnos1.Columns[0].HeaderText = "COD_CUR";
            dGalumnos1.Columns[0].DataPropertyName = "COD_CUR";
            dGalumnos1.Columns[0].Visible = false;

            dGalumnos1.Columns[1].Name = "dg3COD_ALU";
            dGalumnos1.Columns[1].HeaderText = "COD_ALU";
            dGalumnos1.Columns[1].DataPropertyName = "COD_ALU";
            dGalumnos1.Columns[1].Visible = true;

            dGalumnos1.Columns[2].Name = "dg3DNI";
            dGalumnos1.Columns[2].HeaderText = "DNI";
            dGalumnos1.Columns[2].DataPropertyName = "DNI";
            dGalumnos1.Columns[2].Visible = true;

            dGalumnos1.Columns[3].Name = "dg3APELLIDOS";
            dGalumnos1.Columns[3].HeaderText = "APELLIDOS";
            dGalumnos1.Columns[3].DataPropertyName = "APELLIDOS";
            dGalumnos1.Columns[3].Visible = true;

            dGalumnos1.Columns[4].Name = "dg3NOMBRE";
            dGalumnos1.Columns[4].HeaderText = "NOMBRE";
            dGalumnos1.Columns[4].DataPropertyName = "NOMBRE";
            dGalumnos1.Columns[4].Visible = true;
        }

        private void creaColumnasAlumnosdg2()
        {
            dGalumnos2.AutoGenerateColumns = false;
            dGalumnos2.ColumnCount = 12;

            dGalumnos2.Columns[0].Name = "dg4COD_CUR";
            dGalumnos2.Columns[0].HeaderText = "COD_CUR";
            dGalumnos2.Columns[0].DataPropertyName = "COD_CUR";
            dGalumnos2.Columns[0].Visible = false;
            dGalumnos2.Columns[0].ReadOnly = true;

            dGalumnos2.Columns[1].Name = "dg4DESCRIP_COD_CUR";
            dGalumnos2.Columns[1].HeaderText = "DESCRIPCION";
            dGalumnos2.Columns[1].DataPropertyName = "DESCRIPCION";
            dGalumnos2.Columns[1].Visible = false;
            dGalumnos2.Columns[1].ReadOnly = true;

            dGalumnos2.Columns[2].Name = "dg4COD_ASI";
            dGalumnos2.Columns[2].HeaderText = "COD_ASI";
            dGalumnos2.Columns[2].DataPropertyName = "COD_ASI";
            dGalumnos2.Columns[2].Visible = false;
            dGalumnos2.Columns[2].ReadOnly = true;

            dGalumnos2.Columns[3].Name = "dg4DESCRIP_COD_ASI";
            dGalumnos2.Columns[3].HeaderText = "DESCRIPCION";
            dGalumnos2.Columns[3].DataPropertyName = "DESCRIPCION1";
            dGalumnos2.Columns[3].Visible = true;
            dGalumnos2.Columns[3].ReadOnly = true;

            dGalumnos2.Columns[4].Name = "dg4COD_ALU";
            dGalumnos2.Columns[4].HeaderText = "COD_ALU";
            dGalumnos2.Columns[4].DataPropertyName = "COD_ALU";
            dGalumnos2.Columns[4].Visible = false;
            dGalumnos2.Columns[4].ReadOnly = true;

            dGalumnos2.Columns[5].Name = "dg4DNI";
            dGalumnos2.Columns[5].HeaderText = "DNI";
            dGalumnos2.Columns[5].DataPropertyName = "DNI";
            dGalumnos2.Columns[5].Visible = true;
            dGalumnos2.Columns[5].ReadOnly = true;

            dGalumnos2.Columns[6].Name = "dg4APELLIDOS";
            dGalumnos2.Columns[6].HeaderText = "APELLIDOS";
            dGalumnos2.Columns[6].DataPropertyName = "APELLIDOS";
            dGalumnos2.Columns[6].Visible = false;
            dGalumnos2.Columns[6].ReadOnly = true;

            dGalumnos2.Columns[7].Name = "dg4NOMBRE";
            dGalumnos2.Columns[7].HeaderText = "NOMBRE";
            dGalumnos2.Columns[7].DataPropertyName = "NOMBRE";
            dGalumnos2.Columns[7].Visible = false;
            dGalumnos2.Columns[7].ReadOnly = true;

            dGalumnos2.Columns[8].Name = "dg4NOTA1";
            dGalumnos2.Columns[8].HeaderText = "NOTA1";
            dGalumnos2.Columns[8].DataPropertyName = "NOTA1";
            dGalumnos2.Columns[8].Visible = true;

            dGalumnos2.Columns[9].Name = "dg4NOTA2";
            dGalumnos2.Columns[9].HeaderText = "NOTA2";
            dGalumnos2.Columns[9].DataPropertyName = "NOTA2";
            dGalumnos2.Columns[9].Visible = true;

            dGalumnos2.Columns[10].Name = "dg4NOTA3";
            dGalumnos2.Columns[10].HeaderText = "NOTA3";
            dGalumnos2.Columns[10].DataPropertyName = "NOTA3";
            dGalumnos2.Columns[10].Visible = true;

            dGalumnos2.Columns[11].Name = "dg4MEDIA";
            dGalumnos2.Columns[11].HeaderText = "MEDIA";
            dGalumnos2.Columns[11].DataPropertyName = "MEDIA";
            dGalumnos2.Columns[11].Visible = true;
        }
    }
}