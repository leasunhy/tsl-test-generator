﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSLTestGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TSLGenerator.GetRandomTSLScript(2333));
        }
    }
}
