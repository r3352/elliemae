// Decompiled with JetBrains decompiler
// Type: RestoreAppLauncher.Properties.Resources
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace RestoreAppLauncher.Properties
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
        if (object.ReferenceEquals((object) RestoreAppLauncher.Properties.Resources.resourceMan, (object) null))
          RestoreAppLauncher.Properties.Resources.resourceMan = new ResourceManager("RestoreAppLauncher.Properties.Resources", typeof (RestoreAppLauncher.Properties.Resources).Assembly);
        return RestoreAppLauncher.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => RestoreAppLauncher.Properties.Resources.resourceCulture;
      set => RestoreAppLauncher.Properties.Resources.resourceCulture = value;
    }

    internal static byte[] ICSharpCode_SharpZipLib
    {
      get
      {
        return (byte[]) RestoreAppLauncher.Properties.Resources.ResourceManager.GetObject(nameof (ICSharpCode_SharpZipLib), RestoreAppLauncher.Properties.Resources.resourceCulture);
      }
    }
  }
}
