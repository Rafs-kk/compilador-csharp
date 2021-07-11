using System;

namespace complicador
{
    class main
    {
        public static int Main(string[] args)
        {
            Parser parser;
            if(args == null || args.Length == 0)
            {
            	Console.WriteLine("Nenhum argumento foi passado para args 0\n");
            	return 0;
            }
            parser = new Parser(args[0]);
            parser.programa();
            return 0;
        }
    }
}