using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IRepositorioPessoas
    {
        Pessoa ObterPorID(int pessoaID);

        List<Pessoa> ObterTodos();

        List<Pessoa> ObterPorFiltro(string nome);

        Pessoa Salvar(Pessoa pessoa);

        bool Excluir(int ID);
    }
}
