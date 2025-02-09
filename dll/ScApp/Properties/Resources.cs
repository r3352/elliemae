// Decompiled with JetBrains decompiler
// Type: ScApp.Properties.Resources
// Assembly: ScApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D01870D-1104-485C-B90D-F422F6F3402A
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\ScApp\ScApp.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace ScApp.Properties
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
        if (ScApp.Properties.Resources.resourceMan == null)
          ScApp.Properties.Resources.resourceMan = new ResourceManager("ScApp.Properties.Resources", typeof (ScApp.Properties.Resources).Assembly);
        return ScApp.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => ScApp.Properties.Resources.resourceCulture;
      set => ScApp.Properties.Resources.resourceCulture = value;
    }
  }
}
