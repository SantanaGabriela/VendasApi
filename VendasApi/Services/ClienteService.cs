
using Microsoft.AspNetCore.Mvc;
using VendasApi.Data;

namespace VendasApi.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ClienteDbContext _context;

        public ClienteService(ClienteDbContext context)
        {
            _context = context;
        }
        public Task<int> CreateClienteAsync(string nome, string email)
        {
            var cliente = new Models.Cliente
            {
                Nome = nome,
                Email = email
            };
            _context.Clientes.Add(cliente);
            return _context.SaveChangesAsync().ContinueWith(t => cliente.Id);
        }

        public Task<int> DeleteClienteAsync(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
                return Task.FromResult(0);
            _context.Clientes.Remove(cliente);
            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateClienteAsync(int id, string nome, string email)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
                return Task.FromResult(0);
            cliente.Nome = nome;
            cliente.Email = email;
            _context.Clientes.Update(cliente);
            return _context.SaveChangesAsync();

        }

        public Task<Models.Cliente> GetClienteByIdAsync(int id)
        {
            var cliente = _context.Clientes.Find(id);
            return Task.FromResult(cliente);
        }

        public Task<IActionResult> GetAllClientesAsync()
        {
            var clientes = _context.Clientes.ToList();
            return Task.FromResult<IActionResult>(new OkObjectResult(clientes));
        }
    }
}
