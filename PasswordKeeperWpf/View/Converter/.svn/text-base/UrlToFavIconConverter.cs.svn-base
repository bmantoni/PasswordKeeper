using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;

namespace PasswordKeeperWpf.View.Converter
{
    public class UrlToFavIconConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert ( 
            object value, 
            Type targetType, 
            object parameter, 
            System.Globalization.CultureInfo culture )
        {
            string url = value as string;

            string domainUrl = Regex.Replace ( url, "^http://", "" );
            if ( domainUrl.Contains ( "/" ) )
            {
                domainUrl = domainUrl.Substring ( 0, domainUrl.IndexOf ( '/' ) + 1 );
            }

            return new BitmapImage ( new Uri ( "http://google.com/s2/favicons?domain=" + domainUrl ) );
        }

        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException ();
        }

        #endregion
    }
}
