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
    /// Interaction logic for PasswordPromptIntroControl.xaml
    /// </summary>
    public partial class PasswordPromptIntroControl : UserControl
    {
        public PasswordPromptIntroControl ()
        {
            InitializeComponent ();
        }

        public event Action VaultOpenSuccess;

        private void OpenVaultButton_Click ( object sender, RoutedEventArgs e )
        {
            try
            {
                VaultManager.Instance.OpenVault ( this.passwordBox.Password );
                this.passwordBox.Password = "";

                if ( VaultManager.Instance.CurrentVault != null )
                {
                    if ( VaultOpenSuccess != null )
                    {
                        VaultOpenSuccess ();
                    }
                }
            }
            catch ( VaultOpeningException )
            {
            }
        }
    }
}
