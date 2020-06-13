using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using EventAggregator;
using Model;
using Model.Events;

namespace WpfCustomControlLibrary
{
    /// <summary>
    /// Interação lógica para ContatoListView.xam
    /// </summary>
    public partial class ContatoListView : UserControl
    {
        ObservableCollection<Contato> contatosList;
        public ContatoListView()
        {
            InitializeComponent();

            btnAlterar.IsEnabled = false;
        }

        public IEventAggregator EventAggregator { get; set; }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contato selectedContato = this.contatoList.SelectedItem as Contato;

            if(this.EventAggregator != null)
            {
                txtNomeContato.Text = selectedContato.NomeContato;
                txtNumeroContato.Text = selectedContato.NumeroContato;
                btnAlterar.IsEnabled = true;

                this.EventAggregator.PublishEvent(new ContatoSelected() { Contato = selectedContato });
            }
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtNomeContato.Text) && !string.IsNullOrEmpty(txtNumeroContato.Text))
            {
                var newContato = new Contato() { NomeContato = txtNomeContato.Text, NumeroContato = txtNumeroContato.Text };

                contatosList.Add(newContato);

                if(this.EventAggregator != null)
                {
                    this.EventAggregator.PublishEvent(new ContatoCreated() { Contato = newContato });
                }
            }
            
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            var savedContato = this.contatoList.SelectedItem as Contato;

            this.EventAggregator.PublishEvent(new ContatoSaved() { Contato = savedContato });
        }

        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            contatosList = new ObservableCollection<Contato>();

            this.contatoList.ItemsSource = contatosList;
            this.contatoList.SelectedIndex = 0;
            contatosList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(contato_CollectionChanged);
        }

        private void contato_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                this.contatoList.SelectedItem = e.NewItems[0];
            }
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            var indexContato = this.contatoList.SelectedIndex;

            Contato updateContato = new Contato() { NomeContato = txtNomeContato.Text, NumeroContato = txtNumeroContato.Text };

            this.EventAggregator.PublishEvent(new ContatoUpdate() { Contato = updateContato });

            this.contatosList[indexContato].NomeContato = updateContato.NomeContato;
            this.contatosList[indexContato].NumeroContato = updateContato.NumeroContato;

            this.contatoList.Items.Refresh();
        }
    }
}
