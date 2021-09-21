using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;

namespace SCCI
{
    class Metodos
    {
        public static MySqlConnection Conexion;
        public static MySqlCommand Comando;
        public static MySqlDataAdapter Adaptador;
        public static DataSet Datos;
        public static MySqlDataReader Lector;
        public static string Nombre_Usuario, Nivel_Usuario, Usuario_Log, Control_CS;
        public static int Cod_Log, Control_C, Control_C2;
        public static char Control_F;

        // Metodo para conectarnos a MySQL
        public static void ConexionMySQL()
        {
            try
            {
                Conexion = new MySqlConnection();
                Conexion.ConnectionString = "server=localhost;user id=root;password=InformaticaDTS;database=SCF";

                if (Conexion.State == ConnectionState.Closed)
                    Conexion.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error al conectar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Metodo para Ejecutar una consulta sin devolver un valor (INSERT, UPDATE, DELETE)
        public static void Ejecutar(string CadMySQL)
        {
            try
            {
                ConexionMySQL();
                Comando = new MySqlCommand(CadMySQL, Conexion);
                Comando.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error al ejecutar consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Comando.Dispose();
                Conexion.Close();
            }
        }

        public static void EjecutarP(string Proced, MySqlParameter[] Parametros)
        {
            try
            {
                ConexionMySQL();
                Comando = new MySqlCommand(Proced, Conexion);

                Comando.CommandType = CommandType.StoredProcedure;
                Comando.Parameters.AddRange(Parametros);

                Comando.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error al ejecutar consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Comando.Dispose();
                Conexion.Close();
            }
        }

        // Metodo para Ejecutar consulta devolviendo datos (SELECT)
        public static DataTable Mostrar(string CadMySQL)
        {
            try
            {
                ConexionMySQL();
                Adaptador = new MySqlDataAdapter(CadMySQL, Conexion);
                Datos = new DataSet();
                Adaptador.Fill(Datos, "x");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error al ejecutar consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Adaptador.Dispose();
                Datos.Dispose();
                Conexion.Close();
            }

            return Datos.Tables["x"];
        }

        // Metodo que ejecuta una consulta y devuelve el número de registros afectados (INSERT, DELETE, UPDATE o SELECT)
        public static int Contar(string CadMySQL)
        {
            int Contador = -1;

            try
            {
                ConexionMySQL();
                Comando = new MySqlCommand(CadMySQL, Conexion);
                Contador = Convert.ToInt32(Comando.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error al ejecutar consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Comando.Dispose();
                Conexion.Close();
            }

            return Contador;
        }

        // Metodo que ejecuta una consulta y muestra los datos necesarios en un ComboBox
        public static void CargarCombo(string CadMySQL, ComboBox Combo)
        {
            try
            {
                ConexionMySQL();
                Adaptador = new MySqlDataAdapter(CadMySQL, Conexion);
                Datos = new DataSet();
                Adaptador.Fill(Datos);

                Combo.DataSource = Datos.Tables[0];
                Combo.DisplayMember = Datos.Tables[0].Columns[1].Caption.ToString();
                Combo.ValueMember = Datos.Tables[0].Columns[0].Caption.ToString();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error al ejecutar consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Adaptador.Dispose();
                Datos.Dispose();
                Conexion.Close();
            }
        }

        // Metodo que ejecuta una consulta y guarda los datos en un Lector
        public static MySqlDataReader LectorConsulta(string CadMySQL)
        {
            try
            {
                ConexionMySQL();
                Comando = new MySqlCommand(CadMySQL, Conexion);

                Lector = Comando.ExecuteReader();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error al ejecutar consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Lector;
        }

        // Metodo que valida que los Textbox o Combobox esten con información
        public static bool ValidarF(Form f)
        {
            int flag = 0;
            foreach (Control Ctrl in f.Controls)
            {
                if (Ctrl is TextBox || Ctrl is ComboBox)
                {
                    if (Ctrl.Text == String.Empty && Ctrl.Tag.ToString() != "No Validar")
                    {
                        MessageBox.Show(String.Format("Por Favor Ingrese {0}", Ctrl.Tag), "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Ctrl.Focus();

                        flag++;
                        break;
                    }
                }
            }

            if (flag > 0)
                return true;
            else
                return false;
        }

        public static bool Validar(GroupBox f)
        {
            int flag = 0;
            foreach (Control Ctrl in f.Controls)
            {
                if (Ctrl is TextBox || Ctrl is ComboBox)
                {
                    if (Ctrl.Text == String.Empty && Ctrl.Tag.ToString() != "No Validar")
                    {
                        MessageBox.Show(String.Format("Por Favor Ingrese {0}", Ctrl.Tag), "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Ctrl.Focus();

                        flag++;
                        break;
                    }
                }
            }

            if (flag > 0)
                return true;
            else
                return false;
        }

        // Metodo que limpia el contenido de los Textbox y ComboBox
        public static void LimpiarF(Form f)
        {
            foreach (Control Ctrl in f.Controls)
            {
                if (Ctrl is TextBox || Ctrl is ComboBox)
                    Ctrl.Text = String.Empty;
            }
        }

        public static void Limpiar(GroupBox f)
        {
            foreach (Control Ctrl in f.Controls)
            {
                if (Ctrl is TextBox || Ctrl is ComboBox)
                    Ctrl.Text = String.Empty;
            }
        }

        // Metodo que habilita o desabilita los Textbox y Combobox
        public static void Habilitar(Form f, bool Accion)
        {
            foreach (Control Ctrl in f.Controls)
            {
                if (Ctrl is TextBox || Ctrl is ComboBox || Ctrl is NumericUpDown || Ctrl is DateTimePicker)
                    Ctrl.Enabled = Accion;
            }
        }

        // Metodo que Exporta los registros que tenga el datagrid a Excel
        public static void ExportarExcel(DataGridView grd)
        {
            try
            {
                if (grd.DataSource != null)
                {
                    SaveFileDialog fichero = new SaveFileDialog();
                    fichero.Filter = "Microsoft Excel (*.xlsx)|*.xlsx";

                    if (fichero.ShowDialog() == DialogResult.OK)
                    {
                        Microsoft.Office.Interop.Excel.Application app;
                        Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                        Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;

                        app = new Microsoft.Office.Interop.Excel.Application();
                        libros_trabajo = app.Workbooks.Add();
                        hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

                        //Exportar cabeceras Datagrid
                        for (int i = 1; i <= grd.Columns.Count; i++)
                        {
                            hoja_trabajo.Cells[1, i] = grd.Columns[i - 1].HeaderText;
                        }

                        // Recorremos el Datagrid, rellenando la hoja de trabajo con los datos
                        int fil = 2;
                        foreach (DataGridViewRow f in grd.Rows)
                        {
                            for (int c = 0; c < grd.Columns.Count; c++)
                            {
                                hoja_trabajo.Cells[fil, c + 1] = f.Cells[c].Value;
                            }
                            fil++;
                        }

                        libros_trabajo.SaveAs(fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault);
                        libros_trabajo.Close(true);
                        app.Quit();

                        MessageBox.Show("Archivo exportado correctamente en la ruta seleccionada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // Metodo para poder imprimir (mostrar en pantalla) los reportes
        public static void Imprimir_Reporte(string SQL, string Tabla, CrystalDecisions.CrystalReports.Engine.ReportDocument Reporte)
        {
            Datos_SCCI Datos = new Datos_SCCI();
            Visor_Reportes Visor = new Visor_Reportes();

            ConexionMySQL();

            Datos.Clear();

            try
            {
                MySqlCommand Comando = new MySqlCommand(SQL, Conexion);
                MySqlDataAdapter Adaptador = new MySqlDataAdapter(Comando);
                Adaptador.Fill(Datos, Tabla);

                Reporte.SetDataSource(Datos);

                Visor.crystalReportViewer1.ReportSource = Reporte;
                Visor.Show();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Metodos.Conexion.Close();
            }
        }

    }
}
