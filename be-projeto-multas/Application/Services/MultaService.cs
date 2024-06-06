using Application.DTO;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MultaService : IMultaService
    {
        private readonly IMultaRepository _multaRepository;
        private readonly IMapper _mapper;

        public MultaService(IMultaRepository multaRepository, IMapper mapper)
        {
            _multaRepository = multaRepository;
            _mapper = mapper;
        }

        public async Task<List<MultaDTO>> GetAllMultas()
        {
            var multasEntity = await _multaRepository.GetAllMultas();
            return _mapper.Map<List<MultaDTO>>(multasEntity);
        }

        public async Task<MultaDTO> GetMultaById(long id)
        {
            try
            {
                var existsMultaById = await _multaRepository.ExistsMultaById(id);
                if (!existsMultaById)
                {
                    throw new NotFoundException("Multa de id " + id + " não encontrada");
                }
                var findMultaById = await _multaRepository.GetMultaById(id);
                return _mapper.Map<MultaDTO>(findMultaById);
            } 
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }

        public async Task<MultaDTO> PostMulta(MultaDTO multaDTO)
        {
            try
            {
                var existsMultaByAIT = await _multaRepository.ExistsMultaByAIT(multaDTO.NumeroAIT);
                if (existsMultaByAIT)
                {
                    throw new DuplicatedObjectException("Multa de AIT " + multaDTO.NumeroAIT + " já existe");
                }
                multaDTO.DataHoraInfracao = DateTime.UtcNow;
                var multaEntity = _mapper.Map<Multa>(multaDTO);
                var newMulta = await _multaRepository.PostMulta(multaEntity);
                return _mapper.Map<MultaDTO>(newMulta);
            }
            catch (DuplicatedObjectException ex)
            {
                throw new DuplicatedObjectException(ex.Message);
            }
        }

        public async Task<MultaDTO> PutMultaById(MultaDTO multaDTO)
        {
            try
            {
                var existsMultaById = await _multaRepository.ExistsMultaById(multaDTO.MultaId);
                if (!existsMultaById)
                {
                    throw new NotFoundException("Multa de id " + multaDTO.MultaId + " não encontrada");
                }
                var multaEntity = _mapper.Map<Multa>(multaDTO);
                multaEntity.MultaId = multaDTO.MultaId;
                var updatedMulta = await _multaRepository.PutMulta(multaEntity);
                return _mapper.Map<MultaDTO>(updatedMulta);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }

        public async Task DeleteMultaById(long id)
        {
            try
            {
                var existsMultaById = await _multaRepository.ExistsMultaById(id);
                if (!existsMultaById)
                {
                    throw new NotFoundException("Multa de id " + id + " não encontrada");
                }
                await _multaRepository.DeleteMultaById(id);
            } 
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }
    }
}
