using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Storage;

namespace ImageApp.UnitTests.ImageBrowserPage
{
    public class FileModifiedComparer<T> : IComparer<T>
    {
        public async void PrepareToOrdering(List<T> files)
        {
        }

        public int Compare(T firstFile, T secondFile)
        {
            throw new NotImplementedException();
        }
    }
}
