// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AnimatedProgessResource
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace EllieMae.EMLite
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class AnimatedProgessResource
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal AnimatedProgessResource()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (AnimatedProgessResource.resourceMan == null)
          AnimatedProgessResource.resourceMan = new ResourceManager("EllieMae.EMLite.AnimatedProgessResource", typeof (AnimatedProgessResource).Assembly);
        return AnimatedProgessResource.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => AnimatedProgessResource.resourceCulture;
      set => AnimatedProgessResource.resourceCulture = value;
    }

    internal static Bitmap progress
    {
      get
      {
        return (Bitmap) AnimatedProgessResource.ResourceManager.GetObject(nameof (progress), AnimatedProgessResource.resourceCulture);
      }
    }
  }
}
