using Microsoft.AspNetCore.Mvc;
using VendasApi.DTOs;
using VendasApi.Services;

namespace VendasApi.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }
        [HttpGet("api/clientes/")]
        public async Task<IActionResult> GetAllClientesAsync()
        {
            var clientes = await _clienteService.GetAllClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("api/clientes/{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null)
                return NotFound(new { Message = "Cliente não encontrado" });
            return Ok(cliente);
        }

        [HttpPost("api/clientes/")]
        public async Task<ActionResult> ClienteOrder([FromBody] ClienteDto clienteDto)
        {
            var clienteId = await _clienteService.CreateClienteAsync(clienteDto.Nome, clienteDto.Email);
            return Ok(new { Message = "Cliente criado com sucesso", ClienteId = clienteId });
        }
        [HttpPut("api/clientes/{id}")]
        public async Task<ActionResult> UpdateCliente(int id, [FromBody] ClienteDto clienteDto)
        {
            var result = await _clienteService.UpdateClienteAsync(id, clienteDto.Nome, clienteDto.Email);
            if (result == 0)
                return NotFound(new { Message = "Cliente não encontrado" });
            return Ok(new { Message = "Cliente atualizado com sucesso" });
        }
        [HttpDelete("api/clientes/{id}")]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            var result = await _clienteService.DeleteClienteAsync(id);
            if (result == 0)
                return NotFound(new { Message = "Cliente não encontrado" });
            return Ok(new { Message = "Cliente deletado com sucesso" });
        }
    }
}
