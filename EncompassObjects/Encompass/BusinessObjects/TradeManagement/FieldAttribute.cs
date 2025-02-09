// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.FieldAttribute
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  internal class FieldAttribute : Attribute
  {
    private string name;

    public string Name => this.name;

    public FieldAttribute(string name) => this.name = name;
  }
}
