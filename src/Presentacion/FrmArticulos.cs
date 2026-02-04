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
        private List<Articulo> listaArticulos;
        private List<Imagen> listaImagenes;

        public FrmArticulos()
        {
            InitializeComponent();
        }

        private void FrmArticulos_Load(object sender, EventArgs e)
        {
            try
            {
                cargarDatos();
                cargarGrilla();
                ocultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); // Error al cargar los datos
            }

        }

        private void cargarDatos()
        {
            cargarListaArticulos();
            cargarListaImagenes();
            cargarImagenesPorArticulo();
        }

        private void cargarListaArticulos()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            listaArticulos = negocio.Listar();
        }

        private void cargarListaImagenes()
        {
            ImagenNegocio negocio = new ImagenNegocio();

            listaImagenes = negocio.Listar();
        }

        private void cargarImagenesPorArticulo()
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

        private void cargarGrilla()
        {
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaArticulos;
        }

        private void cargarDetalle()
        {
            if (dgvArticulos.CurrentRow == null)
            {
                return;
            }

            Articulo seleccionado = dgvArticulos.CurrentRow.DataBoundItem as Articulo;

            if (seleccionado == null)
            {
                return;
            }

            lblNombre.Text = (string)seleccionado.Nombre;
            lblPrecio.Text = "$ " + seleccionado.Precio.ToString("N2");
            lblMarca.Text = (string)seleccionado.Marca.Descripcion;
            lblCategoria.Text = (string)seleccionado.Categoria.Descripcion;
            txtDescripcion.Text = (string)seleccionado.Descripcion;

            cargarImagen(seleccionado.Imagenes[0].UrlImagen);
        }

        private void cargarImagen(string imagenUrl)
        {
            try
            {
                pbxImagen.Load(imagenUrl);
                // Si la URL es inválida, el sistema espera a que el
                // request HTTP falle (timeout) y recien ahi pasa al catch
            }
            catch
            {
                pbxImagen.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            cargarDetalle();
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["Descripcion"].Visible = false;
            dgvArticulos.Columns["Precio"].Visible = false;
        }
    }
}
