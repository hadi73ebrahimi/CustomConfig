using System;
using CustomConfig;
namespace CustomConfig_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello I'm Hadi :)");
            Console.WriteLine("Let's get started with our test...");
            Console.WriteLine("At first we will make a custom config");
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine("Boom");

            CreateNewConfig();

            Console.WriteLine("Okay... now we have a config file.");
            Console.WriteLine("Let's load it into our program");

            LoadConfig();



            Console.Read();
        }

        static void CreateNewConfig()
        {
            var myconfig = new CConfig();
            myconfig.SetVar("Ip", "127..0.0.1","my server port");
            myconfig.SetVar("Port", "5555"," tcp port");
            myconfig.SetVar("LastUpdate", DateTime.Now.ToLongDateString()," when this config updated");
            myconfig.SaveData(AppContext.BaseDirectory+"/configs/firstconfig.txt");
        }

        static void LoadConfig()
        {
            var myconfig = new CConfig(AppContext.BaseDirectory + "/configs/firstconfig.txt");

            Console.WriteLine("Last config update was:  " + myconfig.ReadVar("LastUpdate"));
            Console.WriteLine("And it's comment is:  " + myconfig.ReadComment("LastUpdate"));

            myconfig.SetVar("LastUpdate", DateTime.Now.ToLongDateString(), " updated for the last time");
            myconfig.SaveData(AppContext.BaseDirectory + "/configs/firstconfig.txt");
        }
    }
}
