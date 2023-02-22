using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetConectado
{
    public partial class frmTipoClienteEdit : Form
    {

       // DataRow fila;
        int ID;
        String cadenaConexion = @"Server=LAPTOP-MKA1MDEC; DataBase=BancoBD; Integrated Security=true;";
        public frmTipoClienteEdit(int id= 0)//Agregamos filaEditar como parametro
        {
 
            InitializeComponent();
            //Emamen Parcial
            //ALUMNO: Diego Jafet Lopez Cabezas
            //Falta cambiar el testo del formulario tanto cuando se edite un cliente
    
            this.ID= id;

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

        }
        private void mostrarDatos()
        {
            using (var conexion= new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var comando = new SqlCommand("Select * from TipoCliente Where ID = @id", conexion))
                {
                    comando.Parameters.AddWithValue("@id", this.ID);
                    using ( var reader= comando.ExecuteReader())
                    {
                        if (reader !=null && reader.HasRows)
                        {
                            reader.Read();
                            txtNombre.Text= reader[1].ToString();
                            txtDescripcion.Text = reader[2].ToString();
                            chkEstado.Checked= reader[3].ToString() == "1" ? true:false;
                        }
                    }
                }
            }
        }

        private void aceptarCambios_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar un nombre valido", "Sistema",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void cargarDatos(object sender, EventArgs e)
        {
            if (this.ID > 0)
            {
                this.Text = "Editar";
                mostrarDatos();
            }
        }

    }
}
