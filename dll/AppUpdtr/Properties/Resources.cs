// Decompiled with JetBrains decompiler
// Type: AppUpdtr.Properties.Resources
// Assembly: AppUpdtr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3ADBA3EC-518B-4BBF-94A2-A2027DADA3FC
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\AppUpdtr.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace AppUpdtr.Properties
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
        if (AppUpdtr.Properties.Resources.resourceMan == null)
          AppUpdtr.Properties.Resources.resourceMan = new ResourceManager("AppUpdtr.Properties.Resources", typeof (AppUpdtr.Properties.Resources).Assembly);
        return AppUpdtr.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => AppUpdtr.Properties.Resources.resourceCulture;
      set => AppUpdtr.Properties.Resources.resourceCulture = value;
    }

    internal static string Encompass_exe_config
    {
      get
      {
        return AppUpdtr.Properties.Resources.ResourceManager.GetString(nameof (Encompass_exe_config), AppUpdtr.Properties.Resources.resourceCulture);
      }
    }

    internal static byte[] RestoreAppLauncher
    {
      get
      {
        return (byte[]) AppUpdtr.Properties.Resources.ResourceManager.GetObject(nameof (RestoreAppLauncher), AppUpdtr.Properties.Resources.resourceCulture);
      }
    }
  }
}
