namespace DTMFEmulator
{
    internal static class Program
    {
        public static MainForm main = new();
        public static Graph graph = new();
        public static Setting setting = new();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(main);
        }
    }
}