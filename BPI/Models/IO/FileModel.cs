using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPI.Models.IO
{
    public class FileModel
    {
        public string Path { get; set; }
        public bool Exists { get; set; }

        public FileModel(string path, bool create)
        {
            Path = path;
            Exists = File.Exists(path);

            if (create == true)
                Create();

        }

        private void Create()
        {
            if (Exists == false)
                File.Create(Path);
            Exists = true;
        }
    }
}
