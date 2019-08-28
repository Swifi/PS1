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
                if (checkInt(temp, operators, values))
                {
                    continue;
                }

                //see if it's a variable
                else if (checkVar(temp, variableEvaluator, operators, values))
                {
                    continue;
                }

                //see if it's a + or -
                else if (checkAddOrSub(temp, operators, values))
                {
                    continue;
                }

                //see if it's a * or / 
                else if (checkMulOrDiv(temp, operators))
                {
                    continue;
                }

                //see if its a (
                else if (checkLeftPar(temp, operators))
                {
                    continue;
                }

                // see it's a )
                else if (checkRightPar(temp, operators, values))
                {
                    continue;
                }

                //If it isn't whitespace, and it's not one of the others, throw an exception
                else if (temp != "")
                {
                    throw new ArgumentException();
                }
            }

            //Check to see if operator stack is empty
            if (operators.Count != 0)
            {
                if (values.Count != 2)
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

            //Check to make sure we have no other values
            if (values.Count != 1)
            {
                throw new ArgumentException("More than 1 value while there are no operators.");
            }
            //return the final answer
            return values.Pop();
        }


        /// <summary>
        /// This method checks if the string is an integer, and if the top of the stack has a * or /, it will perform that operation and push it onto the stack
        /// </summary>
        /// <param name="s"></param> The string being evaluated
        /// <param name="operators"></param> The current operator stack
        /// <param name="values"></param> The current value stack
        /// <returns></returns> Returns true if it is an integer
        private static Boolean checkInt(string s, Stack<string> operators, Stack<int> values)
        {
            //create a temp number
            int tempNum;

            try
            {
                //parse the int
                tempNum = Int32.Parse(s);
            }
            //if the int is not parseable, throw this error
            catch (Exception e)
            {
                return false;
            }

            //check if it is * or /. If it is, do the operation right away
            string tempOper = " ";
            //make sure the stack has something
            if (operators.Count == 0)
                throw new ArgumentException("No operators");
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
            return true;
        }

        /// <summary>
        /// This method checks the string to see if it is a variable. If it is, runs the checkInt method.
        /// </summary>
        /// <param name="s"></param> The string being evaluated
        /// <param name="variableEvaluator"></param> The delegate method to evaluate the variable
        /// <param name="operators"></param> The current operator stack
        /// <param name="values"></param> The current value stack
        /// <returns></returns> Returns true if it was a variable and was an integer
        private static Boolean checkVar(string s, Lookup variableEvaluator, Stack<string> operators, Stack<int> values)
        {
            //check to see if it's a variable
            try
            {
                //if it is, do the checkInt method
                return checkInt(variableEvaluator(s).ToString(), operators, values);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// This method checks if the string is a + or -, and will evaluate the expression if the previous operator was a + or - as well.
        /// </summary>
        /// <param name="s"></param> The string being evaluated
        /// <param name="operators"></param> The current operator stack
        /// <param name="values"></param> The current value stack
        /// <returns></returns> Returns true if it is a + or -
        private static Boolean checkAddOrSub(string s, Stack<string> operators, Stack<int> values)
        {
            //check if it is a + or -
            if (s != "+" && s != "-")
                return false;

            //check for if the previous operator is a + or -
            string tempOpe = " ";
            if (operators.Count != 0)
                tempOpe = operators.Peek();
            //if it is a + or -, we can go ahead and evaluate right away
            if (tempOpe == "+" || tempOpe == "-")
            {
                //check to make sure we have enough values
                if (values.Count < 2)
                    throw new ArgumentException("Not enough values");

                //pop ther operater and values
                operators.Pop();
                int tempV1 = values.Pop();
                int tempV2 = values.Pop();

                //do the maths
                if (tempOpe == "+")
                {
                    values.Push(tempV2 + tempV1);
                }
                else
                {
                    values.Push(tempV2 - tempV1);
                }
            }

            //push the new operator on the stack, return true
            operators.Push(s);
            return true;
        }

        /// <summary>
        /// This method checks if the string is a * or /, and will push the operator on the stack
        /// </summary>
        /// <param name="s"></param> The string being evaluated
        /// <param name="operators"></param> The current operator stack
        /// <returns></returns> Returns true if it was a * or /
        private static Boolean checkMulOrDiv(string s, Stack<string> operators)
        {
            //if it is a * or /, just add to the list
            if (s == "*" || s == "/")
            {
                operators.Push(s);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method checks if the string is a (, and will push the operator onto the stack
        /// </summary>
        /// <param name="s"></param> The string being evaluated
        /// <param name="operators"></param> The current operator stack
        /// <returns></returns> Returns true if it was a (
        private static Boolean checkLeftPar(string s, Stack<string> operators)
        {
            //if there is a (, add to stack
            if (s == "(")
            {
                operators.Push(s);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method checks if the string is a ), and will then check if it is a + or - and perform the operator if needed. 
        /// After, will make sure the next operator is a (, and then check if the next operator is a * or / and perform the operation if needed.
        /// </summary>
        /// <param name="s"></param> The string being evaluated
        /// <param name="operators"></param> The current operator stack
        /// <param name="values"></param> The current value stack
        /// <returns></returns> Returns true if it was able to perform all of the operations
        private static Boolean checkRightPar(string s, Stack<string> operators, Stack<int> values)
        {
            //check if we have a right )
            if (s == ")")
            {
                //check to see if we have an operator. If not, throw
                string tempOpe = " ";
                if (operators.Count == 0)
                    throw new ArgumentException("No operators");
                tempOpe = operators.Peek();

                //if it is a + or -, continue to add or subtract the values inside the paranthesis
                if (tempOpe == "+" || tempOpe == "-")
                {
                    operators.Pop();

                    //check to make sure we have enough values
                    if (values.Count < 2)
                        throw new ArgumentException("Not enough values");

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

                //make sure the next thing we pop is a (, otherwise just throw exception
                tempOpe = operators.Pop();
                if (tempOpe != "(")
                    throw new ArgumentException("No left paranthesis");

                //Now, check to see if there is a * or / on the edge of these paranthesis.
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

                //return true if we were able to do this whole operation
                return true;
            }
            //return false if there was no )
            return false;
        }
    }
}
