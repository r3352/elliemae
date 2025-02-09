// Decompiled with JetBrains decompiler
// Type: AppLauncher.Properties.Resources
// Assembly: AppLauncher, Version=4.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 332B99CD-CD6E-4691-BDB2-CE964521D35B
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\AppLauncher.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace AppLauncher.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (AppLauncher.Properties.Resources.resourceMan == null)
          AppLauncher.Properties.Resources.resourceMan = new ResourceManager("AppLauncher.Properties.Resources", typeof (AppLauncher.Properties.Resources).Assembly);
        return AppLauncher.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => AppLauncher.Properties.Resources.resourceCulture;
      set => AppLauncher.Properties.Resources.resourceCulture = value;
    }
  }
}
