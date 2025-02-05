﻿using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Core.Result;
using Microsoft.Extensions.Logging;

namespace Fidelicard.Campanha.Core.Service
{
    public class CampanhaService : ICampanhaService
    {
        private readonly ICampanhaRepository _campanhaRepository;
        private readonly ILogger<CampanhaService> _logger;

        public CampanhaService(ILogger<CampanhaService> logger
        , ICampanhaRepository usuarioRepository)
        {
            _logger = logger;
            _campanhaRepository = usuarioRepository;
        }
        public async Task<CampanhaResult> ConsultarCampanhaAsync(int idCampanha)
        {
            _logger.LogInformation("Iniciando consulta de campanha com Id: {IdCampanha}", idCampanha);

            try
            {
                var usuario = await _campanhaRepository.ObterCampanhaAsync(idCampanha).ConfigureAwait(false);

                if (usuario == null)
                {
                    var mensagem = $"Campanha inexistente pelo código informado: {idCampanha}";
                    _logger.LogWarning(mensagem);
                    return CampanhaResult.DadosInvalidos(new Exception(mensagem), mensagem);
                }

                _logger.LogInformation("Consulta do usuário com Id: {IdCampanha} concluída com sucesso.", idCampanha);
                return CampanhaResult.SucessoObterUsuario(usuario.Campanha);
            }
            catch (Exception ex)
            {
                var mensagemErro = $"Erro ao consultar o usuário com Id: {idCampanha}.";
                _logger.LogError(ex, mensagemErro);
                return CampanhaResult.ErroObterCampanha(ex, mensagemErro);
            }
        }

        public async Task<CampanhaResult> ConsultarCampanhasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            _logger.LogInformation("Iniciando consulta de campanhas no período de {DataInicio} a {DataFim}", dataInicio, dataFim);

            try
            {
                // Consulta as campanhas no repositório
                var campanhas = await _campanhaRepository.ObterCampanhasPorPeriodoAsync(dataInicio, dataFim).ConfigureAwait(false);

                if (campanhas == null || !campanhas.Any())
                {
                    var mensagem = $"Nenhuma campanha encontrada no período de {dataInicio:yyyy-MM-dd} a {dataFim:yyyy-MM-dd}.";
                    _logger.LogWarning(mensagem);
                    return CampanhaResult.DadosInvalidos(new Exception(mensagem), mensagem);
                }

                _logger.LogInformation("Consulta de campanhas no período concluída com sucesso.");
                return CampanhaResult.SucessoObterUsuarioPorPeriodo(campanhas);
            }
            catch (Exception ex)
            {
                var mensagemErro = $"Erro ao consultar campanhas no período de {dataInicio:yyyy-MM-dd} a {dataFim:yyyy-MM-dd}.";
                _logger.LogError(ex, mensagemErro);
                return CampanhaResult.ErroObterCampanha(ex, mensagemErro);
            }
        }


        public async Task<int> CadastrarCampanhaAsync(Campanhas campanha)
        {
            _logger.LogInformation("Iniciando cadastro de campanha: {CampanhaNome}", campanha?.Nome);

            try
            {
                var result = await _campanhaRepository.CadastrarCampanhaAsync(campanha).ConfigureAwait(false);

                _logger.LogInformation("Campanha cadastrada com sucesso. Id gerado: {CampanhaId}", result);
                return result;
            }
            catch (Exception ex)
            {
                var mensagemErro = "Erro ao cadastrar o campanha.";
                _logger.LogError(ex, mensagemErro);
                throw new ApplicationException(mensagemErro, ex);
            }
        }

        public async Task<int> AtualizarCampanhaAsync(Campanhas campanha)
        {
            _logger.LogInformation("Iniciando cadastro da campanha: {CampanhaNome}", campanha?.Nome);

            try
            {
                var result = await _campanhaRepository.AtualizarCampanhaAsync(campanha).ConfigureAwait(false);

                _logger.LogInformation("Campanha atualizado com sucesso. Id gerado: {CampanhaId}", result);
                return result;
            }
            catch (Exception ex)
            {
                var mensagemErro = "Erro ao atualizar o campanha.";
                _logger.LogError(ex, mensagemErro);
                throw new ApplicationException(mensagemErro, ex);
            }
        }        
    }
}
