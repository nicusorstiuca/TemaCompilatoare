﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;

namespace TemaFacultativa
{
    class Compiler
    {
        public Compiler(string code)
        {
            _code = code;
        }
        private string _code;
        public void Complie()
        {
            var provider = new CSharpCodeProvider();
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }, "generated.exe", true);
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = true;

            var results = provider.CompileAssemblyFromSource(parameters, _code);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error: Line: {0}. (ErrNo: {1}): {2}", error.Line, error.ErrorNumber, error.ErrorText));
                }

                MessageBox.Show(sb.ToString(), "Eroare compilare",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            else
            {
                var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Process.Start(path + @"\generated.exe");
            }
        }
    }
}
