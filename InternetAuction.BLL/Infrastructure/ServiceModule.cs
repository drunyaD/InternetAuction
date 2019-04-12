using InternetAuction.DAL.Interfaces;
using InternetAuction.DAL.Repositories;
using Ninject.Modules;

namespace InternetAuction.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private readonly string _connectionString;
        public ServiceModule(string connection)
        {
            _connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(_connectionString);
        }
    }
}
