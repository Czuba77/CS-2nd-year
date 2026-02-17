using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace labpt7
{
    public class RecursiveClass : IComparable<RecursiveClass>
    {
        private int Number;
        public int number
        {
            get { return Number; }
            set { Number = value; }
        }
        private string Name;
        public string name
        {
            get { return Name; }
            set { Name = value; }
        }
        private string Surname;
        public string surname
        {
            get { return Surname; }
            set { Surname = value; }
        }
        protected SortedSet<RecursiveClass> Children;
        private RecursiveClass Parent;
        public RecursiveClass parent
        {
            get { return Parent; }
            set { Parent = value; }
        }

        public RecursiveClass(int number, string name, string surname, RecursiveClass parent)
        {
            this.Number = number;
            this.Name = name;
            this.Surname = surname;
            Children = new SortedSet<RecursiveClass>();
            this.parent = parent;
        }

        public int GetNumber() => Number;
        public string GetName() => Name;
        public string GetSurname() => Surname;

        public void AddChild(RecursiveClass child)
        {
            Children.Add(child);
        }

        public void PrintRecursive(string depth)
        {
            Console.WriteLine(ToString());
            foreach (var child in Children)
            {
                Console.Write(depth);
                child.PrintRecursive(depth + "\t");
            }
        }

        public int CountChildren()
        {
            int count = 0;
            foreach (var child in Children)
            {
                count++;
                count += child.CountChildren();
            }
            return count;
        }

        public override string ToString()
        {
            return $"{Name} {Surname}, {Number}\n";
        }

        public override int GetHashCode()
        {
            int hash = 7;
            foreach (char c in Name)
                hash += c;
            foreach (char c in Surname)
                hash += 13 * c;
            hash += 17 * Number;
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is RecursiveClass o)
            {
                return Number == o.Number && Name == o.Name && Surname == o.Surname;
            }
            return false;
        }

        public int CompareTo(RecursiveClass other)
        {
            int numCompare = Number.CompareTo(other.Number);
            if (numCompare != 0)
                return numCompare;

            int nameCompare = Name.CompareTo(other.Name);
            if (nameCompare != 0)
                return nameCompare;

            return Surname.CompareTo(other.Surname);
        }

        public TreeViewItem makeRecTree(RecursiveClass node)
        {
            TreeViewItem currentTree = new TreeViewItem
            {
                Header = node.GetName(),
                Tag = node
            };
            ContextMenu cm = new ContextMenu();
            currentTree.ContextMenu = cm;

            MenuItem delete = new MenuItem { Header = "Delete", Tag = node };
            delete.Click += delNode;

            cm.Items.Add(delete);

            MenuItem create = new MenuItem { Header = "Create", Tag = node };
            create.Click += addNode;

            cm.Items.Add(create);

            if (node.Children != null && node.Children.Any())
            {
                foreach (RecursiveClass childNode in node.Children)
                {
                    TreeViewItem childTreeViewItem = makeRecTree(childNode);
                    currentTree.Items.Add(childTreeViewItem);
                }
            }

            return currentTree;
        }

        private void delNode(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is RecursiveClass toDelete)
            {
                if (toDelete.parent != null)
                {
                    toDelete.parent.Children.Remove(toDelete);
                }
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is MainWindow main)
                    {
                        RecRemoveItemFromTree(main.treeView, toDelete);
                        main.descriptionText.Text = "";
                        break;
                    }
                }
            }
        }
        private void RecRemoveItemFromTree(ItemsControl parent, RecursiveClass toDelete)
        {
            foreach (object item in parent.Items)
            {
                if (item is TreeViewItem tvi)
                {
                    if (tvi.Tag is RecursiveClass rc && rc.Equals(toDelete))
                    {
                        parent.Items.Remove(tvi);
                        return;
                    }
                    RecRemoveItemFromTree(tvi, toDelete);
                }
            }
        }

        private void addNode(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is RecursiveClass parentNode)
            {
                Window1 window = new Window1(parentNode);
                window.ShowDialog();

                foreach (Window w in Application.Current.Windows)
                {
                    if (w is MainWindow main)
                    {
                        AddNewChildToTree(main.treeView, parentNode);
                        break;
                    }
                }
            }
        }

        private void AddNewChildToTree(ItemsControl parent, RecursiveClass target)
        {
            foreach (object item in parent.Items)
            {
                if (item is TreeViewItem tvi)
                {
                    if (tvi.Tag is RecursiveClass rc && rc.Equals(target))
                    {
                        tvi.Items.Clear();
                        foreach (var child in rc.Children)
                        {
                            tvi.Items.Add(child.makeRecTree(child));
                        }
                        tvi.IsExpanded = true;
                        return;
                    }
                    AddNewChildToTree(tvi, target);
                }
            }
        }


        public TreeViewItem makeTree()
        {
            return makeRecTree(this);
        }

    }
}
