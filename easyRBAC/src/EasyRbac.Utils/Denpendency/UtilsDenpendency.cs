﻿using System;
using System.Collections.Generic;
using System.Text;
using EasyRbac.Utils.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRbac.Utils.Denpendency
{
    public static class UtilsDenpendency
    {
        public static IServiceCollection AddUtils(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEncryptHelper, EncryptHelper>();
            //serviceCollection.Configure<IdGenerateConfig>("IdGenerate", opt=>{});
            IConfiguration idGenerateConfiguration = (configuration.GetSection("IdGenerate") as IConfiguration) ?? new ConfigurationBuilder().Build();
            services.Configure<IdGenerateConfig>(idGenerateConfiguration);
            services.AddSingleton<IIdGenerator, EasyGenerator>();
            services.AddSingleton<INumberConvert, NumberConvert>();

            return services;
        }
    }
}
