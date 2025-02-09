// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Properties.Resources
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace EllieMae.EMLite.Common.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
        if (EllieMae.EMLite.Common.Properties.Resources.resourceMan == null)
          EllieMae.EMLite.Common.Properties.Resources.resourceMan = new ResourceManager("EllieMae.EMLite.Common.Properties.Resources", typeof (EllieMae.EMLite.Common.Properties.Resources).Assembly);
        return EllieMae.EMLite.Common.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => EllieMae.EMLite.Common.Properties.Resources.resourceCulture;
      set => EllieMae.EMLite.Common.Properties.Resources.resourceCulture = value;
    }

    internal static string AEpky
    {
      get => EllieMae.EMLite.Common.Properties.Resources.ResourceManager.GetString(nameof (AEpky), EllieMae.EMLite.Common.Properties.Resources.resourceCulture);
    }
  }
}
