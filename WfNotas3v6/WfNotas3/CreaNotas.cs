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
        SqlCommand sqlCreaNotas;
        SqlCommand sqlSelectCommandNotas;
        SqlCommand sqlSelectCommandNotas2;
        SqlCommand sqlSelectCommandCursos;
        SqlCommand sqlUpdateCommand;
        SqlCommand sqlSelectCommandAsignaturas;
        SqlCommand sqlSelectCommandAlumnos;
        SqlCommand sql;                         //pestaña sql
        SqlCommand sqlCursos;                   //pestaña sql
        SqlCommand sqlAsignaturas;              //pestaña sql
        SqlCommand sqlAlumnos;                  //pestaña sql
        SqlCommand sqlNotas;                    //pestaña sql
        SqlCommand sqlBuscar;                   //pestaña buscar

        SqlDataAdapter daNotas;
        SqlDataAdapter daNotas2;
        SqlDataAdapter daCursos;
        SqlDataAdapter daAsignaturas;
        SqlDataAdapter daAlumnos;
        SqlDataAdapter daSql;                   // pestaña sql
        SqlDataAdapter daSqlCursos;             // pestaña sql
        SqlDataAdapter daSqlAsignaturas;        // pestaña sql
        SqlDataAdapter daSqlAlumnos;            // pestaña sql
        SqlDataAdapter daSqlNotas;              // pestaña sql
        SqlDataAdapter daBuscar;                // pestaña buscar


        DataView vistaNotasDg;                  // data grid primera pestaña
        DataView vistaBuscarAlumnos;            // data grid pestaña buscar
        DataView vistaBuscar;                   // data grid pestaña buscar
        

        //RELACION pestaña 2
        BindingSource bindingSourceCurso;
        BindingSource bindingSourceAsignatura;
        BindingSource bindingSourceRelacionCursoAsignatura;
        BindingSource bindingSourceRelacionAsignaturaNota;

        DataColumn ColCurso;
        DataColumn ColAsignaturaCUR, ColAsignaturaASI;
        DataColumn ColNota;

        DataRelation RelacionCursoAsignatura;
        DataRelation RelacionAsignaturaNota;

        //RELACION pestaña 3
        BindingSource bindingSourceAlumno;
        BindingSource bindingSourceRelacionCursoAlumno;
        BindingSource bindingSourceRelacionAlumnoNota;

        DataColumn ColAlumnoCUR;
        DataColumn ColAlumnoALU;

        DataRelation RelacionCursoAlumno;
        DataRelation RelacionAlumnoNota;




        private void tPcrearNotas_Enter(object sender, EventArgs e)
        {
            dataset1.Tables["Notas2"].Clear();
            daNotas2.Fill(dataset1, "Notas2");
            //dGCargarNotas.Refresh();
        }


        private void cargarNotas()
        {
            if (!abrirConexion())
                return;

            sqlSelectCommandNotas = new SqlCommand();
            sqlSelectCommandNotas2 = new SqlCommand();
            sqlUpdateCommand = new SqlCommand();
            sqlSelectCommandAsignaturas = new SqlCommand();
            sqlSelectCommandAlumnos = new SqlCommand();
            sqlCreaNotas = new SqlCommand();

            daNotas = new SqlDataAdapter();
            daNotas2 = new SqlDataAdapter();
            daCursos = new SqlDataAdapter();
            daAsignaturas = new SqlDataAdapter();
            daAlumnos = new SqlDataAdapter();


            //  sqlSelectCommand
            
            // NOTAS
            sqlSelectCommandNotas2.CommandText = @"select cur.COD_CUR, cur.DESCRIPCION, asi.COD_ASI, asi.DESCRIPCION, alu.COD_ALU, alu.DNI, alu.APELLIDOS, alu.NOMBRE, 
                n.NOTA1, n.NOTA2, n.NOTA3, n.MEDIA from NOTAS as n
                inner join CURSOS as cur on cur.COD_CUR = n.COD_CUR
                inner join ASIGNATURAS as asi on asi.COD_ASI = n.COD_ASI
                inner join ALUMNOS as alu on alu.COD_ALU = n.COD_ALU";
            sqlSelectCommandNotas2.Connection = conexion;


            // Crea Notas pestaña 1
            sqlCreaNotas.CommandText = @"TRUNCATE TABLE NOTAS
                INSERT INTO NOTAS (COD_CUR, COD_ASI, COD_ALU, NOTA1, NOTA2, NOTA3, MEDIA)
                (select cur.COD_CUR, asi.COD_ASI, alu.COD_ALU, NOTA1 = 0, NOTA2 = 0, NOTA3 = 0, MEDIA = 0 from CURSOS as cur
                inner join ASIGNATURAS as asi on cur.COD_CUR = asi.COD_CUR
                inner join ALUMNOS as alu on cur.COD_CUR = alu.COD_CUR)";
            sqlCreaNotas.Connection = conexion;



            // sqlUpdateCommand pestaña 2   
            sqlUpdateCommand.Parameters.Add(new SqlParameter("@Curso", SqlDbType.VarChar, 10, "COD_CUR"));
            sqlUpdateCommand.Parameters.Add(new SqlParameter("@Asignatura", SqlDbType.VarChar, 10, "COD_ASI"));
            sqlUpdateCommand.Parameters.Add(new SqlParameter("@Alumno", SqlDbType.VarChar, 10, "COD_ALU"));

            sqlUpdateCommand.Parameters.Add(new SqlParameter("@Nota1", SqlDbType.Int, 10, "NOTA1"));
            sqlUpdateCommand.Parameters.Add(new SqlParameter("@Nota2", SqlDbType.Int, 10, "NOTA2"));
            sqlUpdateCommand.Parameters.Add(new SqlParameter("@Nota3", SqlDbType.Int, 10, "NOTA3"));
            sqlUpdateCommand.Parameters.Add(new SqlParameter("@Media", SqlDbType.Int, 10, "MEDIA"));

            sqlUpdateCommand.CommandText = "UPDATE NOTAS SET NOTA1 = @Nota1, NOTA2 = @Nota2, NOTA3 = @Nota3, MEDIA = @Media WHERE COD_CUR = @Curso AND COD_ASI = @Asignatura AND COD_ALU = @Alumno";
            sqlUpdateCommand.Connection = conexion;



            // sqlDataAdapter
            daNotas2.SelectCommand = sqlSelectCommandNotas2;
            daNotas2.UpdateCommand = sqlUpdateCommand;
            daNotas2.Fill(dataset1, "Notas2");


            // Vista
            vistaNotasDg = new DataView(dataset1.Tables["Notas2"]);


            //crear columnas
            creaColumnasNotas();


            // DataSource
            dGCargarNotas.DataSource = vistaNotasDg;

            cerrarConexion();
        }


        private void bCreaNotas_Click(object sender, EventArgs e)
        {
            if (!abrirConexion())
                return;

            sqlCreaNotas.ExecuteNonQuery();
            
            // sqlDataAdapter
            dataset1.Tables["Notas2"].Clear();
            daNotas2.Fill(dataset1, "Notas2");
            //dGCargarNotas.Refresh();

            cerrarConexion();
        }

        private void creaColumnasNotas()
        {
            dGCargarNotas.AutoGenerateColumns = false;
            dGCargarNotas.ColumnCount = 12;

            dGCargarNotas.Columns[0].Name = "dg1COD_CUR";
            dGCargarNotas.Columns[0].HeaderText = "COD_CUR";
            dGCargarNotas.Columns[0].DataPropertyName = "COD_CUR";
            dGCargarNotas.Columns[0].Visible = true;

            dGCargarNotas.Columns[1].Name = "dg1DESCRIP_COD_CUR";
            dGCargarNotas.Columns[1].HeaderText = "DESCRIPCION";
            dGCargarNotas.Columns[1].DataPropertyName = "DESCRIPCION";
            dGCargarNotas.Columns[1].Visible = true;

            dGCargarNotas.Columns[2].Name = "dg1COD_ASI";
            dGCargarNotas.Columns[2].HeaderText = "COD_ASI";
            dGCargarNotas.Columns[2].DataPropertyName = "COD_ASI";
            dGCargarNotas.Columns[2].Visible = true;

            dGCargarNotas.Columns[3].Name = "dg1DESCRIP_COD_ASI";
            dGCargarNotas.Columns[3].HeaderText = "DESCRIPCION";
            dGCargarNotas.Columns[3].DataPropertyName = "DESCRIPCION1";
            dGCargarNotas.Columns[3].Visible = true;

            dGCargarNotas.Columns[4].Name = "dg1COD_ALU";
            dGCargarNotas.Columns[4].HeaderText = "COD_ALU";
            dGCargarNotas.Columns[4].DataPropertyName = "COD_ALU";
            dGCargarNotas.Columns[4].Visible = true;

            dGCargarNotas.Columns[5].Name = "dg1DNI";
            dGCargarNotas.Columns[5].HeaderText = "DNI";
            dGCargarNotas.Columns[5].DataPropertyName = "DNI";
            dGCargarNotas.Columns[5].Visible = true;

            dGCargarNotas.Columns[6].Name = "dg1APELLIDOS";
            dGCargarNotas.Columns[6].HeaderText = "APELLIDOS";
            dGCargarNotas.Columns[6].DataPropertyName = "APELLIDOS";
            dGCargarNotas.Columns[6].Visible = true;

            dGCargarNotas.Columns[7].Name = "dg1NOMBRE";
            dGCargarNotas.Columns[7].HeaderText = "NOMBRE";
            dGCargarNotas.Columns[7].DataPropertyName = "NOMBRE";
            dGCargarNotas.Columns[7].Visible = true;

            dGCargarNotas.Columns[8].Name = "dg1NOTA1";
            dGCargarNotas.Columns[8].HeaderText = "NOTA1";
            dGCargarNotas.Columns[8].DataPropertyName = "NOTA1";
            dGCargarNotas.Columns[8].Visible = true;

            dGCargarNotas.Columns[9].Name = "dg1NOTA2";
            dGCargarNotas.Columns[9].HeaderText = "NOTA2";
            dGCargarNotas.Columns[9].DataPropertyName = "NOTA2";
            dGCargarNotas.Columns[9].Visible = true;

            dGCargarNotas.Columns[10].Name = "dg1NOTA3";
            dGCargarNotas.Columns[10].HeaderText = "NOTA3";
            dGCargarNotas.Columns[10].DataPropertyName = "NOTA3";
            dGCargarNotas.Columns[10].Visible = true;

            dGCargarNotas.Columns[11].Name = "dg1MEDIA";
            dGCargarNotas.Columns[11].HeaderText = "MEDIA";
            dGCargarNotas.Columns[11].DataPropertyName = "MEDIA";
            dGCargarNotas.Columns[11].Visible = true;
        }
    }
}