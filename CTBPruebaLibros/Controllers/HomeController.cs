using CTBPruebaLibros.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CTBPruebaLibros.Repositorios.Interface;
using System;
using System.Dynamic;
using System.Text.Json;

namespace CTBPruebaLibros.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericoRepositorio<Libro> _libroRepositorio;

        public HomeController(ILogger<HomeController> logger, IGenericoRepositorio<Libro> libroRepositorio)
        {
            _logger = logger;
            _libroRepositorio = libroRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListarLibros()
        {
            List<Libro> _listaLibros = await _libroRepositorio.Listar();
            return StatusCode(StatusCodes.Status200OK, _listaLibros);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarLibro([FromBody] Libro libro)
        {
            bool _respuesta = await _libroRepositorio.Guardar(libro);
            if (_respuesta)
            {
                return StatusCode(StatusCodes.Status200OK, new {resultado = _respuesta });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { resultado = _respuesta });
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarLibro([FromBody] Libro libro)
        {
            bool _respuesta = await _libroRepositorio.Editar(libro);
            if (_respuesta)
            {
                return StatusCode(StatusCodes.Status200OK, new { resultado = _respuesta });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { resultado = _respuesta });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarLibro(int idLibro)
        {
            try
            {
                bool _respuesta = await _libroRepositorio.Eliminar(idLibro);
                if (_respuesta)
                {
                    return StatusCode(StatusCodes.Status200OK, new { resultado = _respuesta });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { resultado = _respuesta });
                }
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { resultado = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PrestarLibro([FromBody] dynamic prestamo)
        {
            string jsonString = JsonSerializer.Serialize(prestamo);
            JsonDocument jsonDoc = JsonDocument.Parse(jsonString);
            JsonElement root = jsonDoc.RootElement;
            int idLibro = root.GetProperty("id").GetInt32();

            bool _respuesta = await _libroRepositorio.Prestar(idLibro, DateTime.Now);
            if (_respuesta)
            {
                return StatusCode(StatusCodes.Status200OK, new { resultado = _respuesta });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { resultado = _respuesta });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}