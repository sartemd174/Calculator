using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace ConsoleApplication3
{
    class Program
    {

        class Calcuator
        {
            private static List<char> operations = new List<char> { '(', ')', '+', '-', '*', '/' };

            private static bool Skip(char s)
            {
                if ((" ".IndexOf(s) != -1))
                    return true;
                return false;
            }
            private static bool Operator(char s)
            {
                if (operations.Contains(s))
                    return true;
                else return false;              
            }
            private static int GetPriority(char s)
            {
                return operations.IndexOf(s);
            }
            public static string Calc(string input)
            {
                string get_ans = string.Empty;
                try
                {
                    double d = Count(Parse_str(input));
                    get_ans = "Результат: "+d.ToString();
                }
                catch (Exception)
                {
                   get_ans= "Входная строка имела неверный формат";
                }
                return get_ans;
            }
            private static string Parse_str(string input)
            {                   
                    Stack<char> operationsStack = new Stack<char>(); //стек операторов
                    string output = string.Empty; 
                    for (int i = 0; i < input.Length; i++) 
                    {
                        if (Skip(input[i]))
                            continue; //пропускаем пробелы
                        if (Char.IsDigit(input[i])) 
                        {
                            while (!Skip(input[i]) && !Operator(input[i]))//считываем число до разделятиеля или оператора
                            {
                                output += input[i]; 
                                i++;

                                if (i == input.Length) break; 
                            }

                            output += " "; 
                           
                            i--;// возврат к символу перед разделителем
                        }

                        if (Operator(input[i]) ) //если символ один из операторов
                        {
                            if (input[i] == '(') 
                                operationsStack.Push(input[i]); 
                            else if (input[i] == ')') //если правая скобка то считываем все операторые до открывающей скобки
                            {
                                char s = operationsStack.Pop();
                                while (s != '(')
                                {
                                    output += s.ToString() + ' ';
                                    s = operationsStack.Pop();
                                }
                            }
                            else //если другой оператор
                            {
                                if (operationsStack.Count > 0) 
                                    if (GetPriority(input[i]) <= GetPriority(operationsStack.Peek())) //если текущий оператор ниже по приорету чем тот что на вершине стека, то добавляем верхний оператор в строку
                                        output += operationsStack.Pop().ToString() + " "; 

                                operationsStack.Push(char.Parse(input[i].ToString())); //если наоборот то добавляем текущий оператор на вершину стека (самый высокий приоритет)

                            }
                        }
                    }

                        while (operationsStack.Count > 0)
                            output += operationsStack.Pop() + " ";
                    output = output.Replace('.', ',');
                    return output; 
            }
            private static double Count(string input)
            {
                double result = 0; 
                Stack<double> temp_stack = new Stack<double>(); 
                for (int i = 0; i < input.Length; i++) 
                {
                    if (Char.IsDigit(input[i]))
                    {
                        string a = string.Empty;

                        while (!Skip(input[i]) && !Operator(input[i])) //до оператора или разделителя
                        {
                            a += input[i]; 
                            i++;
                            if (i == input.Length) break;
                        }

                        temp_stack.Push(double.Parse(a)); //Записываем в стек
                        i--;
                    }
                    else if (Operator(input[i])) 
                    {
                        double a = temp_stack.Pop();//берем два последних
                        double b = temp_stack.Pop();
                        switch (input[i]) 
                        {
                            case '+': result = b + a; break;
                            case '-': result = b - a; break;
                            case '*': result = b * a; break;
                            case '/': result = b / a; break;
                        }
                        temp_stack.Push(result); 
                    }
                }
                return temp_stack.Peek(); 
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Введите выражение: ");
                Console.WriteLine(Calcuator.Calc(Console.ReadLine()));
            }
        }
       
    }
}
