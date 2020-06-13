using EventAggregator;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
using WpfCustomControlLibrary;

namespace WpfAppEventAggregator
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        IEventAggregator ea;

        public MainWindow()
        {
            InitializeComponent();

            this.ea = new EventAggregator.EventAggregator();
            this.ContatoListView.EventAggregator = this.ea;

            var tabs = this.ContatoView.Items;

            tabs.Add(new TabItem() { Header = "Nome Contato", Content = new ContatoView(this.ea) });
            tabs.Add(new TabItem() { Header = "Número Contato", Content = new ContatoDatailsView(this.ea) });
        }
    }
}
