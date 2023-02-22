using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace AdoNetConectado
{
    public partial class frmTipoCliente : Form
    {

        String cadenaConexion = @"Server=LAPTOP-MKA1MDEC; DataBase=BancoBD; Integrated Security=true;";
        public frmTipoCliente()
        {
            InitializeComponent();
            //Emamen Parcial
            //ALUMNO: Diego Jafet Lopez Cabezas
        }

        private void cargarFormulario(object sender, EventArgs e)
        {
            cargarDatos();

        }
        //creamos un metodo para abrir conexion
        private void cargarDatos()
        {
            //creamos una variable conexion
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (SqlCommand comando = new SqlCommand("SELECT * FROM TipoCliente", conexion))
                {
                    //Ejecutamos el comando 
                    using (var lector = comando.ExecuteReader())
                    {
                        if(lector != null && lector.HasRows)
                        {
                            //Mientars el lector se pueda leer agregará datos 
                            while (lector.Read())
                            {
                                dgvDatos.Rows.Add(lector[0], lector[1], lector[2], lector[3]);
                            }
                            lector.Close();
                        }
                        lector.Close();
                    }//Fin lector
                    
                }//fin comando

                
            }
            
            /*var conexion = new SqlConnection(cadenaConexion);
                conexion.Open();
            var querySql = "SELECT * FROM TipoCliente";
            var comando = new SqlCommand(querySql, conexion);
            var reader= comando.ExecuteReader();
            if(reader!= null && reader.HasRows)
            {
                reader.Read();
                dgvDatos.Rows.Add(reader[0], reader[1]);
            }
            //comando.ExecuteNonQuery();//Inserccion, Actualización, Eliminar*/

        }//fin metodo

       

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            var conexion = new SqlConnection(cadenaConexion);
            conexion.Open();
            var comando = new SqlCommand();
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        private void tsbEliminar_Click(object sender, EventArgs e)
        {
            int filaActual = dgvDatos.CurrentRow.Index;
            // var frm = new frmTipoClienteEdit();
            
           if (filaActual != null)
            {
                using (var conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    int idTipo = int.Parse(dgvDatos.CurrentRow.Cells[0].Value.ToString());
                    using (var comando = new SqlCommand("Delete from TipoCliente where ID=@id", conexion))
                    {
                        comando.Parameters.AddWithValue("@id", idTipo);
                        int filas= comando.ExecuteNonQuery();
                        if (filas > 0)
                        {
                            MessageBox.Show("Registro eliminado", "Sistema",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No se ha podido Eliminar el registro ", "Sistema",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        dgvDatos.Rows.Clear();
                        cargarDatos(); 
                    }
                }
            }
            
        }

        private void nuevoRegistro_Click(object sender, EventArgs e)
        {
            var frm = new frmTipoClienteEdit();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                String nombre = frm.Controls["txtNombre"].Text;
                String descripcion= frm.Controls["txtDescripcion"].Text;
                //Operador Ternario
                var estado = ((CheckBox)frm.Controls["chkEstado"]).Checked==true ? 1: 0;
                using (var conexion= new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (var comando= new SqlCommand("Insert into TipoCliente (Nombre, Descripcion, Estado)" +
                        "Values(@nombre, @descripcion, @estado)", conexion))
                    {
                        comando.Parameters.AddWithValue("@nombre", nombre);
                        comando.Parameters.AddWithValue("@descripcion", descripcion);
                        comando.Parameters.AddWithValue("@estado", estado);
                        int resultado = comando.ExecuteNonQuery();
                        dgvDatos.Rows.Clear();
                        cargarDatos();
                        if (resultado > 0)
                        {
                            MessageBox.Show("Datos Registrados", "Sistema",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No se ha podido registrar los datos ", "Sistema",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                       
                    }
                }
            }
            
                /* SqlConnection conexion = new SqlConnection(cadenaConexion);
                 conexion.Open();

                 if (frm.ShowDialog() == DialogResult.OK)
                 {

                     var querySql = $"INSERT INTO TipoCliente (Nombre, Descripcion, Estado) VALUES('{frm.Controls["txtNombre"].Text}', '{frm.Controls["txtDescripcion"].Text}','{frm.Controls["txtEstado"].Text}')";
                     var comando = new SqlCommand(querySql, conexion);
                     // MessageBox.Show("Filas afectadas: " +comando.ExecuteNonQuery()); Presenta las filas afectadas en un messagebox y inserta los datos
                     comando.ExecuteNonQuery();//Ejecuta la instrucción de insercción
                     dgvDatos.Rows.Clear();
                     cargarDatos();


                 }
                 conexion.Close();*/
            }

        private void editarRegistro(object sender, EventArgs e)
        {
            
            //Validamos que existan filas para editar
            if (dgvDatos.RowCount > 0 && dgvDatos.CurrentRow !=null )
            {
                //Tomamos el id del a fila seleccionada
                int filaActual = dgvDatos.CurrentRow.Index;
                //Tenemos el ID tipo
                int idTipo = int.Parse(dgvDatos.CurrentRow.Cells[0].Value.ToString());
                //Tomamos el ID de la fila seleccionada
                var frm = new frmTipoClienteEdit(idTipo);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    String nombre = frm.Controls["txtNombre"].Text;
                    String descripcion = frm.Controls["txtDescripcion"].Text;
                    //Operador Ternario
                    var estado = ((CheckBox)frm.Controls["chkEstado"]).Checked == true ? 1 : 0;
                    using (var conexion= new SqlConnection(cadenaConexion))
                    {
                       conexion.Open();
                        using (var comando= new SqlCommand("Update TipoCliente set Nombre=@nombre," +
                            "Descripcion=@descripcion, Estado=@estado where ID=@id", conexion))
                        {
                            comando.Parameters.AddWithValue("@nombre", nombre);
                            comando.Parameters.AddWithValue("@descripcion", descripcion);
                            comando.Parameters.AddWithValue("@estado", estado);
                            comando.Parameters.AddWithValue("@id", idTipo);
                            int resultado = comando.ExecuteNonQuery();
                            dgvDatos.Rows.Clear();
                            cargarDatos();
                            if (resultado > 0)
                            {
                                MessageBox.Show("Datos actualizados", "Sistema",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("No se ha podido actualizar los datos ", "Sistema",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                           
                        }
                    }
                }
            }//Fin del if row count
            
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
