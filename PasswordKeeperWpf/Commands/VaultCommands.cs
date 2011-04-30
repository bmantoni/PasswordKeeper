using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows.Input;

namespace PasswordKeeperWpf.Commands
{
    public static class VaultCommands
    {
        private static Dispatcher _dispatcher;

        static VaultCommands ()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        private static RoutedUICommand openVaultCommand = new RoutedUICommand ( "Open Vault", "OpenVault", typeof ( VaultCommands ) );
        private static RoutedUICommand closeVaultCommand = new RoutedUICommand ( "Close Vault", "CloseVault", typeof ( VaultCommands ) );

        public static RoutedUICommand OpenVault
        {
            get { return openVaultCommand; }
        }

        public static RoutedUICommand CloseVault
        {
            get { return closeVaultCommand; }
        }
    }
}
