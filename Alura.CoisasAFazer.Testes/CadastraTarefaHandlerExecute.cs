using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

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

            var mock = new Mock<ILogger<CadastraTarefaHandler>>();

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;

            var contexto = new DbTarefasContext(options);
            var repo = new RepositorioFake();
            var handler = new CadastraTarefaHandler(repo, mock.Object);

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

            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();
            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(x => x.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(new Exception("Houve um erro na inclusão de tarefas"));

            var repo = mock.Object;
            var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

            // act
            CommandResult resultado = handler.Execute(comando);

            // assert
            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void QuandoExceptionForLancadaDeveLogarAMensagemDaExcecao()
        {
            // arrange
            var mensagemDeErroEsperada = "Houve um erro na inclusão de tarefas";
            var excecaoEsperada = new Exception(mensagemDeErroEsperada);

            var comando = new CadastraTarefa(
                "Executar Xunit",
                new Categoria("Estudo"),
                new DateTime(2019, 12, 31)
            );

            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();
            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(x => x.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(excecaoEsperada);

            var repo = mock.Object;
            var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

            // act
            CommandResult resultado = handler.Execute(comando);

            // assert
            mockLogger.Verify(x => x.Log(
                LogLevel.Error, //nível de log => LogError
                It.IsAny<EventId>(), //identificador do evento
                It.IsAny<object>(), //objeto que será logado
                excecaoEsperada, //exceção que será logada 
                It.IsAny<Func<object, Exception, string>>() // função que converte objeto+exceção >> string
            ), Times.Once());
        }
    }
}
