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

namespace PasswordKeeperWpf.View
{
    /// <summary>
    /// Interaction logic for NewVaultEntryWindow.xaml
    /// </summary>
    public partial class NewVaultEntryWindow : Window
    {
        private Entry CurrentEntry;

        public NewVaultEntryWindow ()
        {
            InitializeComponent ();

            this.Mode = VaultEntryFormMode.Insert;
        }

        public NewVaultEntryWindow ( Entry e )
            : this ()
        {
            LoadEntry ( e );
            this.Mode = VaultEntryFormMode.Edit;
        }

        public VaultEntryFormMode Mode { get; set; }
        public event Action<Entry> NewEntryOk;

        private void LoadEntry ( Entry e )
        {
            this.CurrentEntry = e;
            titleTextBox.Text = e.Title;
            usernameTextBox.Text = e.Login;
            passwordBox.Password = e.Password;
            ClearPassword.Text = e.Password;
            notesTextBox.Text = e.Note;
            urlTextBox.Text = e.Url;
        }

        private void OkButton_Click ( object sender, RoutedEventArgs e )
        {
            if ( ValidateForm () )
            {
                if ( Mode == VaultEntryFormMode.Edit )
                {
                    this.CurrentEntry.Title = titleTextBox.Text;
                    this.CurrentEntry.Login = usernameTextBox.Text;
                    this.CurrentEntry.Password = passwordBox.Password;
                    this.CurrentEntry.Note = notesTextBox.Text;
                    this.CurrentEntry.Url = urlTextBox.Text;
                }
                else
                {
                    this.CurrentEntry = VaultManager.GenerateNewEntry (
                        titleTextBox.Text,
                        usernameTextBox.Text,
                        passwordBox.Password,
                        notesTextBox.Text,
                        urlTextBox.Text );
                }

                if ( NewEntryOk != null )
                {
                    NewEntryOk ( this.CurrentEntry );
                }
                this.Close ();
            }
        }

        private void CancelButton_Click ( object sender, RoutedEventArgs e )
        {
            this.CurrentEntry = null;
            this.Close ();
        }

        private void DeleteButton_Click ( object sender, RoutedEventArgs e )
        {
            this.Mode = VaultEntryFormMode.Delete;
            NewEntryOk ( this.CurrentEntry );
            this.Close ();
        }

        private bool ValidateForm ()
        {
            return titleTextBox != null;
        }

        private void ViewPasswordButton_Click ( object sender, RoutedEventArgs e )
        {
            
        }

        public void DragWindow ( object sender, MouseButtonEventArgs args )
        {
            DragMove ();
        }
    }

    public enum VaultEntryFormMode
    {
        Insert,
        Edit,
        Delete
    }
}
