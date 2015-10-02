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
        private void tPnotasCursos_Enter(object sender, EventArgs e)
        {
            dataset1.Tables["Notas"].Clear();
            daNotas2.Fill(dataset1, "Notas");

            dGcursos.Columns[0].Visible = false;    // solo funciona despues de la vista?
        }

        private void cargarCursos()
        {
            sqlSelectCommandCursos = new SqlCommand();


            //  sqlSelectCommand

            // CURSOS pestaña 2
            sqlSelectCommandCursos.CommandText = "select cur.COD_CUR, cur.DESCRIPCION from CURSOS as cur";
            sqlSelectCommandCursos.Connection = conexion;


            // ASIGNATURAS pestaña 2
            sqlSelectCommandAsignaturas.CommandText = "select asi.COD_ASI, asi.DESCRIPCION, asi.COD_CUR from ASIGNATURAS as asi";
            sqlSelectCommandAsignaturas.Connection = conexion;


            // NOTAS pestaña 2
            sqlSelectCommandNotas.CommandText = @"select cur.COD_CUR, cur.DESCRIPCION, asi.COD_ASI, asi.DESCRIPCION, alu.COD_ALU, alu.DNI, alu.APELLIDOS, alu.NOMBRE, 
                n.NOTA1, n.NOTA2, n.NOTA3, n.MEDIA from NOTAS as n
                inner join CURSOS as cur on cur.COD_CUR = n.COD_CUR
                inner join ASIGNATURAS as asi on asi.COD_ASI = n.COD_ASI
                inner join ALUMNOS as alu on alu.COD_ALU = n.COD_ALU";
            sqlSelectCommandNotas.Connection = conexion;



            // sqlDataAdapter
            daNotas.SelectCommand = sqlSelectCommandNotas;
            daNotas.Fill(dataset1, "Notas");

            daCursos.SelectCommand = sqlSelectCommandCursos;
            daCursos.Fill(dataset1, "Cursos");

            daAsignaturas.SelectCommand = sqlSelectCommandAsignaturas;
            daAsignaturas.Fill(dataset1, "Asignaturas");


            // Relacion
            relacionCursoAsignatura();

            cBcurCursos.DataSource = bindingSourceCurso;
            cBcurCursos.DisplayMember = "DESCRIPCION";
            cBcurCursos.ValueMember = "COD_CUR";

            cBcurAsignaturas.DataSource = bindingSourceRelacionCursoAsignatura;
            cBcurAsignaturas.DisplayMember = "DESCRIPCION";
            cBcurAsignaturas.ValueMember = "COD_ASI";

            creaColumnasCursos();

            dGcursos.DataSource = bindingSourceRelacionAsignaturaNota;
        }



        private void bMedia_Click(object sender, EventArgs e)
        {
            double media;

            for (int i = 0; i < dGcursos.Rows.Count; i++)
            {
                media = ((Convert.ToDouble(dGcursos.Rows[i].Cells["dg2NOTA1"].Value) + Convert.ToDouble(dGcursos.Rows[i].Cells["dg2NOTA2"].Value) + Convert.ToDouble(dGcursos.Rows[i].Cells["dg2NOTA3"].Value)) / 3);
                media = Math.Round(media);

                dGcursos.Rows[i].Cells["dg2MEDIA"].Value = media;
            }
        }

        private void bGraba_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dGcursos.Rows.Count; i++)
            {
                sqlUpdateCommand.Parameters["@Curso"].Value = dGcursos.Rows[i].Cells["dg2COD_CUR"].Value.ToString();
                sqlUpdateCommand.Parameters["@Asignatura"].Value = dGcursos.Rows[i].Cells["dg2COD_ASI"].Value.ToString();
                sqlUpdateCommand.Parameters["@Alumno"].Value = dGcursos.Rows[i].Cells["dg2COD_ALU"].Value.ToString();

                sqlUpdateCommand.Parameters["@Nota1"].Value = dGcursos.Rows[i].Cells["dg2NOTA1"].Value;
                sqlUpdateCommand.Parameters["@Nota2"].Value = dGcursos.Rows[i].Cells["dg2NOTA2"].Value;
                sqlUpdateCommand.Parameters["@Nota3"].Value = dGcursos.Rows[i].Cells["dg2NOTA3"].Value;
                sqlUpdateCommand.Parameters["@Media"].Value = dGcursos.Rows[i].Cells["dg2MEDIA"].Value;

                abrirConexion();
                sqlUpdateCommand.ExecuteNonQuery();
                cerrarConexion();

                dataset1.AcceptChanges();
            }
        }

        private void creaColumnasCursos()
        {
            dGcursos.AutoGenerateColumns = false;
            dGcursos.ColumnCount = 12;

            dGcursos.Columns[0].Name = "dg2COD_CUR";
            dGcursos.Columns[0].HeaderText = "COD_CUR";
            dGcursos.Columns[0].DataPropertyName = "COD_CUR";
            dGcursos.Columns[0].Visible = false;
            dGcursos.Columns[0].ReadOnly = true;

            dGcursos.Columns[1].Name = "dg2DESCRIP_COD_CUR";
            dGcursos.Columns[1].HeaderText = "DESCRIPCION";
            dGcursos.Columns[1].DataPropertyName = "DESCRIPCION";
            dGcursos.Columns[1].Visible = false;
            dGcursos.Columns[1].ReadOnly = true;

            dGcursos.Columns[2].Name = "dg2COD_ASI";
            dGcursos.Columns[2].HeaderText = "COD_ASI";
            dGcursos.Columns[2].DataPropertyName = "COD_ASI";
            dGcursos.Columns[2].Visible = false;
            dGcursos.Columns[2].ReadOnly = true;

            dGcursos.Columns[3].Name = "dg2DESCRIP_COD_ASI";
            dGcursos.Columns[3].HeaderText = "DESCRIPCION";
            dGcursos.Columns[3].DataPropertyName = "DESCRIPCION1";
            dGcursos.Columns[3].Visible = true;
            dGcursos.Columns[3].ReadOnly = true;

            dGcursos.Columns[4].Name = "dg2COD_ALU";
            dGcursos.Columns[4].HeaderText = "COD_ALU";
            dGcursos.Columns[4].DataPropertyName = "COD_ALU";
            dGcursos.Columns[4].Visible = false;
            dGcursos.Columns[4].ReadOnly = true;

            dGcursos.Columns[5].Name = "dg2DNI";
            dGcursos.Columns[5].HeaderText = "DNI";
            dGcursos.Columns[5].DataPropertyName = "DNI";
            dGcursos.Columns[5].Visible = true;
            dGcursos.Columns[5].ReadOnly = true;

            dGcursos.Columns[6].Name = "dg2APELLIDOS";
            dGcursos.Columns[6].HeaderText = "APELLIDOS";
            dGcursos.Columns[6].DataPropertyName = "APELLIDOS";
            dGcursos.Columns[6].Visible = true;
            dGcursos.Columns[6].ReadOnly = true;

            dGcursos.Columns[7].Name = "dg2NOMBRE";
            dGcursos.Columns[7].HeaderText = "NOMBRE";
            dGcursos.Columns[7].DataPropertyName = "NOMBRE";
            dGcursos.Columns[7].Visible = true;
            dGcursos.Columns[7].ReadOnly = true;

            dGcursos.Columns[8].Name = "dg2NOTA1";
            dGcursos.Columns[8].HeaderText = "NOTA1";
            dGcursos.Columns[8].DataPropertyName = "NOTA1";
            dGcursos.Columns[8].Visible = true;

            dGcursos.Columns[9].Name = "dg2NOTA2";
            dGcursos.Columns[9].HeaderText = "NOTA2";
            dGcursos.Columns[9].DataPropertyName = "NOTA2";
            dGcursos.Columns[9].Visible = true;

            dGcursos.Columns[10].Name = "dg2NOTA3";
            dGcursos.Columns[10].HeaderText = "NOTA3";
            dGcursos.Columns[10].DataPropertyName = "NOTA3";
            dGcursos.Columns[10].Visible = true;

            dGcursos.Columns[11].Name = "dg2MEDIA";
            dGcursos.Columns[11].HeaderText = "MEDIA";
            dGcursos.Columns[11].DataPropertyName = "MEDIA";
            dGcursos.Columns[11].Visible = true;
        }
    }
}