// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Properties.Resources
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace EllieMae.EMLite.Server.Properties
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
        if (EllieMae.EMLite.Server.Properties.Resources.resourceMan == null)
          EllieMae.EMLite.Server.Properties.Resources.resourceMan = new ResourceManager("EllieMae.EMLite.Server.Properties.Resources", typeof (EllieMae.EMLite.Server.Properties.Resources).Assembly);
        return EllieMae.EMLite.Server.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => EllieMae.EMLite.Server.Properties.Resources.resourceCulture;
      set => EllieMae.EMLite.Server.Properties.Resources.resourceCulture = value;
    }

    internal static string BlankLoan
    {
      get => EllieMae.EMLite.Server.Properties.Resources.ResourceManager.GetString(nameof (BlankLoan), EllieMae.EMLite.Server.Properties.Resources.resourceCulture);
    }

    internal static string Consent
    {
      get => EllieMae.EMLite.Server.Properties.Resources.ResourceManager.GetString(nameof (Consent), EllieMae.EMLite.Server.Properties.Resources.resourceCulture);
    }

    internal static string LoanStatusEmailTemplate
    {
      get
      {
        return EllieMae.EMLite.Server.Properties.Resources.ResourceManager.GetString(nameof (LoanStatusEmailTemplate), EllieMae.EMLite.Server.Properties.Resources.resourceCulture);
      }
    }
  }
}
