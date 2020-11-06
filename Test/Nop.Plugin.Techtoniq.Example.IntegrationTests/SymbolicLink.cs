using System.Diagnostics;

namespace Nop.Plugin.Techtoniq.Example.IntegrationTests
{
    public static class SymbolicLink
    {
        public static void CreateJunction(string linkFolderName, string targetFolderName)
        {
            var args = " /C mklink /J \"" + linkFolderName + "\" \"" + targetFolderName + "\"";
            var psi = new ProcessStartInfo("cmd.exe", args);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi).WaitForExit();
        }
    }
}