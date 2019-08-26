using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FormulaEvaluator.Evaluator;

namespace EvaluatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //simple test
            Lookup l1 = new Lookup(Lookup);
            string temp = "55 + 4 * (10 + 10)";
            Console.WriteLine(Evaluate(temp, l1));
            Console.Read();
        }

        static int Lookup(String v)
        {
            if(v == "cow")
                return 0;

            throw new ArgumentException();
        }
    }
}
