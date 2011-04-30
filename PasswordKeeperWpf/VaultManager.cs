using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace PasswordKeeperWpf
{
    public class VaultManager
    {
        private static readonly VaultManager _instance = new VaultManager ();

        static VaultManager ()
        {
        }

        private VaultManager ()
        {
            CurrentVault = null;

            try
            {
                GetVaultFileNameFromRegistry ();
            }
            catch ( VaultOpeningException )
            {
                CurrentVault = null;
                CurrentVaultFileName = null;
            }
        }

        public static VaultManager Instance { get { return _instance; } }

        #region Vault Management

        public void CreateNewVault ( string fn, string pw )
        {
            CurrentVault = new Vault ( pw );
            CurrentVaultFileName = fn;
        }

        public void OpenVault ( string fn, string pw )
        {
            CloseVault ();
            CurrentVaultFileName = fn;
            OpenVault ( pw );
        }

        public void OpenVault ( string pw )
        {
            try
            {
                CurrentVault = new Vault ( CurrentVaultFileName, pw );
            }
            catch ( VaultOpeningException )
            {
                MessageBox.Show ( 
                    "Invalid Password, please try again", 
                    "Error Opening Vault", 
                    MessageBoxButton.OK, MessageBoxImage.Hand );
            }
        }

        public void GetVaultFileNameFromRegistry ()
        {
            CurrentVaultFileName = LoadLastVaultFileName ();
        }

        public void SaveVault ()
        {
            CurrentVault.SaveVault ( CurrentVaultFileName );
            SaveVaultFileNameToRegistry ();
        }

        public void CloseVault ()
        {
            if ( CurrentVault != null )
            {
                CurrentVault.CloseVault ();
            }
            CurrentVault = null;
        } 

        #endregion

        #region Password Management

        public string GetPassword ()
        {
            string password;
            bool keepAsking = true;
            do
            {
                password = PromptPassword ();

                if ( password != null && !MeetsPasswordCriteria ( password ) )
                {
                    MessageBox.Show (
                        "Your password doesn't meet the minimum requirements",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Stop );
                }
                else
                {
                    keepAsking = false;
                }
            } while ( keepAsking );
            return password;
        }

        public string PromptPassword ()
        {
            string pass = null;

            //PasswordPromptForm passForm = new PasswordPromptForm ();

            //if ( passForm.ShowDialog () == DialogResult.OK )
            //{
            //    pass = passForm.EnteredPassword;
            //}

            return pass;
        }

        public static bool IsValidPassword ( string pw )
        {
            return MeetsPasswordCriteria ( pw );
        }

        private static bool MeetsPasswordCriteria ( string password )
        {
            return password.Length >= 8;
        }

        #endregion

        #region Registry Management

        private string LoadLastVaultFileName ()
        {
            string lastKnownFileName = null;

            RegistryKey CurrentRegistryKey = Registry.CurrentUser;

            try
            {
                CurrentRegistryKey = CurrentRegistryKey.CreateSubKey
                    ( "Software" );
                CurrentRegistryKey = CurrentRegistryKey.CreateSubKey
                    ( "BodhiSoftware" );
                CurrentRegistryKey = CurrentRegistryKey.CreateSubKey
                    ( "PasswordKeeper" );
                string fileName =
                    CurrentRegistryKey.GetValue
                    ( "LastVaultFilename" ).ToString ();

                if ( File.Exists ( fileName ) )
                {
                    lastKnownFileName = fileName;
                }
            }
            catch ( System.Security.SecurityException )
            {
                lastKnownFileName = null;
            }
            catch ( ArgumentException )
            {
                lastKnownFileName = null;
            }
            catch ( ObjectDisposedException )
            {
                lastKnownFileName = null;
            }
            catch ( UnauthorizedAccessException )
            {
                lastKnownFileName = null;
            }
            catch ( Exception )
            {
                lastKnownFileName = null;
            }
            finally
            {
                if ( CurrentRegistryKey != null )
                {
                    CurrentRegistryKey.Close ();
                }
            }

            return lastKnownFileName;
        }

        private void SaveVaultFileNameToRegistry ()
        {
            SaveFileNameToRegistry ( CurrentVaultFileName );
        }

        public static void SaveFileNameToRegistry ( string vaultFileName )
        {
            RegistryKey CurrentRegistryKey = Registry.CurrentUser;
            try
            {
                CurrentRegistryKey = CurrentRegistryKey.CreateSubKey
                    ( "Software" );
                CurrentRegistryKey = CurrentRegistryKey.CreateSubKey
                    ( "BodhiSoftware" );
                CurrentRegistryKey = CurrentRegistryKey.CreateSubKey
                    ( "PasswordKeeper" );
                CurrentRegistryKey.SetValue
                    ( "LastVaultFilename", vaultFileName );
            }
            catch ( UnauthorizedAccessException e )
            {
                MessageBox.Show ( "Unauthorized Access: " + e.Message );
            }
            catch ( System.Security.SecurityException e )
            {
                MessageBox.Show ( "Security Exception: " + e.Message );
            }
            finally
            {
                CurrentRegistryKey.Close ();
            }
        }

        #endregion

        #region Entry Id Management

        public int GetNextEntryId ()
        {
            if ( this.CurrentVault == null ||
                this.CurrentVault.EntriesList.Count < 1 )
            {
                return 1;
            }
            else
            {
                return this.CurrentVault.EntriesList.Count + 1;
            }
        }

        #endregion

        public static Entry GenerateNewEntry (
            string title,
            string username,
            string password,
            string notes,
            string url )
        {
            return new Entry (
                VaultManager.Instance.GetNextEntryId (),
                title, username, password, notes, url );
        }

        public Vault CurrentVault { get; set; }

        public string CurrentVaultFileName { get; set; }
    }
}
