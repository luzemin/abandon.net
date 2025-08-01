using Autofac;
using SLN.Utility.DataBase;
using SLN.Utility.Logger;

namespace SLN;

public class AutofacServiceInjection : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
        builder.RegisterType<NLogHelper>().As<INLogHelper>();
        builder.RegisterType<SqlSugarFactory>().As<ISqlSugarFactory>();
    }
}