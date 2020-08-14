using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCore.Models;
using ApiCore.Banco;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApiCore.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")] // <- Rota de acesso exemplo: "/Patrimonios"

    public class MarcasController : Controller
    {

        private readonly string ConnectionString;

        public MarcasController(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("default");
        }
                
        [HttpGet]//busca todas as marcas
        public IEnumerable<Marca> Get()
        {
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {                                
                return banco.GetAllMarcas();
            }
        }

        [HttpGet("{id}")] //busca marcas pelo idMarca
        public Marca Get(int id)
        {
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {
                return banco.GetMarcaByIdMarca(id);
            }
        }

        [HttpGet("{id}/patrimonios")] //busca patrimonio pelo idMarca
        public List<Patrimonio> GetPatrimonioByIdMarca(int id)
        {
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {
                return banco.GetPatrimonioByIdMarca(id);
            }
        }

        [HttpPost]//Insere marca
        public ActionResult Post([FromBody]Marca marcaModel)
        {            
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {
                banco.InsertMarca(marcaModel.Nome);
                return Ok();
            }
            
        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Marca marcaModel)
        {
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {
                banco.UpdateMarca(new Marca(id,marcaModel.Nome));
            }
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (ComunicaBd banco = new ComunicaBd(ConnectionString))
            {
                banco.DeleteMarca(id);
            }
        }
    }
}
