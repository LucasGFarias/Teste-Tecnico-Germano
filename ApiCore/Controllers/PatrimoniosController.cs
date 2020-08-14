using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCore.Banco;
using ApiCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApiCore.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class PatrimoniosController : Controller
    {
        private readonly string ConnectionString;

        public PatrimoniosController(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("default");//string de conexão vinda do settings.json
        }
                
        [HttpGet]//busca todos os patrimonios
        public IList<Patrimonio> Get()
        {
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {
                return banco.GetAllPatrimonios();
            }
        }

        [HttpGet("{numeroTombo}")]//busca patrimonio pelo numero do tombo
        public Patrimonio Get(int numeroTombo)
        {
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {
                return banco.GetPatrimonioByNumeroTombo(numeroTombo);
            }
        }
                
        [HttpPost]//insere patrimonio 
        public void Post([FromBody]Patrimonio patrimonio)
        {
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {
                banco.InsertPatrimonio(new Patrimonio(patrimonio.Nome,patrimonio.IdMarca,patrimonio.Descricao));
            }
        }
        
        [HttpPut("{numeroTombo}")]//atualiza patrimonio pelo id
        public void Put(int numeroTombo, [FromBody]Patrimonio patrimonio)
        {
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {
                banco.UpdatePatrimonio(new Patrimonio(patrimonio.Nome, patrimonio.IdMarca, patrimonio.Descricao,numeroTombo));
            }
        }
        
        [HttpDelete("{numeroTombo}")]//exclui patrimonio pelo numero do tombo
        public void Delete(int numeroTombo)
        {
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {
                banco.DeletePatrimonio(numeroTombo);
            }
        }
    }
}
