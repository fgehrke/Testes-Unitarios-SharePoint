using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class RepositorioPessoa: IRepositorioPessoas
    {
        public Pessoa ObterPorID(int pessoaID)
        {
            throw new NotImplementedException();
        }

        public List<Pessoa> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public List<Pessoa> ObterPorFiltro(string nome)
        {
            throw new NotImplementedException();
        }

        public Pessoa Salvar(Pessoa pessoa)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(int ID)
        {
            throw new NotImplementedException();
        }
    }
}
