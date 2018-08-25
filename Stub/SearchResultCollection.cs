using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.DirectoryServices
{
    public class SearchResultCollection:IEnumerable
    {
        public int Count { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
