using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace PasswordKeeperWpf
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        public App ()
            : base ()
        {
            InitAppInstalledSetup ();
        }

        private void InitAppInstalledSetup ()
        {
        }
	}
}