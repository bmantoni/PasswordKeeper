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
using Microsoft.Win32;

namespace PasswordKeeperWpf.View
{
    /// <summary>
    /// Interaction logic for NewVaultWindow.xaml
    /// </summary>
    public partial class NewVaultWindow : Window
    {
        public NewVaultWindow ()
        {
            InitializeComponent ();
        }

        /// <summary>
        /// vaultFileName, password
        /// </summary>
        public event Action<string, string> NewVaultWindowClosedOk;

        private void ChooseVaultLocationButton_Click ( object sender, RoutedEventArgs e )
        {
            SaveFileDialog sfd = new SaveFileDialog ();
            sfd.AddExtension = true;
            sfd.DefaultExt = "vault";
            sfd.FileName = "MyVault";
            sfd.Title = "Choose where to save the Vault...";

            if ( sfd.ShowDialog () ?? false )
            {
                this.vaultFileName = sfd.FileName;
            }
        }

        private string vaultFileName;

        private void OkButton_Click ( object sender, RoutedEventArgs e )
        {
            if ( vaultFileName != null
                && VaultManager.IsValidPassword ( passwordBox.Password ) )
            {
                CloseMyself ();
            }
            else
            {
                MessageBox.Show ( "You must enter a valid password" );
            }
        }

        private void CloseMyself ()
        {
            if ( NewVaultWindowClosedOk != null )
            {
                NewVaultWindowClosedOk ( vaultFileName, passwordBox.Password );
            }
            this.Close ();
        }
    }
}
