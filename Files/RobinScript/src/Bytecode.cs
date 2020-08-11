﻿using System;
using System.Collections.Generic;

namespace RobinScript
{
    public enum Istructions {
        CALL, // call functions giving parameters
        STORE, // storing <arg1> in the memory using an id (var name)
        JUMP, // go to a specific istruction given in <arg0> (id of istruction [int])
                
    }
    public class Bytecode
    {
        public List<Block> Current = new List<Block>();
        public void Append(string lineNumber, Istructions istruction, string[] arguments)
        {
            Current.Add(new Block() { IstructionNumber = int.Parse(lineNumber), Arguments = arguments, Istruction = istruction });
        }
        public static Bytecode Parse(string bytecode)
        {
            string[] code = bytecode.Split(new char[] { '\n', '\r' });
            Bytecode BTTable = new Bytecode();
            for (int i = 0; i < code.Length; i++) {
                if (string.IsNullOrWhiteSpace(code[i])) continue;
                string text = code[i].Substring(code[i].IndexOf("\t")+1);
                List<string> arguments = new List<string>();
                bool isInterpolate = false;
                string word = "";
                for (int j = 0; j < text.Length; j++) {
                    if (text[j] == '\'') {
                        isInterpolate = (isInterpolate) ? false : true;
                        word += '\'';
                    }
                    else if (isInterpolate) {
                        word += text[j];
                    } else if (text[j] == ' ' && !isInterpolate) {
                        if (!string.IsNullOrWhiteSpace(word)) {
                            arguments.Add(word);
                            word = "";
                        }
                    } else {
                        word += text[j];
                    }
                }
                BTTable.Append(code[i].Split(' ')[0], (Istructions) Enum.Parse(typeof(Istructions), code[i].Split('\t')[0].Split(' ')[1].ToUpper()), arguments.ToArray());
            }
            return BTTable;
        }
        public Block ElementAt(int index)
        {
            return Current[index];
        }
        public override string ToString()
        {
            System.Text.StringBuilder toReturn = new System.Text.StringBuilder();
            for (int i = 0; i < Current.Count; i++) {
                string tmp = "";
                for (int j = 0; j < Current[i].Arguments.Length; j++) {
                    tmp += Current[i].Arguments[j]+" ";
                }
                toReturn.AppendLine($"{Current[i].IstructionNumber} {Current[i].Istruction}\t{tmp}");
            }
            return toReturn.ToString();
        }
    }
    public class Block
    {
        public int IstructionNumber { get; set; }
        public Istructions Istruction { get; set; }
        public object[] Arguments { get; set; }
    }
}