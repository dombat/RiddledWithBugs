using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MicrosoftSecurityCodeAnalysisTesting.FlawedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiddledWithBugs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            var poorcode = new PoorCode();
            Console.WriteLine(poorcode.TimerDuration());

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
