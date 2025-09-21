using Microsoft.AspNetCore.Mvc;
using VendasApi.Data;

namespace VendasApi.Services
{
    public interface IClienteService
    {
        Task<int> CreateClienteAsync(string nome, string email);
        Task<int> DeleteClienteAsync(int id);
        Task<int> UpdateClienteAsync(int id, string nome, string email);
        Task<Models.Cliente> GetClienteByIdAsync(int id);
        Task<IActionResult> GetAllClientesAsync();
    }
}
    
