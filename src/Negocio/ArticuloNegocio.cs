using Datos;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Traer todos los artículos
                datos.setearConsulta("SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.Precio, " +
                    "M.Id AS IdMarca, M.Descripcion AS Marca, " +
                    "C.Id AS IdCategoria, C.Descripcion AS Categoria " +
                    "FROM ARTICULOS A, MARCAS M, CATEGORIAS C WHERE M.Id = A.IdMarca AND C.Id = A.IdCategoria");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = datos.Lector["Descripcion"] != DBNull.Value ? (string)datos.Lector["Descripcion"] : null;
                    aux.Precio = (Decimal)datos.Lector["Precio"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    lista.Add(aux);
                }

                // Traer todas las imagenes
                ImagenNegocio imagenNegocio = new ImagenNegocio();
                List<Imagen> listaImagenes = imagenNegocio.Listar();

                // Asociar las imagenes a cada articulo
                foreach(Articulo articulo in lista)
                {
                    articulo.Imagenes = listaImagenes.FindAll(imagen => imagen.IdArticulo == articulo.Id);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Agregar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            ImagenNegocio imagenNegocio = new ImagenNegocio();

            try
            {
                // Agregar artículo
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria) OUTPUT INSERTED.Id VALUES (@codigo, @nombre, @descripcion, @precio, @idMarca, @idCategoria)");
                datos.setearParametro("@codigo", articulo.Codigo);
                datos.setearParametro("@nombre", articulo.Nombre);
                datos.setearParametro("@descripcion", articulo.Descripcion);
                datos.setearParametro("@precio", articulo.Precio);
                datos.setearParametro("@idMarca", articulo.Marca.Id);
                datos.setearParametro("@idCategoria", articulo.Categoria.Id);

                // Obtener el id del artículo insertado
                articulo.Id = (int)datos.ejecutarScalar();

                // Agregar las imagenes asociadas al artículo
                foreach (Imagen imagen in articulo.Imagenes)
                {
                    imagen.IdArticulo = articulo.Id;

                    imagenNegocio.Agregar(imagen);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Modificar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            ImagenNegocio imagenNegocio = new ImagenNegocio();

            try
            {
                // Modificar artículo
                datos.setearConsulta("UPDATE ARTICULOS SET Codigo=@codigo, Nombre=@nombre, Descripcion=@descripcion, Precio=@precio, IdMarca=@idMarca, IdCategoria=@idCategoria WHERE Id=@id");
                datos.setearParametro("@id", articulo.Id);
                datos.setearParametro("@codigo", articulo.Codigo);
                datos.setearParametro("@nombre", articulo.Nombre);
                datos.setearParametro("@descripcion", articulo.Descripcion);
                datos.setearParametro("@precio", articulo.Precio);
                datos.setearParametro("@idMarca", articulo.Marca.Id);
                datos.setearParametro("@idCategoria", articulo.Categoria.Id);

                datos.ejecutarAccion();

                // Modificar imagenes asociadas al artículo
                List<Imagen> listaImagenes = imagenNegocio.ListarPorArticulo(articulo.Id);

                // Eliminar imagenes que no esten dentro de la lista de imagenes del articulo
                foreach (Imagen imagen in listaImagenes)
                {
                    if (!articulo.Imagenes.Any(i => i.Id == imagen.Id))
                    {
                        imagenNegocio.Eliminar(imagen.Id);
                    }
                }

                // Agregar nuevas / modificar existentes
                foreach (Imagen img in articulo.Imagenes)
                {
                    img.IdArticulo = articulo.Id;

                    if (img.Id == 0)
                    {
                        imagenNegocio.Agregar(img);
                    }
                    else
                    {
                        imagenNegocio.Modificar(img);
                    }
                }
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Eliminar(int id)
        {
            // Eliminar imagenes asociadas al artículo
            ImagenNegocio imagenNegocio = new ImagenNegocio();

            foreach (Imagen img in imagenNegocio.ListarPorArticulo(id))
            {
                imagenNegocio.Eliminar(img.Id);
            }

            // liminar artículo
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM ARTICULOS WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
