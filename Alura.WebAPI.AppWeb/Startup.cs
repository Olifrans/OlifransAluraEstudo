using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Alura.ListaLeitura.Seguranca;
using Alura.WebAPI.AppWeb.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Timers;

namespace Alura.WebAPI.AppWeb
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LeituraContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("ListaLeitura"));
            });
           
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AuthDB"));
            });

            services.AddIdentity<Usuario, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<AuthDbContext>();

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Usuario/Login";
            });

            services.AddTransient<IRepository<Livro>, RepositorioBaseEF<Livro>>();

            services.AddMvc(options => {
                options.OutputFormatters.Add(new LivroCsvFormatters()); 
            }).AddXmlSerializerFormatters();

            //services.AddControllersWithViews();
            /* Opção alterada para --> AluraWebApi.AuthProvider
            //Injeção de dependencia JwToken
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = "JwtBearer"; //Portador do Json Web Token => exemplo Frodo do filme o Senhor dos Aneis
                options.DefaultChallengeScheme = "JwtBearer";            
            }).AddJwtBearer("JwtBearer", options =>{
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, //Juiz que validar
                    ValidateAudience = true, //Quem esta pedido
                    ValidateLifetime = true, //Tempo de expiração
                    ValidateIssuerSigningKey = true, //Validação da chave

                    //Chave de assinatura que sera usada para validar no Juiz
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("olifrans_estudo_webapi_authentication_valid")),
                   
                    ClockSkew = TimeSpan.FromMilliseconds(5), //Tempo de validação do Tokem
                    ValidIssuer = "Alura.WebAPI.AppWeb",
                    ValidAudience = "Postman",
                };
            });
            */
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();


            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
