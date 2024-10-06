using BPI.Tools;

namespace BPI
{
    public static class MainWindow
    {
        public static void Main(string[] args)
        {
            Patterns patterns = new Patterns();
            Core core = new Core();

            patterns.Load();

            core.ScanBinary("assembly", patterns.Configuration);
        }
    }
}