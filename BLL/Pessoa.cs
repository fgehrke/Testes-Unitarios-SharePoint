using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Pessoa
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Endereco { get; set; }
        public int Idade
        {
            get
            {
                return (int) DateTime.Now.Subtract(DataNascimento).TotalDays / 365;
            }
        }

        public string ConcatenarDadosUsuario()
        {
            return string.Format("ID: {0}, Nome: {1}, Idade: {2}, Endereço: {3}", this.ID.ToString(), this.Nome, this.Idade.ToString(), this.Endereco);
        }
    }
}
