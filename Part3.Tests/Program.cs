using System;

namespace Part3.Tests
{
    internal static class Program
    {
        private static int passed, failed;
        [STAThread]
        private static int Main()
        {
            TreeTests.Run(); BalancedTreeTests.Run(); HeapTests.Run(); GraphTests.Run(); RequestIntegrationTests.Run();
            Console.WriteLine("TOTAL: " + passed + " passed, " + failed + " failed"); return failed == 0 ? 0 : 1;
        }
        internal static void Test(string name, Action action)
        {
            try { action(); passed++; Console.WriteLine("PASS " + name); }
            catch (Exception exception) { failed++; Console.WriteLine("FAIL " + name + ": " + exception); }
        }
    }
}
