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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace PasswordKeeperWpf.View
{
    /// <summary>
    /// Interaction logic for VaultViewWindow.xaml
    /// </summary>
    public partial class VaultViewWindow : Window
    {
        public VaultViewWindow ()
        {
            InitializeComponent ();

            UpdateEntriesList ();
        }

        public event Action SelfClosed;

        private void UpdateEntriesList ()
        {
            //EntryList.Items.Clear ();

            if ( VaultManager.Instance.CurrentVault != null )
            {
                EntriesToShow = VaultManager.Instance.CurrentVault.EntriesList;
                EntryList.DataContext = EntriesToShow;
            }
        }

        #region New/Edit Window Callbacks

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
        }

        #endregion

        #region Local Button Event Handlers

        private void AddNewEntryButton_Click ( object sender, RoutedEventArgs e )
        {
            NewWindow = new NewVaultEntryWindow ();
            NewWindow.NewEntryOk += new Action<Entry> ( NewWindow_Ok );
            NewWindow.Show ();
        }

        private void EntryList_MouseDoubleClick ( object sender, MouseButtonEventArgs e )
        {
            NewWindow = new NewVaultEntryWindow (
                ( sender as ListBox ).SelectedItem as Entry );
            NewWindow.NewEntryOk += new Action<Entry> ( NewWindow_Ok );
            NewWindow.Show ();
        }

        private void SaveButton_Click ( object sender, RoutedEventArgs e )
        {
            VaultManager.Instance.SaveVault ();
            MessageBox.Show ( 
                "Save Successful", "Vault Saved", MessageBoxButton.OK, MessageBoxImage.Information );
        }

        private void CloseVaultButton_Click ( object sender, RoutedEventArgs e )
        {
            bool doClose = true;

            if ( VaultManager.Instance.CurrentVault != null
                && VaultManager.Instance.CurrentVault.IsUnsavedChanges )
            {
                MessageBoxResult result = MessageBox.Show (
                    "Do you wish to save before closing?",
                    "Save unsaved changes?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question );

                if ( result == MessageBoxResult.Yes )
                {
                    VaultManager.Instance.SaveVault ();
                    VaultManager.Instance.CloseVault ();
                }
                else if ( result == MessageBoxResult.Cancel )
                {
                    doClose = false;
                }
            }
            if ( doClose )
            {
                VaultManager.Instance.CloseVault ();
                this.Close ();
                if ( SelfClosed != null )
                {
                    SelfClosed ();
                }
            }
        }

        #endregion

        private NewVaultEntryWindow NewWindow;

        private void FavIconImage_Loaded ( object sender, RoutedEventArgs e )
        {
            Image fiImg = sender as Image;
            string url = ( fiImg.DataContext as Entry ).Url;

            if ( !String.IsNullOrEmpty ( url ) )
            {
                fiImg.Dispatcher.BeginInvoke (
                    new Action ( delegate () { fiImg.Source = GetImage ( url ); } ), 
                    DispatcherPriority.Normal );
            }
            // Source="{Binding Path=Url, Converter={StaticResource UrlToFavIconConverter}}"
        }

        private BitmapImage GetImage ( string _url )
        {
            string domainUrl = Regex.Replace ( _url, "^http://", "" );

            if ( domainUrl.Contains ( "/" ) )
            {
                domainUrl = domainUrl.Substring ( 0, domainUrl.IndexOf ( '/' ) + 1 );
            }

            return new BitmapImage ( new Uri ( "http://google.com/s2/favicons?domain=" + domainUrl ) );
        }

        public void DragWindow ( object sender, MouseButtonEventArgs args )
        {
            DragMove ();
        }

        private void EntrySearchTextBox_TextChanged ( object sender, TextChangedEventArgs e )
        {
            if ( EntrySearchTextBox.Text == "" )
            {
                ShowAllEntries ();
            }
            else
            {
                FilterEntries ( EntrySearchTextBox.Text );
            }
        }

        private void FilterEntries ( string str )
        {
            if ( EntriesToShow != null )
            {
                IEnumerable<Entry> temp =
                    EntriesToShow.Where<Entry> ( p => p.Title.Contains ( str ) );
                EntriesToShow = new ObservableCollection<Entry> ();
                foreach ( Entry e in temp )
                {
                    EntriesToShow.Add ( e );
                }
            }
        }

        private void ShowAllEntries ()
        {
            EntriesToShow = VaultManager.Instance.CurrentVault.EntriesList;
        }

        public ObservableCollection<Entry> EntriesToShow
        {
            get { return this.entriesToShow; }
            set
            {
                this.entriesToShow = value;
                
            }
        }
        private ObservableCollection<Entry> entriesToShow;

        private class GetFavionArgs
        {
            public string Url { get; set; }
            public Image Img { get; set; }
        }
    }
}
