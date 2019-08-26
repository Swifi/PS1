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
                //TODO: add way for once we detect something has gone through to just skip to the next string
                string temp = s.Trim();
                //find out what we need to do with the variable
                //see if it's an integer
                checkInt(temp, operators, values);

                //see if it's a variable
                checkVar(temp, variableEvaluator, operators, values);

                //see if it's a + or -
                checkAddOrSub(temp, operators, values);

                //see if it's a * or / 
                checkMulOrDiv(temp, operators);

                //see if its a (
                checkLeftPar(temp, operators);

                // see it's a )
                checkRightPar(temp, operators, values);

                //If it isn't whitespace, and it's not one of the others, throw an exception
                /*if (temp != " ")
                {
                    throw new ArgumentException();
                }*/
            }

            //Check to see if operator stack is empty
            if (operators.Count != 0)
            {
                if(values.Count != 2)
                {
                    throw new ArgumentException("More than 2 values when there are still operators.");
                }
                string tempOpe = operators.Pop();
                int tempV1 = values.Pop();
                int tempV2 = values.Pop();

                if (tempOpe == "+")
                {
                    values.Push(tempV2 + tempV1);
                }
                else
                {
                    values.Push(tempV2 - tempV1);
                }
            }

            if(values.Count != 1)
            {
                throw new ArgumentException("More than 1 value while there are no operators.");
            }
            //return the final answer
            return values.Pop();
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
            string tempOper = " ";
            //make sure the stack has something
            if(operators.Count != 0)
                tempOper = operators.Peek();

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

        private static void checkAddOrSub(string s, Stack<string> operators, Stack<int> values)
        {
            if (s != "+" && s != "-")
                return;

            string tempOpe = " ";
            if (operators.Count != 0)
                tempOpe = operators.Peek();
            if (tempOpe == "+" || tempOpe == "-")
            {
                //TODO: need to write error checking
                operators.Pop();
                int tempV1 = values.Pop();
                int tempV2 = values.Pop();

                if(tempOpe == "+")
                {
                    values.Push(tempV2 + tempV1);
                }
                else
                {
                    values.Push(tempV2 - tempV1);
                }
            }


            operators.Push(s);
        }

        private static void checkMulOrDiv(string s, Stack<string> operators)
        {
            if (s == "*" || s == "/")
                operators.Push(s);
        }

        private static void checkLeftPar(string s, Stack<string> operators)
        {
            if (s == "(")
                operators.Push(s);
        }

        private static void checkRightPar(string s, Stack<string> operators, Stack<int> values)
        {
            if (s == ")")
            {
                string tempOpe = " ";
                if (operators.Count != 0)
                    tempOpe = operators.Peek();

                if (tempOpe == "+" || tempOpe == "-")
                {
                    operators.Pop();
                    int tempV1 = values.Pop();
                    int tempV2 = values.Pop();

                    if (tempOpe == "+")
                    {
                        values.Push(tempV2 + tempV1);
                    }
                    else
                    {
                        values.Push(tempV2 - tempV1);
                    }
                }
                //TODO: check for * or /

                tempOpe = operators.Pop();
                if (tempOpe != "(")
                    throw new ArgumentException("No left paranthesis");

                tempOpe = operators.Peek();
                //check if it is * or /. If it is, do the operation right away
                if (tempOpe == "*")
                {
                    int tempNum = values.Pop();
                    //times the two numbers together, pop the operator
                    values.Push(tempNum * values.Pop());
                    operators.Pop();
                }
                else if (tempOpe == "/")
                {
                    int tempNum = values.Pop();
                    //check if it is a division by 0
                    if (tempNum == 0)
                        throw new ArgumentException("Division by 0");

                    //divide the previous value by this new value, pop the operator
                    values.Push(values.Pop() / tempNum);
                    operators.Pop();
                }
            }
        }
    }
}
