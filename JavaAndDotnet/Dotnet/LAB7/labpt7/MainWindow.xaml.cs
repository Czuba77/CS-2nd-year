using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace labpt7;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    RecursiveClass god;
    public MainWindow()
    {
        InitializeComponent();
        god = new RecursiveClass(7, "Stworca", "Wszechmogący", null);
    }

    private void GenerateData(Object sender, RoutedEventArgs e)
    {

        var michal = new RecursiveClass(2, "Michał", "Kulesza", god);
        var antek = new RecursiveClass(1, "Antek", "Czuba", god);
        var janek = new RecursiveClass(3, "Janek", "Moniewski", god);

        antek.AddChild(new RecursiveClass(6, "Mijka", "Czuba-Wozniak", antek));
        var humus = new RecursiveClass(4, "Humus", "Czuba", antek);
        humus.AddChild(new RecursiveClass(2, "Bibi", "Czuba", humus));
        antek.AddChild(humus);

        janek.AddChild(new RecursiveClass(7, "Bajtek", "Moniewski",janek));
        janek.AddChild(new RecursiveClass(3, "Kasza", "Moniewski", janek));
        janek.AddChild(new RecursiveClass(2, "Cekin", "Moniewski", janek));

        god.AddChild(michal);
        god.AddChild(antek);
        god.AddChild(janek);

        TreeViewItem tree = god.makeTree();
        treeView.Items.Add(tree);
    }

    private void Exit(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void ShowVersion(Object sender, RoutedEventArgs e)
    {
        MessageBox.Show("App version. 1.0");
    }

    private void SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (treeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is RecursiveClass parent)
        {
            descriptionText.Text = parent.ToString();
            descriptionText.Text += $"Number of chilldren {parent.CountChildren()}\n";
            if (parent.parent != null)
                descriptionText.Text += $"Parent: {parent.parent.ToString()}\n";
            else
                descriptionText.Text += "Parent: It is root\n";
        }
    }

}