// Decompiled with JetBrains decompiler
// Type: ScApp.Program
// Assembly: ScApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D01870D-1104-485C-B90D-F422F6F3402A
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\ScApp\ScApp.exe

using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace ScApp
{
  internal static class Program
  {
    [STAThread]
    private static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      List<string> stringList = new List<string>();
      if (args != null && args.Length > 0)
        stringList.AddRange((IEnumerable<string>) args);
      stringList.Add("-SCID");
      stringList.Add("ScApp");
      AssemblyResolver.Start(stringList.ToArray());
      AssemblyResolver.FirstSmartClientAssembly = "ScAppImpl";
      Program.start(args);
    }

    private static void start(string[] args) => ScAppImpl.Main.main(args);
  }
}
