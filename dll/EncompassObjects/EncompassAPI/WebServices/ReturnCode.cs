// Decompiled with JetBrains decompiler
// Type: EllieMae.EncompassAPI.WebServices.ReturnCode
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EncompassAPI.WebServices
{
  [GeneratedCode("System.Xml", "4.8.3752.0")]
  [XmlType(Namespace = "http://hosted.elliemae.com/")]
  [Serializable]
  public enum ReturnCode
  {
    Success,
    AuthenticationFailed,
    UnhandledError,
  }
}
