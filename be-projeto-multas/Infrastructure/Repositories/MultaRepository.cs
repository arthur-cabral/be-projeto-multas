using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Repositories
{
    public class MultaRepository : Repository<Multa>, IMultaRepository
    {
        public MultaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Multa>> GetAllMultas()
        {
            return await Get().OrderByDescending((x) => x.DataHoraInfracao).ToListAsync();
        }

        public async Task<Multa> GetMultaByAIT(string ait)
        {
            return await GetByProperty((x) => x.NumeroAIT == ait);
        }

        public async Task<Multa> GetMultaById(long id)
        {
            return await GetByProperty((x) => x.MultaId == id);
        }

        public async Task<bool> ExistsMultaById(long id)
        {
            return await ExistsByProperty((x) => x.MultaId == id);
        }

        public async Task<bool> ExistsMultaByAIT(string ait)
        {
            return await ExistsByProperty((x) => x.NumeroAIT == ait);
        }

        public async Task<Multa> PostMulta(Multa multa)
        {
            Add(multa);
            await _context.SaveChangesAsync();
            return multa;
        }

        public async Task<Multa> PutMulta(Multa multa)
        {
            Update(multa);
            await _context.SaveChangesAsync();
            return multa;
        }

        public async Task DeleteMultaById(long id)
        {
            Multa multa = await GetByProperty((x) => x.MultaId == id);
            Delete(multa);
            await _context.SaveChangesAsync();
        }
    }
}
