// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CustomFields.CustomFieldsTypeNameProvider
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.CustomFields
{
  public class CustomFieldsTypeNameProvider : CustomEnumNameProvider
  {
    private static Hashtable valueToNameMap = new Hashtable();

    static CustomFieldsTypeNameProvider()
    {
      CustomFieldsTypeNameProvider.valueToNameMap.Add((object) CustomFieldsType.None, (object) "None");
      CustomFieldsTypeNameProvider.valueToNameMap.Add((object) CustomFieldsType.Borrower, (object) "Borrower Contact Custom Fields");
      CustomFieldsTypeNameProvider.valueToNameMap.Add((object) CustomFieldsType.BizPartner, (object) "Business Contact Custom Fields");
      CustomFieldsTypeNameProvider.valueToNameMap.Add((object) CustomFieldsType.BizCategoryCustom, (object) "Business Category Custom Fields");
      CustomFieldsTypeNameProvider.valueToNameMap.Add((object) CustomFieldsType.BizCategoryStandard, (object) "Business Category Standard Fields");
    }

    public CustomFieldsTypeNameProvider()
      : base(typeof (CustomFieldsType), CustomFieldsTypeNameProvider.valueToNameMap)
    {
    }
  }
}
