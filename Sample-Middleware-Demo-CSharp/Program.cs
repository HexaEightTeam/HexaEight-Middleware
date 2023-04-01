using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;


namespace HexaEight_Middleware_SampleDemo
{
    public class Program
    {
        public static string GetSelfHash()
        {
            try
            {
                using (SHA512 SHA512 = SHA512Managed.Create())
                {
                    using (FileStream fs = new FileStream(Process.GetCurrentProcess().MainModule.FileName, FileMode.Open, FileAccess.Read))
                    {
                        return BitConverter.ToString(SHA512.ComputeHash(fs)).Replace("-", "").ToUpper();
                    }
                }
            }
            catch
            {
                return "";
            }

        }

        public static void Main(string[] args)
        {
            Console.WriteLine("API Middleware Server Hash : " + GetSelfHash());

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
