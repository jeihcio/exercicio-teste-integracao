using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Services.Handlers;
using System;
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

            var repo = new RepositorioFake();
            var handler = new CadastraTarefaHandler(repo);

            // act
            handler.Execute(comando); // SUT >> CadastraTarefaHandlerExecute

            // assert

        }
    }
}
