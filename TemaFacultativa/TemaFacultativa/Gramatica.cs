using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemaFacultativa
{
    class Gramatica
    {
        public string SimbolStart;
        public List<string> Terminale;
        public List<string> Neterminale;
        private List<string> NeterminaleNoi;
        public List<RegulaProductie> Reguli;
        public int netcount;
        public Gramatica(string Start, List<string> Terminalele, List<string> Neterminalele)
        {
            SimbolStart = new string(Start.ToCharArray());
            Terminale = new List<string>(Terminalele);
            Neterminale = new List<string>(Neterminalele);
            NeterminaleNoi = new List<string>();
            Reguli = new List<RegulaProductie>();
            netcount = 1;
        }
        public void Add(string line)
        {
            RegulaProductie temp = new RegulaProductie(line);
            Reguli.Add(temp);
        }
        public void Add(RegulaProductie regula)
        {
            Reguli.Add(regula);
        }
        public void VerificaGramatica()
        {
            GramaticaLL1();
        }
        public bool Existenta()
        {
            foreach (RegulaProductie temp in Reguli)
            {
                if (temp.Existenta(Neterminale, Terminale) == false)
                {
                    return false;
                }
            }

            return true;
        }
        public void GramaticaLL1()
        {
            foreach (string Neterminal in Neterminale)
            {
                List<RegulaProductie> RegulileNeterminalului = ReguliNeterminal(Reguli, Neterminal);
                List<RegulaProductie> ReguliNoi;
                ReguliNoi = VerificaInceputAsemanator(RegulileNeterminalului, Neterminal);
                if (ReguliNoi != null)
                {
                    Inlocuieste(RegulileNeterminalului, ReguliNoi);
                }
            }
            Neterminale.AddRange(NeterminaleNoi);
            foreach (string Neterminal in Neterminale)
            {
                List<RegulaProductie> RegulileNeterminalului = ReguliNeterminal(Reguli, Neterminal);
                List<RegulaProductie> ReguliNoi;
                ReguliNoi = VerificaRecursivitateStanga(RegulileNeterminalului, Neterminal);
                if (ReguliNoi != null)
                {
                    Inlocuieste(RegulileNeterminalului, ReguliNoi);
                }
            }
            Neterminale.AddRange(NeterminaleNoi);
        }
        public void Inlocuieste(List<RegulaProductie> ReguliVechi, List<RegulaProductie> ReguliNoi)
        {
            foreach (RegulaProductie temp in ReguliVechi)
            {
                Reguli.Remove(temp);
            }
            foreach (RegulaProductie temp in ReguliNoi)
            {
                Reguli.Add(temp);
            }
        }
        public List<RegulaProductie> ReguliNeterminal(List<RegulaProductie> R, string Neterminal)
        {
            List<RegulaProductie> RegNeterminal = new List<RegulaProductie>();
            foreach (RegulaProductie temp in R)
            {
                if (temp.Stanga == Neterminal)
                {
                    RegNeterminal.Add(temp);
                }
            }
            return RegNeterminal;
        }
        public List<RegulaProductie> VerificaRecursivitateStanga(List<RegulaProductie> RegNeterm, string Neterminal)
        {
            List<RegulaProductie> ReguliNoi = new List<RegulaProductie>();
            RegulaProductie RegulaRecursiva = null;
            string neterminalu = new string(Neterminal.ToCharArray());
            bool Recursivitate = false;
            foreach (RegulaProductie temp in RegNeterm)
            {
                if (temp.Dreapta.First<string>() == Neterminal)
                {
                    RegulaRecursiva = new RegulaProductie(temp.Dreapta, temp.Stanga);
                    Recursivitate = true;
                    RegNeterm.Remove(temp);
                    Reguli.Remove(temp);
                    break;
                }
            }
            if (Recursivitate)
            {
                foreach (RegulaProductie temp in RegNeterm)
                {
                    List<string> NouaDreapta = temp.Dreapta;
                    NouaDreapta.Add(Neterminal + $"{netcount}");
                    RegulaProductie regulanoua = new RegulaProductie(NouaDreapta, temp.Stanga);
                    ReguliNoi.Add(regulanoua);
                }
                if (RegulaRecursiva != null)
                {
                    RegulaRecursiva.Dreapta.Remove(Neterminal);
                    RegulaRecursiva.Dreapta.Add(Neterminal + $"{netcount}");
                    RegulaRecursiva.Stanga += $"{netcount}";
                    ReguliNoi.Add(RegulaRecursiva);
                    neterminalu = Neterminal + $"{netcount}";
                    netcount++;
                    NeterminaleNoi.Add(neterminalu);
                    RegulaProductie Epsilon = new RegulaProductie($"{neterminalu} : ~");
                    ReguliNoi.Add(Epsilon);
                }
                return ReguliNoi;
            }
            return null;
        }
        public List<RegulaProductie> VerificaInceputAsemanator(List<RegulaProductie> RegNeterm, string Neterminal)
        {
            List<RegulaProductie> ReguliNoi = new List<RegulaProductie>(RegNeterm);
            List<RegulaProductie> ReguliAcelasiInceput = new List<RegulaProductie>();
            string neterminalul = new string(Neterminal.ToCharArray());
            List<string> AcelasiInceput = new List<string>();
            for (int i = 0; i < ReguliNoi.Count - 1; i++)
            {
                AcelasiInceput.Clear();
                RegulaProductie temp1 = ReguliNoi[i];
                for (int j = i + 1; j < ReguliNoi.Count; j++)
                {
                    RegulaProductie temp2 = ReguliNoi[j];
                    if (temp1.Stanga != temp2.Stanga)
                        break;
                    bool InceputAsemanator = false;
                    int length;
                    if (temp1.Dreapta.Count < temp2.Dreapta.Count)
                        length = temp1.Dreapta.Count;
                    else
                        length = temp2.Dreapta.Count;
                    List<string> dreaptatemp1 = new List<string>(temp1.Dreapta);
                    List<string> dreaptatemp2 = new List<string>(temp2.Dreapta);
                    for (int k = 0; k < length; k++)
                    {
                        if (dreaptatemp1[k] == dreaptatemp2[k])
                        {
                            AcelasiInceput.Add(dreaptatemp1[k]);
                            dreaptatemp1.RemoveAt(k);
                            dreaptatemp2.RemoveAt(k);
                            k--;
                            length--;
                            InceputAsemanator = true;
                        }
                        else
                            break;
                    }
                    if (InceputAsemanator == true)
                    {
                        neterminalul += $"{netcount}";
                        NeterminaleNoi.Add(neterminalul);
                        netcount++;
                        AcelasiInceput.Add(neterminalul);
                        ReguliNoi.Add(new RegulaProductie(AcelasiInceput, Neterminal));
                        ReguliNoi.Add(new RegulaProductie(dreaptatemp1, neterminalul));
                        ReguliNoi.Add(new RegulaProductie(dreaptatemp2, neterminalul));
                        ReguliNoi.Remove(temp1);
                        ReguliNoi.Remove(temp2);
                        i--;
                        break;
                    }
                }
            }
            return ReguliNoi;
        }
    }
}
