using LanchesMac.Context;
using LanchesMac.Models;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Areas.Admin.Servicos
{
    public class RelatorioVendasService
    {
        private readonly AppDbContext context;
        public RelatorioVendasService(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<List<Pedido>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultado = from obj in context.Pedidos select obj;

            minDate?.ToUniversalTime();
            maxDate?.ToUniversalTime();

            if (minDate.HasValue)
            {
               resultado = resultado.Where(x => x.PedidoEnviado.ToUniversalTime() >= minDate.Value.ToUniversalTime());
             //  minDate?.ToUniversalTime();
            }

            if (maxDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado.ToUniversalTime() <= maxDate.Value.ToUniversalTime());
               // maxDate?.ToUniversalTime();
            }

            return await resultado
                         .Include(l => l.PedidoItens)
                         .ThenInclude(l => l.Lanche)
                         .OrderByDescending(x => x.PedidoEnviado)
                         .ToListAsync();
        }
    }
}
