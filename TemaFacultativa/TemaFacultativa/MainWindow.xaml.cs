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
        private Gramatica gramatica;
        private GeneratorCod generator;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> Neterminale = TextBox.GetLineText(1).Remove(TextBox.GetLineText(1).Length - 2).Split(' ').ToList();
                List<string> Terminale = TextBox.GetLineText(2).Remove(TextBox.GetLineText(2).Length - 2).Split(' ').ToList();
                gramatica = new Gramatica(TextBox.GetLineText(0).Remove(TextBox.GetLineText(0).Length - 2), Terminale, Neterminale);
                int TextBoxTextLines = TextBox.LineCount;
                for (int i = 4; i < TextBoxTextLines - 1; i++)
                {
                    gramatica.Add(TextBox.GetLineText(i).Remove(TextBox.GetLineText(i).Length - 2));
                }
                gramatica.Add(TextBox.GetLineText(TextBoxTextLines - 1));
                gramatica.VerificaGramatica();
                string ReguliCorectate = "";
                foreach (RegulaProductie temp in gramatica.Reguli)
                {
                    ReguliCorectate += temp.Stanga + " : ";
                    foreach (string s in temp.Dreapta)
                    {
                        ReguliCorectate += s + " ";
                    }
                    ReguliCorectate += '\n';
                }
                Display1.Text = ReguliCorectate;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        

    }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                generator = new GeneratorCod(gramatica);
                generator.MultimileFF();
                string MultimiDirector = "";
                foreach (List<string> i in generator.FirstFollow)
                {
                    foreach (string j in i)
                    {
                        MultimiDirector += j + " ";
                    }
                    MultimiDirector += "\n";
                }
                Display2.Text = MultimiDirector;
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        public string CodSursa;
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            CodSursa = generator.GenereazaCod();
            Display3.Text = CodSursa;
        }

        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            Compiler C = new Compiler(CodSursa);
            C.Complie();
        }
    }
}
