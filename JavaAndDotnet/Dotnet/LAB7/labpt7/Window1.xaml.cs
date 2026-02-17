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
using System.Windows.Shapes;

namespace labpt7
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private RecursiveClass parentNode;
        public Window1(RecursiveClass parent)
        {
            InitializeComponent();
            this.parentNode = parent;
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            int number = int.Parse(numberBox.Text);
            string name = nameBox.Text;
            string surname = surnameBox.Text;
            RecursiveClass newChild = new RecursiveClass(number, name, surname, parentNode);
            parentNode.AddChild(newChild);

            Close();
        }

    }
}
