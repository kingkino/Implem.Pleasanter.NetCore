using System;
using System.Collections.Generic;
using System.Text;

namespace System.DirectoryServices
{
    public class DirectorySearcher
    {
        public DirectorySearcher()
        {

        }

        public DirectorySearcher(DirectoryEntry entry)
        {

        }

        public string Filter { get; set; }
        public SearchResult FindOne() => null;

        public int PageSize { get; set; }

        public SearchResultCollection FindAll() => null;
    }
}
