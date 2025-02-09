// Decompiled with JetBrains decompiler
// Type: Elli.SQE.DD.Mapping.RepeatableFieldTag
// Assembly: Elli.SQE.DD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD1B11DD-560E-4083-B59A-D629AF47C790
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.DD.dll

using Elli.Common.Fields;

#nullable disable
namespace Elli.SQE.DD.Mapping
{
  public class RepeatableFieldTag
  {
    public string TemplateFieldId { get; set; }

    public static RepeatableFieldTag CreateIfRepeatable(LoanHierarchy current, EncompassField field)
    {
      LoanCollectionHierarchy collectionHierarchy = current.CastAs<LoanCollectionHierarchy>();
      if (collectionHierarchy.IsNull<LoanCollectionHierarchy>())
        return (RepeatableFieldTag) null;
      if (!collectionHierarchy.IsValidRepeatableCollection())
        return (RepeatableFieldTag) null;
      return new RepeatableFieldTag()
      {
        TemplateFieldId = field.ID
      };
    }
  }
}
