using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SBoT.Code.Uavp.Const;
using SBoT.Code.Uavp.DataModel.Cross;
using SBoT.Code.Uavp.DataModel.Cross.Interfaces;
using SBoT.Code.Uavp.Mapping;
using SBoT.Code.Uavp.Services;
using SBoT.Code.Uavp.Services.Abstractions;

namespace SBoT.Code.Uavp
{
    public class ModuleBootstrapper
    {
        public static void Configure(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<CrossDataModel>(o => o.UseSqlServer(configuration[AppSettingsConst.ConnectionStrings.Cross]));
            serviceCollection.AddScoped<ICrossDataModel>(provider => provider.GetRequiredService<CrossDataModel>());

            serviceCollection.AddScoped<IRosterService, RosterService>();
            serviceCollection.AddScoped<IUserInfoService, UserInfoService>();

            serviceCollection.AddSingleton(provider =>
                new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); }).CreateMapper()
            );
        }
    }
}
