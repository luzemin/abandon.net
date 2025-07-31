using Autofac;
using SLN.DbHelper;
using SLN.LogHelper;

namespace SLN;

public class ServiceInjection : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
        builder.RegisterType<NLogHelper>().As<INLogHelper>();
        builder.RegisterType<SqlSugarFactory>().As<ISqlSugarFactory>();
    }
}