using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Mail;
using System.Configuration;
using System.Net.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace ManejoDeTablas
{
    public partial class FrmRegistroEliminacionColecciones : Form
    {
        List<EnvioCorreo> lista = new List<EnvioCorreo>();
        public FrmRegistroEliminacionColecciones()
        {
            InitializeComponent();

            //que el combobox tenga por defecto el elemento "No"
            cboEnviado.SelectedIndex = 1;
            //cboEnviado.SelectedItem = "No";

            EnvioCorreo registro = new EnvioCorreo(false, "pruebavisualstudio02@gmail.com", "Johann Tristan", 100);
            lista.Add(registro);
            cargarLista();

            //EnvioCorreo registro = new EnvioCorreo(false, "Johann@gmail.com", "Johann Tristan", 100);
            //lista.Add(registro);

            //registro = new EnvioCorreo()
            //{
            //    Enviado = false,
            //    Correo = "laura@gmail.com",
            //    Alumno = "Laura Martinez",
            //    Calificacion = 80
            //};
            //lista.Add(registro);

            //dgvTabla.AutoGenerateColumns = false;
            //dgvTabla.DataSource = lista;

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //llenar un objeto con los datos
            EnvioCorreo registro = new EnvioCorreo(
                cboEnviado.SelectedItem.ToString().Equals("NO")?false:true,
                txtCorreo.Text.ToLower(),
                txtNombre.Text,
                double.Parse(txtCalificacion.Text)
                );

            //añadirlo a la lista
            lista.Add(registro);
            cargarLista();
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvTabla.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una fila a eliminar");
            }
            else
            {
                String alumno = dgvTabla.SelectedRows[0].Cells[2].Value.ToString();
                DialogResult respuesta = MessageBox.Show("¿Está apunto de elimnar al alumno " + alumno +
                    " ¿Está seguro que desea continuar?", "Confirmar eliminación",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                String correo = dgvTabla.SelectedRows[0].Cells[1].Value.ToString();
                if (respuesta == DialogResult.Yes)
                {
                    lista.Remove(new EnvioCorreo() { Correo = correo });
                    cargarLista();

                }
            }
        }

        public void cargarLista()
        {
            dgvTabla.DataSource = null;
            dgvTabla.DataSource = lista;
            foreach (DataGridViewColumn columna in dgvTabla.Columns)
            {
                columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.StreamReader archivo = new System.IO.StreamReader(openFileDialog1.FileName);
                    String[] datos = new string[3];
                    String linea;
                    while ((linea = archivo.ReadLine()) != null)
                    {
                        int c1 = linea.IndexOf(','), c2 = linea.LastIndexOf(','), c3 = linea.Length;

                        //el .trim es para quitar los espacios en blanco al inicio y final de las cadenas
                        datos[0] = linea.Substring(0, c1).Trim();
                        datos[1] = linea.Substring(c1 + 1, c2 - c1 - 1).Trim();
                        datos[2] = linea.Substring(c2 + 1, c3 - c2 - 1).Trim();

                        //lo de arriba se puede sustituir por: datos = linea.Split(','); pero agrega especios al inicio

                        EnvioCorreo registro = new EnvioCorreo(false, datos[0].ToLower(), datos[1], Int32.Parse(datos[2]));
                        lista.Add(registro);
                        cargarLista();
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    MessageBox.Show("No se encontró el archivo especificado", "Archivo No Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEnviarCorreos_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("¿Quiere enviar correos a los correos registrados?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    string email = Convert.ToString(dgvTabla.Rows[i].Cells[1].Value); //lista.ElementAt(i).Enviado = true;
                    try
                    {
                        //información de https://www.kyocode.com/2019/08/como-enviar-correo-con-c/#:~:text=La%20forma%20de%20como%20enviar,ser%20utilizado%20por%20otras%20aplicaciones
                        MailMessage correo = new MailMessage();
                        correo.From = new MailAddress("pruebavisualstudio01@gmail.com", "PruebaVisual", System.Text.Encoding.UTF8);//Correo de salida
                        correo.To.Add(email); //Correo destino?
                        correo.Subject = "Correo de prueba"; //Asunto
                        correo.Body = "Este es un correo de prueba enviado por johann tristán desde visual studio"; //Mensaje del correo
                        correo.IsBodyHtml = true;
                        correo.Priority = MailPriority.Normal;
                        SmtpClient smtp = new SmtpClient();
                        smtp.UseDefaultCredentials = false;
                        smtp.Host = "smtp.gmail.com"; //Host del servidor de correo
                        smtp.Port = 25; //Puerto de salida
                        smtp.Credentials = new System.Net.NetworkCredential("pruebavisualstudio01@gmail.com", "ositrjgdpdaeszst");//Cuenta de correo
                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        smtp.EnableSsl = true;//True si el servidor de correo permite ssl
                        smtp.Send(correo);

                        
                    }
                    catch (System.IO.IOException)
                    {
                        MessageBox.Show("Ha habido un error al enviar el correo electrónico " + email, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}