﻿namespace BlockchainSharp.Compilers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using BlockchainSharp.Vm;

    public class SimpleCompiler
    {
        private BytecodeCompiler compiler = new BytecodeCompiler();
        private string[] lines;
        private int nlines;
        private int cline;

        public SimpleCompiler(string text)
        {
            if (text == null)
                this.lines = new string[0];
            else
                this.lines = text.Split('\n', '\r');

            this.nlines = this.lines.Length;
            this.cline = 0;
        }

        public byte[] Compile()
        {
            while (this.cline < this.nlines)
                this.ProcessLine(this.lines[this.cline++]);

            return compiler.ToBytes();
        }

        private void ProcessLine(string line)
        {
            int p = line.IndexOf("#");

            if (p >= 0)
                line = line.Substring(0, p);

            line = line.Trim();

            if (line.Length == 0)
                return;

            var words = line.Split(' ', '\t');

            if (words.Length == 0)
                return;

            var verb = words[0].ToLower();

            if (verb == "add")
                this.compiler.Add();
            else if (verb == "subtract")
                this.compiler.Subtract();
            else if (verb == "multiply")
                this.compiler.Multiply();
            else if (verb == "divide")
                this.compiler.Divide();
        }
    }
}
