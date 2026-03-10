using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using EP_CarlosAndresRuizMiranda.Models;

namespace EP_CarlosAndresRuizMiranda.Controllers
{
    public class LibrosController : Controller
    {
        private readonly string cadenaConexion = "Server=(localdb)\\AndresRuiz;Database=Biblioteca;User Id=AndresRuiz;Password=12345;TrustServerCertificate=true";

        public IActionResult Index()
        {
            var listaLibros = obtenerLibros();
            return View(listaLibros);
        }

        public IActionResult Create()
        {
            var categorias = obtenerCategorias();
            ViewBag.Categorias = new SelectList(categorias, "ID", "Nombre");
            var autores = obtenerAutores();
            ViewBag.Autores = new SelectList(autores, "ID", "Nombre");
            return View(new Libro());
        }

        [HttpPost]
        public IActionResult Create(Libro libro)
        {
            var exito = CrearLibro(libro);
            return RedirectToAction("Index");
        }

        #region . Private methods .

        private List<Libro> obtenerLibros()
        {
            var listaLibros = new List<Libro>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("select L.Titulo, L.Publicacion, L.IdAutor, L.IdCategoria, A.Nombre, C.Nombre from Libros L INNER JOIN Autores A on L.IdAutor = A.Id INNER JOIN Categorias C on L.IdCategoria = C.Id;", conexion))
                {
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            while (lector.Read())
                            {
                                listaLibros.Add(convertirReaderEnLibro(lector));
                            }
                        }
                    }
                }
            }
            return listaLibros;
        }

        private Libro obtenerLibroPorId(int id)
        {
            var libro = new Libro();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("select L.Titulo, L.Publicacion, L.IdAutor, L.IdCategoria, A.Nombre, C.Nombre from Libros L INNER JOIN Autores A on L.IdAutor = A.Id INNER JOIN Categorias C on L.IdCategoria = C.Id WHERE L.ID = @ID", conexion))
                {
                    comando.Parameters.AddWithValue("@ID", id);
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            lector.Read();
                            libro = convertirReaderEnLibro(lector);
                        }
                    }
                }
            }

            return libro;
        }

        private List<Categoria> obtenerCategorias()
        {
            var listaCategorias = new List<Categoria>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("SELECT * FROM Categorias", conexion))
                {
                    conexion.Open();
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                                listaCategorias.Add(convertirReaderEnCategoria(reader));
                        }
                    }
                }
            }
            return listaCategorias;
        }

        private List<Autor> obtenerAutores()
        {
            var listaAutores = new List<Autor>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("SELECT * FROM Autores", conexion))
                {
                    conexion.Open();
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                                listaAutores.Add(convertirReaderEnAutor(reader));
                        }
                    }
                }
            }
            return listaAutores;
        }

        private bool CrearLibro(Libro libro)
        {
            var exito = false;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("INSERT INTO Libros(Titulo, Publicacion, IdAutor, IdCategoria) " +
                    "VALUES(@titulo,@publicacion,@autor,@categoria)", conexion))
                {
                    comando.Parameters.AddWithValue("@titulo", libro.Titulo);
                    comando.Parameters.AddWithValue("@publicacion", libro.Publicacion);
                    comando.Parameters.AddWithValue("@autor", libro.IdAutor);
                    comando.Parameters.AddWithValue("@categoria", libro.IdCategoria);
                    conexion.Open();
                    exito = comando.ExecuteNonQuery() > 0;
                }
            }
            return exito;
        }

        private Libro convertirReaderEnLibro(SqlDataReader lector)
        {
            return new Libro()
            {
                Titulo = lector.GetString(0),
                Publicacion = lector.GetInt32(1),
                IdAutor = lector.GetInt32(2),
                Autor = new Autor()
                {
                    ID = lector.GetInt32(2),
                    Nombre = lector.GetString(4)
                },
                IdCategoria = lector.GetInt32(3),
                Categoria = new Categoria()
                {
                    ID = lector.GetInt32(3),
                    Nombre = lector.GetString(5)
                }
            };
        }

        private Autor convertirReaderEnAutor(SqlDataReader lector)
        {
            return new Autor
            {
                ID = lector.GetInt32(0),
                Nombre = lector.GetString(1)
            };
        }

        private Categoria convertirReaderEnCategoria(SqlDataReader lector)
        {
            return new Categoria
            {
                ID = lector.GetInt32(0),
                Nombre = lector.GetString(1)
            };
        }        

        #endregion
    }
}
