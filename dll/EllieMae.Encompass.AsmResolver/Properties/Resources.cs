// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Properties.Resources
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Properties
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
        if (EllieMae.Encompass.AsmResolver.Properties.Resources.resourceMan == null)
          EllieMae.Encompass.AsmResolver.Properties.Resources.resourceMan = new ResourceManager("EllieMae.Encompass.AsmResolver.Properties.Resources", typeof (EllieMae.Encompass.AsmResolver.Properties.Resources).Assembly);
        return EllieMae.Encompass.AsmResolver.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => EllieMae.Encompass.AsmResolver.Properties.Resources.resourceCulture;
      set => EllieMae.Encompass.AsmResolver.Properties.Resources.resourceCulture = value;
    }

    internal static Bitmap File
    {
      get => (Bitmap) EllieMae.Encompass.AsmResolver.Properties.Resources.ResourceManager.GetObject(nameof (File), EllieMae.Encompass.AsmResolver.Properties.Resources.resourceCulture);
    }

    internal static Bitmap File_Disabled
    {
      get
      {
        return (Bitmap) EllieMae.Encompass.AsmResolver.Properties.Resources.ResourceManager.GetObject(nameof (File_Disabled), EllieMae.Encompass.AsmResolver.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap IIS
    {
      get => (Bitmap) EllieMae.Encompass.AsmResolver.Properties.Resources.ResourceManager.GetObject(nameof (IIS), EllieMae.Encompass.AsmResolver.Properties.Resources.resourceCulture);
    }

    internal static Bitmap IIS_Disabled
    {
      get
      {
        return (Bitmap) EllieMae.Encompass.AsmResolver.Properties.Resources.ResourceManager.GetObject(nameof (IIS_Disabled), EllieMae.Encompass.AsmResolver.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Web
    {
      get => (Bitmap) EllieMae.Encompass.AsmResolver.Properties.Resources.ResourceManager.GetObject(nameof (Web), EllieMae.Encompass.AsmResolver.Properties.Resources.resourceCulture);
    }

    internal static Bitmap Web_Disabled
    {
      get
      {
        return (Bitmap) EllieMae.Encompass.AsmResolver.Properties.Resources.ResourceManager.GetObject(nameof (Web_Disabled), EllieMae.Encompass.AsmResolver.Properties.Resources.resourceCulture);
      }
    }
  }
}
