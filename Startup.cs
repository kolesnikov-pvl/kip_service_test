using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System.Configuration;
using NHibernate.Driver;
using Configuration = NHibernate.Cfg.Configuration;
using NHibernate.Tool.hbm2ddl;

namespace kip_service_test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Add(ServiceDescriptor.Singleton(ISessionFactory => CreateSessionFactory()));
        }

        public static ISessionFactory CreateSessionFactory()
        {
            var msSqlConfiguration = MsSqlConfiguration.MsSql2008;
            var connString = ConfigurationManager.AppSettings[GlobalConst.ConnectionString];
            return Fluently.Configure(new Configuration())
                    .Database(msSqlConfiguration.ConnectionString(connString)
                    .UseOuterJoin()
                    .Driver(typeof(Sql2008ClientDriver).AssemblyQualifiedName).ShowSql())
                    .ExposeConfiguration(cfg => cfg.SetProperty("command_timeout", "100")
                                                               .SetProperty("connection_timeout", "0"))
                    .Mappings(c => c.FluentMappings.AddFromAssembly(typeof(Startup).Assembly))
                    .ExposeConfiguration(conf => {
                                                                var update = new SchemaUpdate(conf);
                                                                update.Execute(false, true);
                                                            })
                    .BuildSessionFactory();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
