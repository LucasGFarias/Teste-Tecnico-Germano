using ApiCore.Banco;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCore.Models
{
    public class Patrimonio
    {
        [Required]
        public string Nome { get; set; }
        public string Descricao { get; set; }        
        [Required]
        public int IdMarca { get; set; }
        public int NumeroTombo { get; set; }

        //Gera numero do tombo
        public int GeraNumeroTombo()
        {            
            Random random = new Random();
            return random.Next();
        }

        public Patrimonio()
        {

        }

        public Patrimonio(string nome, int idMarca, string descricao,int numeroTombo)
        {
            this.Nome = nome;
            this.IdMarca = idMarca;
            this.Descricao = descricao;
            this.NumeroTombo = numeroTombo;
        }

        public Patrimonio(string nome, int idMarca, string descricao)
        {
            this.Nome = nome;
            this.IdMarca = idMarca;
            this.Descricao = descricao;
        }
    }
}
