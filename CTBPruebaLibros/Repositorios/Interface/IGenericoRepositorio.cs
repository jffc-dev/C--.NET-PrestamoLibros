namespace CTBPruebaLibros.Repositorios.Interface
{
    public interface IGenericoRepositorio<T> where T : class
    {
        Task<List<T>> Listar();
        Task<bool> Eliminar(int id);
        Task<bool> Guardar(T modelo);
        Task<bool> Editar(T modelo);
        Task<bool> Prestar(int id, DateTime fechaSalida);
    }
}
