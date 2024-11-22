using ShareYourText.Singleton;


namespace ShareYourText
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(FormCreater.GetForm());
        }
    }
}

