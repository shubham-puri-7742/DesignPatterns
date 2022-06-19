using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

// INTERPRETER
// Processes text to input by lexing (separating text into tokens) and interpreting them (parsing)
// e.g. converting source code to executables
// Basically anywhere the text conforms to a specific syntax
// (programming language compilers/interpreters, IDEs, HTML or FDX formats)
// This example implements a simple numeric expression analyser

namespace DesignPatterns
{
    // common operations to all types of expressions
    public interface IElement
    {
        double Val { get; }
    }
    
    // types of elements
    // num
    public class Num : IElement
    {
        public Num(double val)
        {
            Val = val;
        }

        public double Val { get; }
    }
    
    // binary operation
    public class BinaryOp : IElement
    {
        public enum Type
        {
            Add, Sub
        }

        public Type type;
        public IElement lhs, rhs;

        public double Val
        {
            get
            {
                switch (type)
                {
                    case Type.Add:
                        return lhs.Val + rhs.Val;
                    case Type.Sub:
                        return lhs.Val - rhs.Val;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
    
    // token class
    public class Token
    {
        // types
        public enum Type
        {
            Num, Plus, Minus, LBrack, RBrack
        }

        // store the type and its textual representation
        public Type type;
        public string Text;

        // ctor
        public Token(Type type, string text)
        {
            this.type = type;
            Text = text ?? throw new ArgumentNullException(paramName: nameof(text));
        }

        // display format
        public override string ToString()
        {
            return $"`{Text}`";
        }
    }

    public class DriverCode
    {
        // lexer
        static List<Token> Lex(string input)
        {
            var result = new List<Token>();
            // for each character in the input
            for (int i = 0; i < input.Length; ++i)
            {
                switch (input[i])
                {
                    // simple single-character tokens (+/-/brackets/space)
                    case ' ':
                        continue;
                    case '+':
                        result.Add(new Token(Token.Type.Plus, "+"));
                        break;
                    case '-':
                        result.Add(new Token(Token.Type.Minus, "-"));
                        break;
                    case '(':
                        result.Add(new Token(Token.Type.LBrack, "("));
                        break;
                    case ')':
                        result.Add(new Token(Token.Type.RBrack, ")"));
                        break;
                    // digit/literal
                    default:
                        // create a new string builder
                        var sb = new StringBuilder(input[i].ToString());
                        // for each subsequent character
                        for (int j = i + 1; j < input.Length; ++j)
                        {
                            // if it is a digit
                            if (char.IsDigit(input[j]))
                            {
                                // append it
                                sb.Append(input[j]);
                                // increment the outer variable (to look one place ahead)
                                ++i;
                            }
                            // end the digits stream
                            else
                            {
                                // add the completed number to the list of tokens
                                result.Add(new Token(Token.Type.Num, sb.ToString()));
                                break;
                            }
                        }
                        break;
                }
            }

            return result;
        }

        // parser
        static IElement Parse(IReadOnlyList<Token> tokens)
        {
            var result = new BinaryOp();
            // checks if the lhs has been initialised
            bool lhs = false;

            for (int i = 0; i < tokens.Count; ++i)
            {
                var token = tokens[i];
                switch (token.type)
                {
                    case Token.Type.Num:
                        var num = new Num(double.Parse(token.Text));
                        // if the lhs is not set
                        if (!lhs)
                        {
                            // set it
                            result.lhs = num;
                            lhs = true;
                        }
                        // else, set the rhs
                        else
                        {
                            result.rhs = num;
                        }
                        break;
                    case Token.Type.Plus:
                        result.type = BinaryOp.Type.Add;
                        break;
                    case Token.Type.Minus:
                        result.type = BinaryOp.Type.Sub;
                        break;
                    // parse until the closing bracket is hit and parse the subexpression recursively
                    case Token.Type.LBrack:
                        int j = i;
                        for (; j < tokens.Count; ++j)
                            if (tokens[j].type == Token.Type.RBrack)
                                break;
                        // get the subexpression from i + 1 (skipping the opening bracket) to j - (i + 1) (skipping the closing bracket)
                        var subEx = tokens.Skip(i + 1).Take(j - (i + 1)).ToList();
                        // parse recursively
                        var elem = Parse(subEx);
                        // if the lhs is not set
                        if (!lhs)
                        {
                            // set it
                            result.lhs = elem;
                            lhs = true;
                        }
                        // else, set the rhs
                        else
                        {
                            result.rhs = elem;
                        }
                        i = j;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }
        
        static void Main(string[] args)
        {
            string input = "(15 + 7) - (16+ 4)"; 
            var tokens = Lex(input);
            WriteLine(string.Join("\t", tokens));

            var parsed = Parse(tokens);
            WriteLine($"{input} = {parsed.Val}");
        }
    }
}