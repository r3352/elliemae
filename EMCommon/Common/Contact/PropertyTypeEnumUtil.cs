// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.PropertyTypeEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class PropertyTypeEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;
    private static Hashtable _ValueToNameInLoan;

    static PropertyTypeEnumUtil()
    {
      PropertyTypeEnumUtil._NameToValue.Add((object) "", (object) PropertyType.Blank);
      PropertyTypeEnumUtil._NameToValue.Add((object) "Attached", (object) PropertyType.Attached);
      PropertyTypeEnumUtil._NameToValue.Add((object) "Condominium", (object) PropertyType.Condominium);
      PropertyTypeEnumUtil._NameToValue.Add((object) "Condo", (object) PropertyType.Condominium);
      PropertyTypeEnumUtil._NameToValue.Add((object) "Co-Operative", (object) PropertyType.Coop);
      PropertyTypeEnumUtil._NameToValue.Add((object) "CoOp", (object) PropertyType.Coop);
      PropertyTypeEnumUtil._NameToValue.Add((object) "Detached", (object) PropertyType.Detached);
      PropertyTypeEnumUtil._NameToValue.Add((object) "High Rise Condominium", (object) PropertyType.HighRiseCondo);
      PropertyTypeEnumUtil._NameToValue.Add((object) "HighRiseCondo", (object) PropertyType.HighRiseCondo);
      PropertyTypeEnumUtil._NameToValue.Add((object) "Manufactured Housing", (object) PropertyType.MfdHousing);
      PropertyTypeEnumUtil._NameToValue.Add((object) "ManufacturedHousing", (object) PropertyType.MfdHousing);
      PropertyTypeEnumUtil._NameToValue.Add((object) "PUD", (object) PropertyType.PUD);
      PropertyTypeEnumUtil._NameToValue.Add((object) "Detached Condo", (object) PropertyType.DetachedCondo);
      PropertyTypeEnumUtil._NameToValue.Add((object) "DetachedCondo", (object) PropertyType.DetachedCondo);
      PropertyTypeEnumUtil._NameToValue.Add((object) "Mfd Home/Condo/PUD/Co-Op", (object) PropertyType.MfdCondoPUDCoop);
      PropertyTypeEnumUtil._NameToValue.Add((object) "ManufacturedHomeCondoPUDCoOp", (object) PropertyType.MfdCondoPUDCoop);
      PropertyTypeEnumUtil._NameToValue.Add((object) "MH Select", (object) PropertyType.MHSelect);
      PropertyTypeEnumUtil._NameToValue.Add((object) "MHSelect", (object) PropertyType.MHSelect);
      PropertyTypeEnumUtil._ValueToName = new Hashtable();
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.Blank, (object) "");
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.Attached, (object) "Attached");
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.Condominium, (object) "Condominium");
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.Coop, (object) "Co-Operative");
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.Detached, (object) "Detached");
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.HighRiseCondo, (object) "High Rise Condominium");
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.MfdHousing, (object) "Manufactured Housing");
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.PUD, (object) "PUD");
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.DetachedCondo, (object) "Detached Condo");
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.MfdCondoPUDCoop, (object) "Mfd Home/Condo/PUD/Co-Op");
      PropertyTypeEnumUtil._ValueToName.Add((object) PropertyType.MHSelect, (object) "MH Select");
      PropertyTypeEnumUtil._ValueToNameInLoan = new Hashtable();
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.Blank, (object) "");
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.Attached, (object) "Attached");
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.Condominium, (object) "Condominium");
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.Coop, (object) "Cooperative");
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.Detached, (object) "Detached");
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.HighRiseCondo, (object) "HighRiseCondominium");
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.MfdHousing, (object) "ManufacturedHousing");
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.PUD, (object) "PUD");
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.DetachedCondo, (object) "DetachedCondo");
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.MfdCondoPUDCoop, (object) "ManufacturedHomeCondoPUDCoOp");
      PropertyTypeEnumUtil._ValueToNameInLoan.Add((object) PropertyType.MHSelect, (object) "MHSelect");
    }

    public static IDictionary ValueToNameMap => (IDictionary) PropertyTypeEnumUtil._ValueToName;

    public static object[] GetDisplayNames()
    {
      PropertyType[] values = (PropertyType[]) Enum.GetValues(typeof (PropertyType));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) PropertyTypeEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(PropertyType val)
    {
      return (string) PropertyTypeEnumUtil._ValueToName[(object) val];
    }

    public static string ValueToNameInLoan(PropertyType val)
    {
      return (string) PropertyTypeEnumUtil._ValueToNameInLoan[(object) val];
    }

    public static PropertyType NameToValue(string name)
    {
      return PropertyTypeEnumUtil._NameToValue.Contains((object) name) ? (PropertyType) PropertyTypeEnumUtil._NameToValue[(object) name] : PropertyType.Blank;
    }
  }
}
