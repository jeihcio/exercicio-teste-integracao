using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Alura.CoisasAFazer.Testes
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void DadaTarefaComInfoValidasDeveIncluirNoDB()
        {
            // arrange
            var comando = new CadastraTarefa(
                "Executar Xunit",
                new Categoria("Estudo"),
                new DateTime(2019, 12, 31)
            );

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;

            var contexto = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(contexto);
            var handler = new CadastraTarefaHandler(repo);

            // act
            handler.Execute(comando); // SUT >> CadastraTarefaHandlerExecute

            // assert
            var tarefa = repo
                .ObtemTarefas(x => x.Titulo == "Executar Xunit")
                .FirstOrDefault();

            Assert.NotNull(tarefa);
        }

        [Fact]
        public void QuandoExceptionForLancadaResultadoIsSuccessDeveSerFalse()
        {
            // arrange
            var comando = new CadastraTarefa(
                "Executar Xunit",
                new Categoria("Estudo"),
                new DateTime(2019, 12, 31)
            );

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
               .UseInMemoryDatabase("DbTarefasContext")
               .Options;

            var contexto = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(contexto);
            var handler = new CadastraTarefaHandler(repo);

            // act
            CommandResult resultado = handler.Execute(comando);

            // assert
            Assert.False(resultado.IsSuccess);
        }
    }
}
