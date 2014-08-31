namespace ImageApp.ImageBrowserPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LastPathsManager
    {
        public readonly int Limit;

        public List<string> Paths { get; set; }

        public LastPathsManager(int limit)
        {
            Limit = limit;
            Paths = new List<string>();
        }

        public void Add(string path)
        {
            Paths.Insert(0, path);
            RemoveLastIfLimitIsExceed();
            RemoveDuplicates(path);
        }

        private void RemoveLastIfLimitIsExceed()
        {
            if (Paths.Count > Limit)
                Paths.RemoveAt(Limit);
        }

        private void RemoveDuplicates(string path)
        {
            for (int i = 1; i < Paths.Count; i++)
                if (Paths[i] == path)
                    Paths.RemoveAt(i);
        }
    }
}
