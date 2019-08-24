using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Created by Nick Hayes. This class evaluates arithmetic
/// expressions using standard infix notation
/// </summary>
namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate int Lookup(String v);

        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            //Declare Variables

            //Initialize stacks
            Stack<string> operators = new Stack<string>();
            Stack<int> values = new Stack<int>();

            //Split the whole expression into different tokens
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");


            foreach (string s in substrings)
            {
                //find out what we need to do with the variable
                //see if it's an integer
                checkInt(s, operators, values);

                //see if it's a variable
                checkVar(s, variableEvaluator, operators, values);

                //see if it's a + or -
                else if (checkAddOrSub(s) == true)
                {

                }

                //see if it's a * or / 
                else if (checkMulOrDiv(s) == true)
                {

                }

                //see if its a (
                else if (checkLeftPar(s) == true)
                {

                }

                // see it's a )
                else if (checkRightPar(s) == true)
                {

                }

                //If it isn't whitespace, and it's not one of the others, throw an exception
                else if (s != " ")
                {
                    throw new ArgumentException();
                }
            }

            //Check to see if operator stack is empty

            //return the final answer
            return 0;
        }


        //TODO: Write comments for each of these methods
        private static void checkInt(string s, Stack<string> operators, Stack<int> values)
        {
            int tempNum;

            try
            {
                //parse the int
                tempNum = Int32.Parse(s);
            }
            //if the int is not parseable, throw this error
            catch (Exception e)
            {
                return;
            }

            //check if it is * or /. If it is, do the operation right away
            string tempOper = operators.Peek();
            if (tempOper == "*")
            {
                //times the two numbers together, pop the operator
                values.Push(tempNum * values.Pop());
                operators.Pop();
            }
            else if (tempOper == "/")
            {
                //check if it is a division by 0
                if (tempNum == 0)
                    throw new ArgumentException("Division by 0");

                //divide the previous value by this new value, pop the operator
                values.Push(values.Pop() / tempNum);
                operators.Pop();
            }
            //else, just add the number to the list
            else
            {
                values.Push(tempNum);
            }
        }

        private static void checkVar(string s, Lookup variableEvaluator, Stack<string> operators, Stack<int> values)
        {
            //check to see if it's a variable
            try
            {
                //if it is, do the checkInt method
                checkInt(variableEvaluator(s).ToString(), operators, values);
            }
            catch (Exception e)
            {
                return;
            }
        }

        private static bool checkAddOrSub(string s)
        {
            if (s == "+" || s == "-")
                return true;

            return false;
        }

        private static bool checkMulOrDiv(string s)
        {
            if (s == "*" || s == "/")
                return true;

            return false;
        }

        private static bool checkLeftPar(string s)
        {
            if (s == "(")
                return true;

            return false;
        }

        private static bool checkRightPar(string s)
        {
            return false;
        }
    }
}
