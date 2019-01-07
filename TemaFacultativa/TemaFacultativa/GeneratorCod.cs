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
        public void print()
        {
            int i = 1;
            foreach (List<string> a in FirstFollow)
            {
                Console.Write($"{i} ");
                i++;
                foreach (string b in a)
                {
                    Console.Write(b + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
