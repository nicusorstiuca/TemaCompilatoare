using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TemaFacultativa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> Neterminale = TextBox.GetLineText(1).Remove(TextBox.GetLineText(1).Length - 2).Split(' ').ToList();
            List<string> Terminale = TextBox.GetLineText(2).Remove(TextBox.GetLineText(2).Length - 2).Split(' ').ToList();
            Gramatica gramatica = new Gramatica(TextBox.GetLineText(0).Remove(TextBox.GetLineText(0).Length - 2), Terminale, Neterminale);
            for(int i=3;i<TextBox.LineCount;i++)
            {
                gramatica.Add(TextBox.GetLineText(i).Remove(TextBox.GetLineText(i).Length-2));
            }
            gramatica.VerificaGramatica();
            string ReguliCorectate="";
            foreach(RegulaProductie temp in gramatica.Reguli)
            {
                ReguliCorectate += temp.Stanga + " : ";
                foreach(string s in temp.Dreapta)
                {
                    ReguliCorectate += s+ " ";
                }
                ReguliCorectate += '\n';
            }
            Display.Text = ReguliCorectate;
            /*for (int i = 3; i < TextBox.LineCount; i++)
            {
                Display.Text+= TextBox.GetLineText(i);
            }*/
        }

    }
}
