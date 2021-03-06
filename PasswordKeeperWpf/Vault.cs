﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using PasswordKeeperCore.Encryption;

namespace PasswordKeeperWpf
{
    public class Vault
    {
        public Vault ( string password )
        {
            Password = password;
            Entries = new ObservableCollection<Entry> ();

            isUnsavedChanges = false;
        }

        public Vault ( string filename, string password )
        {
            Password = password;
            OpenVault ( filename );

            isUnsavedChanges = false;
        }

        #region Open/Close/Save Vault

        public void OpenVault ( string filename )
        {
            // open file
            FileStream inputFile = new FileStream ( filename, FileMode.Open );
            byte [] encryptedVault = new byte [ inputFile.Length ];
            inputFile.Read ( encryptedVault, 0, ( int ) inputFile.Length );
            inputFile.Close ();

            IEncryptionUtility decryptor =
                EncryptionUtilityFactory.BuildEncryptionUtility ();

            byte [] serializedVault;

            try
            {
                /// Decrypt
                serializedVault = decryptor.DecryptBinary (
                    encryptedVault, this.Password );

                MemoryStream stream = new MemoryStream ( serializedVault );

                // Deserialize
                BinaryFormatter deserializer = new BinaryFormatter ();
                this.Entries = ( ObservableCollection<Entry> ) deserializer.Deserialize ( stream );
            }
            catch ( CryptographicException )
            {
                throw new VaultOpeningException (
                    "Unable to decrypt vault, check your password." );
            }
            catch ( SerializationException )
            {
                throw new VaultOpeningException (
                    "Unable to open vault, check your password, " +
                    "or the file may be corrupt." );
            }
        }

        public void SaveVault ( string filename )
        {
            /// Serialize
            MemoryStream stream = new MemoryStream ();
            BinaryFormatter serializer = new BinaryFormatter ();
            serializer.Serialize ( stream, Entries );

            byte [] serializedVault = stream.ToArray ();

            /// Encrypt
            IEncryptionUtility encryptor =
                EncryptionUtilityFactory.BuildEncryptionUtility ();
            byte [] encryptedVault = encryptor.EncryptBinary (
                serializedVault, this.Password );

            /// Save encrypted contents to file, overwritting if exists
            FileStream outputFile = new FileStream ( filename, FileMode.Create );
            outputFile.Write ( encryptedVault, 0, encryptedVault.Length );
            outputFile.Close ();

            isUnsavedChanges = false;
        }

        public void CloseVault ()
        {
            foreach ( Entry e in Entries )
            {
                e.Password = null;
            }
            Entries.Clear ();
            this.Password = null;

            isUnsavedChanges = false;
        } 

        #endregion

        #region Add/Update/Delete/Rename Entries

        public void AddEntry ( Entry e )
        {
            if ( !Entries.Any ( p => p.Title == e.Title ) )
            {
                Entries.Add ( e );
                
                isUnsavedChanges = true;
            }
            else
            {
                throw new ArgumentException ( 
                    "An entry with the title, '" +
                    e.Title + "' already exists.  Titles must be unique" );
            }
        }

        public void UpdateEntry ( Entry e )
        {
            Entry existingEntry = Entries.FirstOrDefault ( p => p.Id == e.Id );
            if ( existingEntry != null )
            {
                existingEntry.Login = e.Login;
                existingEntry.Password = e.Password;
                existingEntry.Note = e.Note;

                isUnsavedChanges = true;
            }
            else
            {
                throw new ArgumentException (
                    "Cannot find an entry with the title, '" +
                    e.Title + "'. Update Failed." );
            }
        }

        public void DeleteEntry ( Entry e )
        {
            // find first in case fields (other than Id) have been edited)
            Entry toDel = Entries.First<Entry> ( p => p.Id == e.Id );
            Entries.Remove ( toDel );
        }

        #endregion

        #region Public Properties

        public bool IsUnsavedChanges
        {
            get { return isUnsavedChanges; }
        }

        public ObservableCollection<Entry> EntriesList
        {
            get { return this.Entries; }
        } 

        #endregion

        private ObservableCollection<Entry> Entries;
        private string Password;

        private bool isUnsavedChanges;
    }

    public class VaultOpeningException : Exception
    {
        public VaultOpeningException ( string msg )
        {
            MyMessage = msg;
        }

        public string MyMessage;
    }
}
