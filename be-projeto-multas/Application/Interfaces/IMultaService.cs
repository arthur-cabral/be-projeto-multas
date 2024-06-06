using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMultaService
    {
        public Task<List<MultaDTO>> GetAllMultas();
        public Task<MultaDTO> GetMultaById(long id);
        public Task<MultaDTO> PostMulta(MultaDTO multaDTO);
        public Task<MultaDTO> PutMultaById(MultaDTO multaDTO);
        public Task DeleteMultaById(long id);
    }
}
