using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace QUDMMSAPI.Common
{
    public class ConfigHelper
    {
        public static IConfigurationRoot GetConfigRoot()
        {
            var Builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            var Config = Builder.Build();

            return Config;

        }
    }
}
