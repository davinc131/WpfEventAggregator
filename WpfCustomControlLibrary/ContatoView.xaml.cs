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
    /// Interação lógica para ContatoView.xam
    /// </summary>
    public partial class ContatoView : UserControl, ISubscriber<ContatoSaved>, ISubscriber<ContatoSelected>, ISubscriber<ContatoCreated>, ISubscriber<ContatoUpdate>
    {
        public ContatoView(IEventAggregator ea)
        {
            InitializeComponent();
            ea.SubscribeEvent(this);
        }



        #region Created
        public void OnEventHandler(ContatoCreated e)
        {
            this.lbView.Content = string.Format("Contato Criado Número {0}", e.Contato.NumeroContato);
        }
        #endregion

        #region Saved
        public void OnEventHandler(ContatoSaved e)
        {
            this.lbView.Content = string.Format("Contato Salvo Número {0}", e.Contato.NumeroContato);
        }
        #endregion

        #region Selected
        public void OnEventHandler(ContatoSelected e)
        {
            this.lbView.Content = string.Format("Contato Selecionado Número {0}", e.Contato.NumeroContato);
        }
        #endregion

        #region Remove
        public void OnEventHandler(ContatoUpdate e)
        {
            this.lbView.Content = string.Format("Contato Alterado Número {0}", e.Contato.NumeroContato);
        }
        #endregion
    }
}
