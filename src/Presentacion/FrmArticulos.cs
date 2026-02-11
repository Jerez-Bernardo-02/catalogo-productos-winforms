using Dominio;
using Negocio;
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
    public partial class FrmArticulos : Form
    {
        // ---------- Campos privados ----------
        private List<Articulo> listaArticulos;
        private List<Imagen> listaImagenes;

        private Articulo articuloActual;
        private int indiceImagenActual;

        // ---------- Inicialización ----------
        public FrmArticulos()
        {
            InitializeComponent();
        }

        private void FrmArticulos_Load(object sender, EventArgs e)
        {
            try
            {
                CargarListaArticulos();
                CargarListaImagenes();
                AsociarImagenesPorArticulo();

                MostrarGrilla();

                OcultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // ---------- Carga de Datos ----------
        private void CargarListaArticulos()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            listaArticulos = negocio.Listar();
        }

        private void CargarListaImagenes()
        {
            ImagenNegocio negocio = new ImagenNegocio();
            listaImagenes = negocio.Listar();
        }

        private void AsociarImagenesPorArticulo()
        {
            if (listaArticulos == null || listaImagenes == null)
            {
                return;
            }

            foreach (Articulo articulo in listaArticulos)
            {
                articulo.Imagenes = listaImagenes.FindAll(img => img.IdArticulo == articulo.Id);
            }
        }

        // ---------- Eventos Principales ----------
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            CargarArticuloSeleccionado();

            if(articuloActual == null)
            {
                return;
            }

            MostrarDetalle();
            MostrarImagen();
            ActualizarBotonesImagen();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (indiceImagenActual <= 0)
            {
                return;
            }

            indiceImagenActual--;
            MostrarImagen();
            ActualizarBotonesImagen();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (indiceImagenActual >= articuloActual.Imagenes.Count - 1)
            {
                return;
            }

            indiceImagenActual++;
            MostrarImagen();
            ActualizarBotonesImagen();
        }

        // ---------- Eventos de UI ----------
        private void CargarArticuloSeleccionado()
        {
            if (dgvArticulos.CurrentRow == null)
            {
                articuloActual = null;
                return;
            }

            articuloActual = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            indiceImagenActual = 0;
        }

        private void MostrarGrilla()
        {
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaArticulos;
        }

        private void MostrarDetalle()
        {
            if(articuloActual == null)
            {
                return;
            }

            lblNombre.Text = articuloActual.Nombre;
            lblPrecio.Text = "$ " + articuloActual.Precio.ToString("N2");
            lblMarca.Text = articuloActual.Marca.Descripcion;
            lblCategoria.Text = articuloActual.Categoria.Descripcion;
            txtDescripcion.Text = articuloActual.Descripcion;
        }

        private void MostrarImagen()
        {
            try
            {
                pbxImagen.Load(articuloActual.Imagenes[indiceImagenActual].UrlImagen);
                // Si la URL es inválida, el sistema espera a que el
                // request HTTP falle (timeout) y recien ahi pasa al catch
            }
            catch
            {
                pbxImagen.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void ActualizarBotonesImagen()
        {
            if (articuloActual.Imagenes == null || articuloActual.Imagenes.Count <= 1)
            {
                btnAnterior.Enabled = false;
                btnSiguiente.Enabled = false;
                return;
            }

            btnAnterior.Enabled = indiceImagenActual > 0;
            btnSiguiente.Enabled = indiceImagenActual < articuloActual.Imagenes.Count - 1;
        }

        private void OcultarColumnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["Descripcion"].Visible = false;
            dgvArticulos.Columns["Precio"].Visible = false;
        }
    }
}
