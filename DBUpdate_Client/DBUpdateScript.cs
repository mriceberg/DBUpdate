using System.IO;

namespace DBUpdate_Client
{
    public class DBUpdateScript
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string FullPath => System.IO.Path.Combine(Path, Name);

        public bool Exists => File.Exists(FullPath);

        public DBUpdateScript(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }
    }
}
