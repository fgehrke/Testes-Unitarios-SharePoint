using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using BLL;

namespace BLL.Test
{
    [TestClass]
    public class PessoaTest
    {
        public readonly IRepositorioPessoas MockRepositorioPessoa;

        public PessoaTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>
            {
                new Pessoa {ID = 1, Nome = "Pedro de Lara", Endereco = "Rua Sem Nome, Nr 1", DataNascimento = new DateTime(1990, 01, 01)},
                new Pessoa {ID = 2, Nome = "Carlos Lara da Silva", Endereco = "Rua Sem Nome, Nr 2", DataNascimento = new DateTime(1980, 12, 23)},
                new Pessoa {ID = 3, Nome = "João Cunha", Endereco = "Rua Sem Nome, Nr 3", DataNascimento = new DateTime(1989, 02, 02)}
            };

            // Criação do Mock utilizando Moq
            Mock<IRepositorioPessoas> mockProductRepository = new Mock<IRepositorioPessoas>();

            // Nas linhas abaixo os métodos estão sendo configurados no mock

            // Retorna todas as pessoas
            mockProductRepository.Setup(mr => mr.ObterTodos()).Returns(pessoas);

            // Retorna a pessoa pelo ID
            mockProductRepository.Setup(mr => mr.ObterPorID(
                It.IsAny<int>())).Returns((int i) => pessoas.Where(
                p => p.ID == i).Single());

            // Retorna as pessoas pelo filtro do nome
            mockProductRepository.Setup(mr => mr.ObterPorFiltro(
                It.IsAny<string>())).Returns((string i) => pessoas.Where(
                p => p.Nome.Contains(i)).ToList());

            // Exclui a pessoa com ID passado por parâmetro
            mockProductRepository.Setup(mr => mr.Excluir(It.IsAny<int>())).Returns(
                (int id) =>
                {
                    Pessoa pessoa = pessoas.Where(p => p.ID == id).Single();
                    pessoas.Remove(pessoa);

                    return true;
                });

            // Salva (insere ou altera) uma pessoa
            mockProductRepository.Setup(mr => mr.Salvar(It.IsAny<Pessoa>())).Returns(
                (Pessoa pessoa) =>
                {
                    DateTime now = DateTime.Now;

                    if (pessoa.ID.Equals(default(int)))
                    {
                        // Insere a pessoa nova
                        pessoa.ID  = pessoas.Count() + 1;
                        pessoas.Add(pessoa);

                        return pessoa;
                    }
                    else
                    {
                        // Atualiza a pessoa existente
                        var original = pessoas.Where(
                            q => q.ID == pessoa.ID).Single();

                        if (original == null)
                        {
                            return null;
                        }

                        original.Nome = pessoa.Nome;
                        original.Endereco = pessoa.Endereco;
                        original.DataNascimento = pessoa.DataNascimento;

                        return original;
                    }
                });

            // Finaliza a configuração do mock
            this.MockRepositorioPessoa = mockProductRepository.Object;
        }

        /// <summary>
        /// Verifica se todos as pessoas foram retornadas
        /// </summary>
        [TestMethod]
        public void PodeRetornarTodasPessoas()
        {
            // Obtém todas as pessoas
            IList<Pessoa> pessoas = this.MockRepositorioPessoa.ObterTodos();

            Assert.IsNotNull(pessoas); // Teste se é nulo
            Assert.AreEqual(3, pessoas.Count); // Verifica o número de pessoas retornadas
        }

        /// <summary>
        /// Verifica se todos se a pessoa foi retornada a partir do ID
        /// </summary>
        [TestMethod]
        public void PodeRetornarPessoaPorID()
        {
            // Obtém a pessoa com ID 1
            Pessoa pessoa = this.MockRepositorioPessoa.ObterPorID(1);

            Assert.IsNotNull(pessoa); // Teste se é nulo
            Assert.AreEqual(1, pessoa.ID); // Verifica a pessoa retornada é a correta
        }

        /// <summary>
        /// Verifica se todos as pessoas foram retornadas pelo filtro
        /// </summary>
        [TestMethod]
        public void PodeRetornarObterPorFiltro()
        {
            string nome = "Lara";

            // Obtém a pessoa com ID 1
            List<Pessoa> pessoas = this.MockRepositorioPessoa.ObterPorFiltro(nome);

            Assert.IsNotNull(pessoas); // Teste se é nulo
            Assert.AreEqual(2, pessoas.Count); // Verifica a pessoa retornada é a correta
            Assert.AreEqual(true, pessoas[0].Nome.Contains(nome)); // Verifica a pessoa retornada é a correta
            Assert.AreEqual(true, pessoas[1].Nome.Contains(nome)); // Verifica a pessoa retornada é a correta
        }

        /// <summary>
        /// Verifica se a pessoa pode ser inserida
        /// </summary>
        [TestMethod]
        public void PodeInserir()
        {
            Pessoa pessoa = new Pessoa();
            pessoa.Nome = "Pessoa da Silva";
            pessoa.Endereco = "Rua com Nome 123, nr 12, CEP 34032-000";
            pessoa.DataNascimento = new DateTime(1979, 12, 21);

            int contador = this.MockRepositorioPessoa.ObterTodos().Count;

            // Salva a pessoa
            pessoa = this.MockRepositorioPessoa.Salvar(pessoa);
            
            Assert.IsNotNull(pessoa); // Teste se é nulo
            Assert.AreEqual(pessoa.ID, contador + 1);
        }

        /// <summary>
        /// Verifica se a pessoa pode ser alterada
        /// </summary>
        [TestMethod]
        public void PodeAlterar()
        {
            Pessoa pessoa = new Pessoa();
            int id = 1;
            pessoa.ID = id;
            string nome = "Pessoa da Silva";
            pessoa.Nome = nome;
            pessoa.Endereco = "Rua com Nome 123, nr 12, CEP 34032-000";
            pessoa.DataNascimento = new DateTime(1979, 12, 21);

            int contador = this.MockRepositorioPessoa.ObterTodos().Count;

            // Salva a pessoa
            pessoa = this.MockRepositorioPessoa.Salvar(pessoa);

            Assert.IsNotNull(pessoa); // Teste se é nulo
            Assert.AreEqual(pessoa.ID, id);
            Assert.AreEqual(pessoa.Nome, nome);
        }

        /// <summary>
        /// Verifica se todos as pessoas foram retornadas
        /// </summary>
        [TestMethod]
        public void PodeExcluir()
        {
            int id = 3;

            // Obtém a pessoa com ID 1
            bool excluido = this.MockRepositorioPessoa.Excluir(id);

            Assert.AreEqual(true, excluido);
        }

        /// <summary>
        /// Verifica se todos as pessoas foram retornadas
        /// </summary>
        [TestMethod]
        public void PodeConcacentarDadosUsuario()
        {
            Pessoa pessoa = this.MockRepositorioPessoa.ObterPorID(2);

            string dadosUsuario = pessoa.ConcatenarDadosUsuario();
            string dadosUsuarioCalculado = string.Format("ID: {0}, Nome: {1}, Idade: {2}, Endereço: {3}", pessoa.ID.ToString(), pessoa.Nome, pessoa.Idade.ToString(), pessoa.Endereco);

            Assert.AreEqual(dadosUsuario, dadosUsuarioCalculado);
        }
    }
}
