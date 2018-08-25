using System;
using System.Collections.Generic;
using System.Text;

namespace System.DirectoryServices
{
    public class DirectoryEntry
    {
        public DirectoryEntry()
        {

        }
        public DirectoryEntry(string init)
        {

        }

        public DirectoryEntry(string init, string loginId, string password)
        {

        }

        public string Path { get; set; }

        public IDictionary<string, KeyValuePair<string,string>> Properties => null;
    }
}
