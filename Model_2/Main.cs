using System;
using System.Net.Http.Json;
using Common;
using Newtonsoft.Json;

namespace Model_2
{
    public class Main:IModel
    {
        public string Name => "Model #2";
        public string Description => "Append NuGet project";
        public int Exe()
        {
            Console.WriteLine(JsonConvert.SerializeObject(this));
            return 0;
        }
    }
}