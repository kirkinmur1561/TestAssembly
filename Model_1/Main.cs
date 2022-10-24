using System;
using Common;

namespace Model_1
{
    public class Main:IModel
    {
        public string Name => "Model #1";
        public string Description => "Create m1";
        public int Exe()
        {
            Console.WriteLine("Model #1");
            return 0;
        }
    }
}