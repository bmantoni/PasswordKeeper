using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PasswordKeeperWpf.View
{
    /// <summary>
    /// Interaction logic for VaultViewControl.xaml
    /// </summary>
    public partial class VaultViewControl : UserControl
    {
        public VaultViewControl ()
        {
            InitializeComponent ();

            UpdateEntriesList ();
        }

        private void UpdateEntriesList ()
        {
            //EntryList.Items.Clear ();
            
            if ( VaultManager.Instance.CurrentVault != null )
            {
                EntryList.DataContext = VaultManager.Instance.CurrentVault.EntriesList;
                //EntryList.ItemsSource = VaultManager.Instance.CurrentVault.EntriesList;
            }
        }

        #region Insert & Update Entries

        private void AddNewEntryButton_Click ( object sender, RoutedEventArgs e )
        {
            NewWindow = new NewVaultEntryWindow ();
            NewWindow.NewEntryOk += new Action<Entry> ( NewWindow_Ok );
            NewWindow.Show ();
        }

        private void EntryList_MouseDoubleClick ( object sender, MouseButtonEventArgs e )
        {
            NewWindow = new NewVaultEntryWindow (
                ( sender as ListView ).SelectedItem as Entry );
            NewWindow.NewEntryOk += new Action<Entry> ( NewWindow_Ok );
            NewWindow.Show ();
        }

        void NewWindow_Ok ( Entry obj )
        {
            switch ( NewWindow.Mode )
            {
                case VaultEntryFormMode.Edit:
                    VaultManager.Instance.CurrentVault.UpdateEntry ( obj );
                    break;
                case VaultEntryFormMode.Insert:
                    VaultManager.Instance.CurrentVault.AddEntry ( obj );
                    break;
                case VaultEntryFormMode.Delete:
                    VaultManager.Instance.CurrentVault.DeleteEntry ( obj );
                    break;
            }

            VaultManager.Instance.SaveVault ();
            //UpdateEntriesList ();

        } 

        #endregion

        private NewVaultEntryWindow NewWindow;
    }
}
