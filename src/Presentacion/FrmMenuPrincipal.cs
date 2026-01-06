using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class FrmMenuPrincipal : Form
    {
        private Form formularioActual;

        public FrmMenuPrincipal()
        {
            InitializeComponent();
        }

        private void FrmMenuPrincipal_Load(object sender, EventArgs e)
        {
            abrirFormulario(new FrmArticulos());
        }

        private void articulosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FrmArticulos());
        }

        private void categoriasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FrmCategorias());
        }

        private void marcasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FrmMarcas());
        }

        private void abrirFormulario(Form formulario)
        {
            // Verifica si hay un formulario abierto en el panel
            if (formularioActual != null)
            {
                // No se vuelve a cargar el mismo formulario si esta abierto
                if (formularioActual.GetType() == formulario.GetType())
                {
                    formulario.Dispose();
                    return;
                }

                formularioActual.Dispose();
            }

            formularioActual = formulario;

            panelContenedor.Controls.Clear(); // Limpia el panel, quitando cualquier control anterior (En este caso un Form)

            formulario.TopLevel = false; // Indica que el formulario no es una ventana independiente
            formulario.FormBorderStyle = FormBorderStyle.None; // Elimina bordes, título y botones de ventana
            formulario.Dock = DockStyle.Fill; // Hace que el formulario ocupe todo el espacio disponible del panel

            panelContenedor.Controls.Add(formulario); // Agrega el formulario al panel como si fuera un control más
            formulario.Show();
        }
    }
}
