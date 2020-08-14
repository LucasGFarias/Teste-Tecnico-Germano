using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCore.Models
{
    public class Marca
    {
        [Required]
        public string Nome { get; set; }
        public int Id { get; set; }



        public Marca(int id, string nome)
        {
            this.Id = id;
            this.Nome = nome;
        }

        public Marca()
        {

        }

    }


}
