using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WfNotas3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConexionDb();   // crear conexion
            dataset1 = new DataSet();
            cargarNotas();
            cargarCursos();
            cargarAlumnos();
            cargarSql();
            cargarBuscar();
        }
    }
}
