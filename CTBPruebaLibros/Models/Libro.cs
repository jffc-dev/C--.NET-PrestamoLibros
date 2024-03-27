namespace CTBPruebaLibros.Models
{
    public class Libro
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string autor { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaPublicacion { get; set; }
        public string genero { get; set; }
        public string imagen { get; set; }
        public int cantidad { get; set; }
    }
}
