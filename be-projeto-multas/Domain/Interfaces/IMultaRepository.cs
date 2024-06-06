using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMultaRepository : IRepository<Multa>
    {

        public Task<List<Multa>> GetAllMultas();
        public Task<Multa> GetMultaById(long id);
        public Task<Multa> GetMultaByAIT(string ait);
        public Task<bool> ExistsMultaById(long id);
        public Task<bool> ExistsMultaByAIT(string ait);
        public Task<Multa> PostMulta(Multa multa);
        public Task<Multa> PutMulta(Multa multa);
        public Task DeleteMultaById(long id);
    }
}
