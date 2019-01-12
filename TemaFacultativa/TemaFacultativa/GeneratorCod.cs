using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemaFacultativa
{
    class GeneratorCod
    {
        public GeneratorCod(Gramatica cGramatica)
        {
            Gramatica = cGramatica;
            FirstFollow = new List<List<string>>();
            ReguliVizitate = new List<RegulaProductie>();
        }
        public List<List<string>> FirstFollow;
        public Gramatica Gramatica;
        public List<RegulaProductie> ReguliVizitate;
        public void CodRegulaProductie(ref string cod, List<string> Dreapta)
        {
            int ifsnumber = 0;
            foreach (string atom in Dreapta)
            {
                if (Gramatica.Neterminale.IndexOf(atom) >= 0)
                {
                    cod += $"{atom}();\n";
                }
                else if (Gramatica.Terminale.IndexOf(atom) >= 0)
                {
                    cod += $"if(Intrare[i].Equals(\"{atom}\"))\n{{\n i++;\n ";
                    ifsnumber++;
                }
            }
            for (int i = 0; i < ifsnumber - 1; i++)
            {
                cod += "}\n" +
                        "else\n" +
                        "{\n" +
                        "string errormessage=i.ToString()+\" eroare!\";"+
                        "throw new Exception(errormessage);" +
                        "}\n";

            }
            if (ifsnumber > 0)
                cod += "}\n";
        }
        public string Initializarecod()
        {
            string cod = "using System;\n" +
                          "using System.Collections.Generic;\n" +
                          "using System.Linq;\n" +
                          "using System.Text;\n" +
                          "using System.Threading.Tasks;\n" +
                          "namespace comp\n" +
                          "{\n" +
                          "class Program\n" +
                          "{\n" +
                          "static string[] Intrare;\n" +
                          "static string SirIntrare;\n" +
                          "public static int i = 0;\n";
            return cod;
        }
        public string FunctieMain()
        {
            string cod = "static void Main(string[] args)" +
                         "{\n" +
                         "try{\n" +
                         "Console.Write(\"Sir de intrare:\");\n" +
                         "SirIntrare=Console.ReadLine();\n" +
                         "Intrare=SirIntrare.Split(\' \');\n" +
                        $"{Gramatica.SimbolStart}();\n" +
                         "if (Intrare[i].Equals(\"$\"))\n" +
                         "Console.WriteLine(\"Propozitie corecta!\");\n" +
                         "else\n" +
                         "Console.WriteLine(\"Propozitie incorecta!\");\n" +
                         "Console.Read();\n"+
                         "}\n" +
                         "catch (Exception e)\n" +
                         "{\n" +
                         "Console.WriteLine(e.Message);\n" +
                         "Console.Read();\n" +
                         "}\n}\n";
            return cod;
        }
        public string FunctieNeterminal(string Neterminal, int[] reguli)
        {
            string cod = "";
            int i = 0;
            cod += $"public static void {Neterminal}()\n{{\n";
            while (Gramatica.Reguli[reguli[i] - 1].Dreapta.IndexOf("~") >= 0)
            {
                i++;
            }
            int primaregula = i;
            CodRegulaProductie(ref cod, Gramatica.Reguli[reguli[i] - 1].Dreapta);
            i++;
            int nregula;
            while (i < reguli.Count())
            {
                nregula = reguli[i] - 1;
                if (Gramatica.Terminale.IndexOf(Gramatica.Reguli[nregula].Dreapta[0]) >= 0)
                {
                    cod += "else ";
                    CodRegulaProductie(ref cod, Gramatica.Reguli[nregula].Dreapta);
                }
                else if (Gramatica.Neterminale.IndexOf(Gramatica.Reguli[nregula].Dreapta[0]) >= 0)
                {
                    cod += "else {";
                    CodRegulaProductie(ref cod, Gramatica.Reguli[nregula].Dreapta);
                    cod += "}";
                }
                i++;
            }
            cod += "\n}\n";
            return cod;
        }
        public string FunctiiNeterminale()
        {
            string cod = "";
            int[][] tabela = TabelaAnalizaSintactica();
            int[] zero = { 0 };
            for (int i = 0; i < Gramatica.Neterminale.Count; i++)
            {
                int[] reguli = tabela[i].Distinct().ToArray();
                reguli = reguli.Except(zero).ToArray();
                cod += FunctieNeterminal(Gramatica.Neterminale[i], reguli);
            }
            return cod;
        }
        public string GenereazaCod()
        {
            string cod = "";
            cod += Initializarecod();
            cod += FunctieMain();
            cod += FunctiiNeterminale();
            cod += "}}";
            return cod;
        }
        public List<string> Follow(string Neterminal)
        {
            List<string> MFollow = new List<string>();
            foreach (RegulaProductie regula in Gramatica.Reguli)
            {
                if (ReguliVizitate.IndexOf(regula) < 0)
                {
                    for (int i = 0; i < regula.Dreapta.Count; i++)
                    {
                        if (regula.Dreapta[i] == Neterminal)
                        {
                            ReguliVizitate.Add(regula);
                            if (i != regula.Dreapta.Count - 1)
                            {
                                if (!Gramatica.Neterminale.Contains(regula.Dreapta[i + 1]))
                                    MFollow.Add(regula.Dreapta[i + 1]);
                                else
                                    foreach (RegulaProductie r in Gramatica.Reguli)
                                    {
                                        if (r.Stanga == regula.Dreapta[i + 1])
                                            MFollow.AddRange(First(r));

                                    }
                            }
                            else
                                if (MFollow.IndexOf("$") < 0)
                                MFollow.Add("$");
                        }
                    }
                }
            }
            return MFollow;
        }
        public List<string> First(RegulaProductie alfa)
        {
            List<string> MFirst = new List<string>();
            if (!Gramatica.Neterminale.Contains(alfa.Dreapta[0]))
            {
                if (alfa.Dreapta[0] != "~")
                    MFirst.Add(alfa.Dreapta[0]);
                else
                    foreach (RegulaProductie regula in Gramatica.Reguli)
                    {
                        if (regula.Dreapta[regula.Dreapta.Count - 1] == alfa.Stanga)
                        {
                            MFirst.AddRange(Follow(regula.Stanga));
                        }
                    }
            }
            else
                foreach (RegulaProductie regula in Gramatica.Reguli)
                {
                    if (regula.Stanga == alfa.Dreapta[0])
                        MFirst.AddRange(First(regula));
                }
            return MFirst;
        }
        public void ConditieLL1()
        {
            List<RegulaProductie> Reguli = Gramatica.Reguli;
            for (int i = 0; i < Reguli.Count - 1; i++)
            {
                for (int j = i + 1; j < Reguli.Count; j++)
                {
                    if (Reguli[i].Stanga == Reguli[j].Stanga)
                    {
                        if (FirstFollow[i].Intersect(FirstFollow[j]) != null)
                        {
                            throw new Exception("Gramatica nu indeplineste conditiile gramaticii LL1");
                        }
                    }
                }
            }
        }
        public void MultimileFF()
        {
            foreach (RegulaProductie alfa in Gramatica.Reguli)
            {
                if (alfa.Dreapta.Contains("~"))
                {
                    List<string> MFollow = new List<string>();
                    foreach (RegulaProductie regula in Gramatica.Reguli)
                    {
                        if (regula.Dreapta[regula.Dreapta.Count - 1] == alfa.Stanga && regula.Stanga != alfa.Stanga)
                        {
                            MFollow.AddRange(Follow(regula.Stanga));
                        }
                    }
                    ReguliVizitate.Clear();
                    FirstFollow.Add(MFollow);
                }
                else
                {
                    FirstFollow.Add(First(alfa));
                }
            }
        }
        private int[][] TabelaAnalizaSintactica()
        {
            Gramatica.Terminale.Add("$");
            int Nsize = Gramatica.Neterminale.Count;
            int Tsize = Gramatica.Terminale.Count;
            int[][] tabela = new int[Nsize][];
            int Rulessize = Gramatica.Reguli.Count;
            for (int i = 0; i < Nsize; i++)
            {
                tabela[i] = new int[Tsize];
                for (int j = 0; j < Tsize; j++)
                {
                    for (int k = 0; k < Rulessize; k++)
                    {
                        if (Gramatica.Reguli[k].Stanga == Gramatica.Neterminale[i])
                        {
                            if (FirstFollow[k].IndexOf(Gramatica.Terminale[j]) >= 0)
                            {
                                tabela[i][j] = k + 1;
                                break;
                            }
                        }
                    }
                }

            }
            return tabela;
        }
    }
}
