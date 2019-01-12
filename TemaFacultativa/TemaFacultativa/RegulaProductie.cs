using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemaFacultativa
{
    class RegulaProductie
    {
        public string Stanga;
        public List<string> Dreapta;
        public RegulaProductie(string regula)
        {
            int first = 0;
            int second = regula.IndexOf(' ');
            Stanga = regula.Substring(first, second);
            regula = regula.Substring(second + 1);
            second = regula.IndexOf(' ');
            regula = regula.Substring(second + 1);
            string[] temp = regula.Split(' ');
            Dreapta = temp.ToList<string>();
        }
        public RegulaProductie(List<string> Dreaptatemp, string Stangatemp)
        {
            Stanga = Stangatemp;
            if (Dreaptatemp.Count != 0)
            {
                Dreapta = new List<string>(Dreaptatemp);
            }
            else
            {
                Dreapta = new List<string>();
                Dreapta.Add("~");
            }
        }
        public bool Existenta(List<string> Neterminale, List<string> Terminale)
        {
            if (Neterminale.IndexOf(Stanga) == -1 && Terminale.IndexOf(Stanga) == -1)
            {
                return false;
            }
            foreach (string temp in Dreapta)
            {
                if (Neterminale.IndexOf(temp) == -1 && Terminale.IndexOf(temp) == -1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
