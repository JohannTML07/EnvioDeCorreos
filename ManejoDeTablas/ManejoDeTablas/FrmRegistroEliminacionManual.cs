using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManejoDeTablas
{
    public partial class FrmRegistroEliminacionManual : Form
    {
        public FrmRegistroEliminacionManual()
        {
            InitializeComponent();
            
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            dgvTabla.Rows.Add(cboEnviado.SelectedIndex, txtCorreo.Text,
                txtNombre.Text, txtCalificacion.Text);
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
                DialogResult respuesta= MessageBox.Show("¿Está apunto de elimnar al alumno " + alumno +
                    " ¿Está seguro que desea continuar?", "Confirmar eliminación",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (respuesta == DialogResult.Yes)
                {
                    dgvTabla.Rows.Remove(dgvTabla.SelectedRows[0]);
                }
            }
        }
    }
}
