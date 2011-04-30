using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordKeeperWpf
{
    [Serializable]
    public class Entry
    {
        public Entry ( int id, string t, string l, string p, string n, string u )
        {
            Id = id;
            Title = t;
            Login = l;
            Password = p;
            Note = n;
            Url = u;
        }

        public override bool Equals ( object obj )
        {
            if ( !( typeof ( Entry ).IsAssignableFrom ( obj.GetType () ) ) )
            {
                return base.Equals ( obj );
            }
            else
            {
                return ( obj as Entry ).Title == this.Title;
            }
        }

        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }

        public override string ToString ()
        {
            return Title;
        }

        public int Id { get; set; }

        public string Title { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Note { get; set; }
        public string Url { get; set; }
    }
}
