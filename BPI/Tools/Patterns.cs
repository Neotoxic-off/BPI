using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPI.Models;
using BPI.Models.IO;
using Newtonsoft.Json;

namespace BPI.Tools
{
    public class Patterns
    {
        private FileModel? path { get; set; }
        public ConfigurationModel? Configuration { get; set; }

        public Patterns()
        {
            path = new FileModel("patterns.json", true);
        }

        public void Load()
        {
            string buffer = String.Empty;

            if (path.Exists == true)
            {
                buffer = File.ReadAllText(path.Path);
                Configuration = JsonConvert.DeserializeObject<ConfigurationModel>(buffer);
            }
        }
    }
}
