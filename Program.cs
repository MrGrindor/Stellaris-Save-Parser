using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace StellarisSaveParser
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var _saveParserContext = new SaveParserContext();
            var parser = new Parser(_saveParserContext);
            parser.parseSave();

            

        }


        
    }
}
