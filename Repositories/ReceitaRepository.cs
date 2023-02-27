﻿using AutoMapper;
using ChallengeBackend4EdicaoAlura.Context;
using ChallengeBackend4EdicaoAlura.Dtos.Receitas;
using ChallengeBackend4EdicaoAlura.Interfaces;
using ChallengeBackend4EdicaoAlura.Models;
using ChallengeBackend4EdicaoAlura.Util;

namespace ChallengeBackend4EdicaoAlura.Repositories
{
    public class ReceitaRepository : IReceitaRepository
    {

        private readonly DataBaseContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidacao _validacao;
        public ReceitaRepository(DataBaseContext dbContext, IMapper Mapper, IValidacao validacao)
        {
            _dbContext = dbContext;
            _mapper = Mapper;
            _validacao = validacao;
        }

        public ReceitaModel AddReceita(PostReceitaDto postReceitaDto)
        {
            var receita = _mapper.Map<ReceitaModel>(postReceitaDto);

            _validacao.ValidaSeJaExisteNoBancoDeDados<ReceitaModel>(receita.Descricao, receita.Data);

            _dbContext.Add(receita);
            _dbContext.SaveChanges();
            return receita;
        }

        public void DeleteReceita(int id)
        {
            var receita = _dbContext.Receitas.Find(id);
            _validacao.ValidaSeAEntidadeENula<ReceitaModel>(id);
            _dbContext.Remove(receita);
            _dbContext.SaveChanges();
        }

        public List<ReadReceitaDto> GetReceitaByDate(int ano, int mes)
        {
            var receitasPorData = _dbContext.Receitas.Where(d =>
            d.Data.Year == ano && d.Data.Month == mes).ToList();

            var dtoReceitasPorData = _mapper.Map<List<ReadReceitaDto>>(receitasPorData);

            return dtoReceitasPorData;
        }

        public List<ReadReceitaDto> GetReceitaByDescricao(string descricao)
        {
            var receitasComPalavraChaveNaDescricao = _dbContext.Receitas
                .Where(d => d.Descricao.ToUpper()
                .Contains(descricao.ToUpper())).ToList();

            var dtoreceitasComPalavraChaveNaDescricao = _mapper.Map<List<ReadReceitaDto>>(receitasComPalavraChaveNaDescricao);

            return dtoreceitasComPalavraChaveNaDescricao.ToList();
        }

        public ReadReceitaDto GetReceitaById(int id)
        {
            var receita = _dbContext.Receitas.Find(id);
            _validacao.ValidaSeAEntidadeENula<ReceitaModel>(id);
            var readReceitaDto = _mapper.Map<ReadReceitaDto>(receita);
            return readReceitaDto;
        }

        public List<ReadReceitaDto> GetReceitas()
        {
            var receitas = _dbContext.Receitas.ToList();
            var dtoReadReceitas = _mapper.Map<List<ReadReceitaDto>>(receitas);
            return dtoReadReceitas;
        }
        public void PutReceita(int id, PutReceitaDto putReceitaDto)
        {
            var receita = _dbContext.Receitas.Find(id);
            _validacao.ValidaSeAEntidadeENula<ReceitaModel>(id);

            _validacao.ValidaSeJaExisteNoBancoDeDados<ReceitaModel>(putReceitaDto.Descricao, putReceitaDto.Data);

            _mapper.Map(putReceitaDto, receita);
            _dbContext.SaveChanges();
        }

    }
}
