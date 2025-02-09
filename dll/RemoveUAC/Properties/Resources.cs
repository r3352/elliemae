// Decompiled with JetBrains decompiler
// Type: RemoveUAC.Properties.Resources
// Assembly: RemoveUAC, Version=4.5.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 77B208E8-E0D8-4A0C-958C-E5CF190AB691
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RemoveUAC.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace RemoveUAC.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
  [DebuggerNonUserCode]
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
        if (object.ReferenceEquals((object) RemoveUAC.Properties.Resources.resourceMan, (object) null))
          RemoveUAC.Properties.Resources.resourceMan = new ResourceManager("RemoveUAC.Properties.Resources", typeof (RemoveUAC.Properties.Resources).Assembly);
        return RemoveUAC.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => RemoveUAC.Properties.Resources.resourceCulture;
      set => RemoveUAC.Properties.Resources.resourceCulture = value;
    }
  }
}
