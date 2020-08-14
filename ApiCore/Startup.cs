using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ApiCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
             .AddJsonOptions(options =>
            {
             options.SerializerSettings.Formatting = Formatting.Indented;
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async c =>
                    {
                        c.Response.StatusCode = 500;
                        await c.Response.WriteAsync("Algo de errado ocorreu, verifique se os dados estão sendo inseridos corretamente e a possibilidade de redundancia. \r\n\r\n---Ajuda---\r\n" +
                            "Colunas alteráveis da tabela marcas: nome\r\n" +
                            "Colunas alteráveis da tabela patrimonios: nome,descricao,idMarca)\r\n"+
                            "Obs: Nome da marca e do patrimonio não podem estar branco e nem redundantes.");
                    });
                });
            }

            app.UseMvc();
        }
    }
}
