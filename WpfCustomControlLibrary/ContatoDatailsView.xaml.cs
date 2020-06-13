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

using EventAggregator;
using Model;
using Model.Events;


namespace WpfCustomControlLibrary
{
    /// <summary>
    /// Interação lógica para ContatoDatailsView.xam
    /// </summary>
    public partial class ContatoDatailsView : UserControl, ISubscriber<ContatoSaved>, ISubscriber<ContatoSelected>, ISubscriber<ContatoCreated>, ISubscriber<ContatoUpdate>
    {
        public ContatoDatailsView(IEventAggregator ea)
        {
            InitializeComponent();
            ea.SubscribeEvent(this);
        }

        #region Saved
        public void OnEventHandler(ContatoSaved e)
        {
            this.lbDetail.Content = string.Format("Contato Salvo {0}", e.Contato.NomeContato);
        }
        #endregion

        #region Selected
        public void OnEventHandler(ContatoSelected e)
        {
            this.lbDetail.Content = string.Format("Contato Selecionado {0}", e.Contato.NomeContato);
        }
        #endregion

        #region Created
        public void OnEventHandler(ContatoCreated e)
        {
            this.lbDetail.Content = string.Format("Contato Criado {0}", e.Contato.NomeContato);
        }
        #endregion

        #region Remove
        public void OnEventHandler(ContatoUpdate e)
        {
            this.lbDetail.Content = string.Format("Contato Alterado {0}", e.Contato.NomeContato);
        }
        #endregion

    }
}
