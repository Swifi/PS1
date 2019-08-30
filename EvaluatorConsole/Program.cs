using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FormulaEvaluator.Evaluator;

/// <summary>
/// This class tests the FormulaEvaluator to make sure 
/// there are no issues.
/// </summary>
namespace EvaluatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            if (test1() == 135)
            {
                Console.WriteLine("Test 1 passed");
            }
            if (test2() == 2)
            {
                Console.WriteLine("Test 2 passed");
            }
            if (test3() == 1)
            {
                Console.WriteLine("Test 3 passed");
            }
            if (test4() == 1)
            {
                Console.WriteLine("Test 4 passed");
            }
            if (test5() == 1)
            {
                Console.WriteLine("Test 5 passed");
            }
            if (test6())
            {
                Console.WriteLine("Test 6 passed");
            }
            if (test7())
            {
                Console.WriteLine("Test 7 passed");
            }


            Console.WriteLine("All tests completed.");


            Console.ReadKey();
        }

        static int Lookup(String v)
        {
            if (v == "A1")
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
                // Console.WriteLine(e.Message);
                if (e.Message != "Division by 0")
                {
                    return -1;
                }
                return 1;
            }

        }

        /// <summary>
        /// Tests to see if we can give an invalid parameter
        /// </summary>
        /// <returns></returns>
        static int test4()
        {
            try
            {
                string temp = "C55 + 89";
                return Evaluate(temp, new Lookup(Lookup));
            }
            catch (Exception e)
            {
                // Console.WriteLine(e.Message);
                if (e.Message != "Not a valid argument")
                {
                    return -1;
                }

                return 1;
            }
        }

        /// <summary>
        /// This test checks if we throw an error for too many operators
        /// </summary>
        /// <returns></returns>
        static int test5()
        {
            try
            {
                string temp = "B55 + 89 + 9 + +";
                return Evaluate(temp, new Lookup(Lookup));
            }
            catch (Exception e)
            {
                // Console.WriteLine(e.Message);
                if (e.Message != "Not enough values")
                {
                    return -1;
                }

                return 1;
            }
        }

        /// <summary>
        /// This will test a bunch of different expression
        /// </summary>
        /// <returns></returns> True if all the expressions passed
        static bool test6()
        {
            string temp = "6 * 5 + (70 - 68)";
            if (Evaluate(temp, new Lookup(Lookup)) != 32)
                return false;

            temp = "5 + 5 + 5 + 3 * 2 / 5";
            if (Evaluate(temp, new Lookup(Lookup)) != 16)
                return false;

            temp = "(5 / 5) + (6 / 3) - (0 / 8) * (6 - 2)";
            if (Evaluate(temp, new Lookup(Lookup)) != 3)
                return false;


            return true;
        }

        static bool test7()
        {
            string temp;
            try
            {
                temp = "(5 / 5) (6 / 3) (0 / 8) (6 - 2)";
                Evaluate(temp, new Lookup(Lookup));
                return false;
            }
            catch(Exception)
            {
            }

            try
            {
                temp = "456AGS + 5";
                Evaluate(temp, new Lookup(Lookup));
                return false;
            }
            catch (Exception)
            {

            }

            try
            {
                temp = "A1 + 90 / 9 + ( 7 A1)";
                Evaluate(temp, new Lookup(Lookup));
                return false;
            }
            catch (Exception)
            {

            }

            return true;
        }
    }
}
