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
        private void relacionCursoAsignatura()
        {
            bindingSourceCurso = new BindingSource();
            bindingSourceAsignatura = new BindingSource();
            bindingSourceRelacionCursoAsignatura = new BindingSource();
            bindingSourceRelacionAsignaturaNota = new BindingSource();

            bindingSourceCurso.DataSource = dataset1.Tables["Cursos"];
            bindingSourceAsignatura.DataSource = dataset1.Tables["Asignaturas"];

            ColCurso = dataset1.Tables["Cursos"].Columns["COD_CUR"];
            ColAsignaturaCUR = dataset1.Tables["Asignaturas"].Columns["COD_CUR"];
            ColAsignaturaASI = dataset1.Tables["Asignaturas"].Columns["COD_ASI"];
            ColNota = dataset1.Tables["Notas"].Columns["COD_ASI"];

            RelacionCursoAsignatura = new DataRelation("RelCursoAsignatura", ColCurso, ColAsignaturaCUR);
            RelacionAsignaturaNota = new DataRelation("RelAsignaturaNota", ColAsignaturaASI, ColNota);

            dataset1.Relations.Clear();
            dataset1.Relations.Add(RelacionCursoAsignatura);
            dataset1.Relations.Add(RelacionAsignaturaNota);

            bindingSourceRelacionCursoAsignatura.DataSource = bindingSourceCurso;
            bindingSourceRelacionCursoAsignatura.DataMember = "RelCursoAsignatura";

            bindingSourceRelacionAsignaturaNota.DataSource = bindingSourceRelacionCursoAsignatura;
            bindingSourceRelacionAsignaturaNota.DataMember = "RelAsignaturaNota";
        }


        private void relacionCursoAlumnos()
        {
            //bindingSourceAsignatura = new BindingSource();
            bindingSourceAlumno = new BindingSource();


            bindingSourceRelacionCursoAlumno = new BindingSource();
            bindingSourceRelacionAlumnoNota = new BindingSource();
            bindingSourceAlumno.DataSource = dataset1.Tables["Alumnos"];

            ColAlumnoCUR = dataset1.Tables["Alumnos"].Columns["COD_CUR"];
            ColAlumnoALU = dataset1.Tables["Alumnos"].Columns["COD_ALU"];
            ColNota = dataset1.Tables["Notas"].Columns["COD_ALU"];

            RelacionCursoAlumno = new DataRelation("RelCursoAlumno", ColCurso, ColAlumnoCUR);
            RelacionAlumnoNota = new DataRelation("RelAlumnoNota", ColAlumnoALU, ColNota);

            dataset1.Relations.Add(RelacionCursoAlumno);
            dataset1.Relations.Add(RelacionAlumnoNota);

            bindingSourceRelacionCursoAlumno.DataSource = bindingSourceCurso;
            bindingSourceRelacionCursoAlumno.DataMember = "RelCursoAlumno";

            bindingSourceRelacionAlumnoNota.DataSource = bindingSourceRelacionCursoAlumno;
            bindingSourceRelacionAlumnoNota.DataMember = "RelAlumnoNota";
        }
    }
}