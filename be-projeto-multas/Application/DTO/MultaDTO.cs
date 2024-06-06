using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class MultaDTO
    {
        public long MultaId { get; set; }
        public string NumeroAIT { get; set; }
        public DateTime DataHoraInfracao { get; set; }
        public string CodigoInfracao { get; set; }
        public string PlacaVeiculo { get; set; }
    }
}
