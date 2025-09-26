using Autofac;
using Abandon.NET.Utility.DataBase;
using Abandon.NET.Utility.Logger;

namespace Abandon.NET;

public class AutofacServiceInjection : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
        builder.RegisterType<NLogHelper>().As<INLogHelper>();
        builder.RegisterType<SqlSugarFactory>().As<ISqlSugarFactory>();
    }
}