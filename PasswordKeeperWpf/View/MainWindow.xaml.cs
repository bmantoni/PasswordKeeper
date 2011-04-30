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
using PasswordKeeperWpf.View;
using Microsoft.Win32;
using System.Windows.Media.Animation;

namespace PasswordKeeperWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();

            #region Init Commands
            
            // Insert code required on object creation below this point.
            CommandBindings.Add ( new CommandBinding ( ApplicationCommands.Close,
                new ExecutedRoutedEventHandler ( delegate ( object sender, ExecutedRoutedEventArgs args ) { this.Close (); } ) ) ); 

            #endregion

            this.LockedView.VaultOpenSuccess += 
                new Action ( LockedView_VaultOpenSuccess );

            InitializeVault ();
		}

        private void InitializeVault ()
        {
            if ( VaultManager.Instance.CurrentVaultFileName == null )
            {
                DoCreateNewVault ();
            }
            else
            {
                GoToLockedVaultState ();
            }
        }

        private void DoCreateNewVault ()
        {
            /// No vault setup on machine
            this.Visibility = Visibility.Hidden;
            NewVaultWindow w = new NewVaultWindow ();

            w.NewVaultWindowClosedOk +=
                new Action<string, string> ( w_NewVaultWindowClosedOk );
            w.Show ();
        }

        #region Child View Event Handlers

        void LockedView_VaultOpenSuccess ()
        {
            GoToOpenVaultState ();
        }

        #endregion

        #region Options-related event handlers

        private void OpenExistingVaultButton_Click ( object sender, RoutedEventArgs e )
        {
            OpenFileDialog ofd = new OpenFileDialog ();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DefaultExt = ".vault";
            ofd.Filter = "PasswordKeeper Vault files (*.vault)|*.vault";
            ofd.Multiselect = false;
            ofd.Title = "Choose the .vault file where you previously created your vault.";

            if ( ofd.ShowDialog () ?? false )
            {
                string filename = ofd.FileName;
                VaultManager.Instance.CloseVault ();
                VaultManager.SaveFileNameToRegistry ( filename );

                MessageBox.Show ( "Vault selected, now enter the password to attempt to open the vault.",
                    "Vault selected.", MessageBoxButton.OK, MessageBoxImage.Information );
                ( this.Resources[ "CloseOptionsSB" ] as Storyboard ).Begin ();
                GoToLockedVaultState ();
            }
        }

        private void CreateNewVaultButton_Click ( object sender, RoutedEventArgs e )
        {
            DoCreateNewVault ();
        }

        #endregion

        void w_NewVaultWindowClosedOk ( string newVaultFileName, string password )
        {
            this.Visibility = Visibility.Visible;

            try
            {
                VaultManager.Instance.CreateNewVault ( newVaultFileName, password );
                VaultManager.Instance.SaveVault ();
                VaultManager.Instance.CloseVault ();

                GoToLockedVaultState ();
                MessageBox.Show ( "Vault successfully created.  Now please login." );
            }
            catch ( Exception e )
            {
                MessageBox.Show (
                    "Error creating vault: '" + e.Message
                    + "'", "Error", MessageBoxButton.OK, MessageBoxImage.Error );
            }
        }

        private void GoToOpenVaultState ()
        {
            this.Visibility = Visibility.Hidden;
            VaultWindow = new VaultViewWindow ();
            VaultWindow.SelfClosed += new Action ( VaultWindow_SelfClosed );
            VaultWindow.Show ();
        }

        void VaultWindow_SelfClosed ()
        {
            this.Visibility = Visibility.Visible;
            this.LockedView.passwordBox.Password = "";
        }

        private void GoToLockedVaultState ()
        {
            if ( VaultWindow != null )
            {
                VaultWindow.Close ();
            }            
        }

        public void DragWindow ( object sender, MouseButtonEventArgs args )
        {
            DragMove ();
        }

        private VaultViewWindow VaultWindow;
	}
}