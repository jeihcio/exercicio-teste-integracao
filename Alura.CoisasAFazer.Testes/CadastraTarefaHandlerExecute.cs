using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Moq;

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
            var repo = new RepositorioFake();
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

            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(x => x.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(new Exception("Houve um erro na inclus�o de tarefas"));

            var repo = mock.Object;
            var handler = new CadastraTarefaHandler(repo);

            // act
            CommandResult resultado = handler.Execute(comando);

            // assert
            Assert.False(resultado.IsSuccess);
        }
    }
}
