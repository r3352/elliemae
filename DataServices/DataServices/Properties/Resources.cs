// Decompiled with JetBrains decompiler
// Type: DataServices.Properties.Resources
// Assembly: DataServices, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 227B0203-DF45-468D-9C1B-FA6CED472E23
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataServices.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace DataServices.Properties
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
        if (DataServices.Properties.Resources.resourceMan == null)
          DataServices.Properties.Resources.resourceMan = new ResourceManager("DataServices.Properties.Resources", typeof (DataServices.Properties.Resources).Assembly);
        return DataServices.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => DataServices.Properties.Resources.resourceCulture;
      set => DataServices.Properties.Resources.resourceCulture = value;
    }
  }
}
