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
        private void tPsql_Enter(object sender, EventArgs e)
        {
            cBselect.Items.Clear();
            cBselect.Items.Add("*");
            cBwhere.Items.Clear();
            cBwhere.Items.Add("sin condicion");

            abrirConexion();
            sqlCursos.ExecuteNonQuery();
            sqlAsignaturas.ExecuteNonQuery();
            sqlAlumnos.ExecuteNonQuery();
            sqlNotas.ExecuteNonQuery();
            cerrarConexion();

            
            cBselect.SelectedIndex = 0;
            cBfrom.SelectedIndex = 0;
            cBwhere.SelectedIndex = 0;
            cBlike.SelectedIndex = 0;


            if (cBfrom.Text == "alumnos")
                for (int i = 0; i < dataset1.Tables["AlumnosSql"].Columns.Count; i++)
                {
                    cBselect.Items.Add(dataset1.Tables["AlumnosSql"].Columns[i].ColumnName);
                    cBwhere.Items.Add(dataset1.Tables["AlumnosSql"].Columns[i].ColumnName);
                }
            cBlike.Enabled = false;
            tBcondicion.Enabled = false;

            lSql.Text = "select " + cBselect.Text + " from " + cBfrom.Text;
        }

        private void cBfrom_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // PASO 1

            cBselect.Items.Clear();
            cBselect.Items.Add("*");
            cBselect.SelectedIndex = 0;

            cBwhere.Items.Clear();
            cBwhere.Items.Add("sin condicion");
            cBwhere.SelectedIndex = 0;

            if (cBfrom.Text == "cursos")
                for (int i = 0; i < dataset1.Tables["CursosSql"].Columns.Count; i++)
                {
                    cBselect.Items.Add(dataset1.Tables["CursosSql"].Columns[i].ColumnName);
                    cBwhere.Items.Add(dataset1.Tables["CursosSql"].Columns[i].ColumnName);
                }

            if (cBfrom.Text == "asignaturas")
                for (int i = 0; i < dataset1.Tables["AsignaturasSql"].Columns.Count; i++)
                {
                    cBselect.Items.Add(dataset1.Tables["AsignaturasSql"].Columns[i].ColumnName);
                    cBwhere.Items.Add(dataset1.Tables["AsignaturasSql"].Columns[i].ColumnName);
                }

            if (cBfrom.Text == "alumnos")
                for (int i = 0; i < dataset1.Tables["AlumnosSql"].Columns.Count; i++)
                {
                    cBselect.Items.Add(dataset1.Tables["AlumnosSql"].Columns[i].ColumnName);
                    cBwhere.Items.Add(dataset1.Tables["AlumnosSql"].Columns[i].ColumnName);
                }

            if (cBfrom.Text == "notas")
                for (int i = 0; i < dataset1.Tables["NotasSql"].Columns.Count; i++)
                {
                    cBselect.Items.Add(dataset1.Tables["NotasSql"].Columns[i].ColumnName);
                    cBwhere.Items.Add(dataset1.Tables["NotasSql"].Columns[i].ColumnName);
                }
                 
            lSql.Text = "select " + cBselect.Text + " from " + cBfrom.Text;
        }

        private void cBselect_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // PASO 2

            lSql.Text = "select " + cBselect.Text + " from " + cBfrom.Text;
        }

        private void cBwhere_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // PASO 3

            if (cBwhere.Text == "sin condicion")
            {
                lSql.Text = "select " + cBselect.Text + " from " + cBfrom.Text;
                cBlike.Enabled = false;
                tBcondicion.Enabled = false;
            }
            else
            {
                cBlike.Enabled = true;
                tBcondicion.Enabled = true;

                if (cBlike.Text == "like")
                    lSql.Text = "select " + cBselect.Text + " from " + cBfrom.Text + " where " + cBwhere.Text + " like '%" + tBcondicion.Text + "%'";
                else
                    lSql.Text = "select " + cBselect.Text + " from " + cBfrom.Text + " where " + cBwhere.Text + " = '" + tBcondicion.Text + "'";
            }
        }

        private void cBlike_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // PASO 4

            if (cBlike.Text == "like")
                lSql.Text = "select " + cBselect.Text + " from " + cBfrom.Text + " where " + cBwhere.Text + " like '%" + tBcondicion.Text + "%'";
            else
                lSql.Text = "select " + cBselect.Text + " from " + cBfrom.Text + " where " + cBwhere.Text + " = '" + tBcondicion.Text + "'";
        }

        private void tBcondicion_TextChanged(object sender, EventArgs e)
        {
            // PASO 5

            if (cBlike.Text == "like")
                lSql.Text = "select " + cBselect.Text + " from " + cBfrom.Text + " where " + cBwhere.Text + " like '%" + tBcondicion.Text + "%'";
            else
                lSql.Text = "select " + cBselect.Text + " from " + cBfrom.Text + " where " + cBwhere.Text + " = '" + tBcondicion.Text + "'";
        }


        private void bBuscarSql_Click(object sender, EventArgs e)
        {
            consulta();

            //borrar datagrid

            int cantidadColumnas = dGsql.Columns.Count;

            for (int i = 0; i < cantidadColumnas; cantidadColumnas--)
                dataset1.Tables["Sql"].Columns.RemoveAt(i);

            dataset1.Tables["Sql"].Clear();
            dataset1.Tables["Sql"].AcceptChanges();
            
            daSql.Fill(dataset1, "Sql");
        }


        private void consulta()
        {
            sql = new SqlCommand();
            daSql = new SqlDataAdapter();

            //  sqlSelectCommand
            sql.CommandText = lSql.Text;
            sql.Connection = conexion;

            // sqlDataAdapter
            daSql.SelectCommand = sql;
            daSql.Fill(dataset1, "Sql");

            // DataSource
            dGsql.DataSource = dataset1.Tables["Sql"];


            abrirConexion();
            sql.ExecuteNonQuery();
            cerrarConexion();
        }

        private void cargarSql()
        {
            sqlCursos = new SqlCommand();
            sqlAsignaturas = new SqlCommand();
            sqlAlumnos = new SqlCommand();
            sqlNotas = new SqlCommand();

            daSqlCursos = new SqlDataAdapter();
            daSqlAsignaturas = new SqlDataAdapter();
            daSqlAlumnos = new SqlDataAdapter();
            daSqlNotas = new SqlDataAdapter();


            //  sqlSelectCommand
            sqlCursos.CommandText = "select top 0 * from CURSOS";
            sqlCursos.Connection = conexion;

            sqlAsignaturas.CommandText = "select top 0 * from ASIGNATURAS";
            sqlAsignaturas.Connection = conexion;

            sqlAlumnos.CommandText = "select top 0 * from ALUMNOS";
            sqlAlumnos.Connection = conexion;

            sqlNotas.CommandText = "select top 0 * from NOTAS";
            sqlNotas.Connection = conexion;


            // sqlDataAdapter
            daSqlCursos.SelectCommand = sqlCursos;
            daSqlCursos.Fill(dataset1, "CursosSql");

            daSqlAsignaturas.SelectCommand = sqlAsignaturas;
            daSqlAsignaturas.Fill(dataset1, "AsignaturasSql");

            daSqlAlumnos.SelectCommand = sqlAlumnos;
            daSqlAlumnos.Fill(dataset1, "AlumnosSql");

            daSqlNotas.SelectCommand = sqlNotas;
            daSqlNotas.Fill(dataset1, "NotasSql");
        }
    }
}