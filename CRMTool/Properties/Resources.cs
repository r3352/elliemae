// Decompiled with JetBrains decompiler
// Type: CRMTool.Properties.Resources
// Assembly: CRMTool, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: C4E26DB0-5EEF-43E1-8127-BF24D4B06853
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\CRMTool.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace CRMTool.Properties
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
        if (CRMTool.Properties.Resources.resourceMan == null)
          CRMTool.Properties.Resources.resourceMan = new ResourceManager("CRMTool.Properties.Resources", typeof (CRMTool.Properties.Resources).Assembly);
        return CRMTool.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => CRMTool.Properties.Resources.resourceCulture;
      set => CRMTool.Properties.Resources.resourceCulture = value;
    }
  }
}
