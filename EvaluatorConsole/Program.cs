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
            Console.WriteLine(test1());
            Console.WriteLine(test2());
            Console.WriteLine(test3());


            Console.ReadKey();
        }

        static int Lookup(String v)
        {
            if(v == "A1")
                return 0;

            if (v == "B55")
                return 10;

            if (v == "H95")
                return 3;

            throw new ArgumentException();
        }

        /// <summary>
        /// Tests simple addition with some paranthesis
        /// </summary>
        /// <returns></returns>
        static int test1()
        {
            //simple test
            string temp = "55 + 4 * (10 + 10)";
            return Evaluate(temp, new Lookup(Lookup));
        }

        /// <summary>
        /// Tests some variables with multiplication
        /// </summary>
        /// <returns></returns>
        static int test2()
        {
            string temp = "A1 - 5 + B55 * H95 / (2 * 2)";
            return Evaluate(temp, new Lookup(Lookup));
        }

        /// <summary>
        /// Tests division by zero
        /// </summary>
        /// <returns></returns>
        static int test3()
        {
            try
            {
                string temp = "B55 + 89 / 0";
                return Evaluate(temp, new Lookup(Lookup));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (e.Message != "Division by 0")
                {
                    return -1;
                }
                return 1;
            }

        }
    }
}
