// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactCustomField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Represents a custom field for a contact.</summary>
  /// <example>
  /// The following code lists all of the custom fields associated with a contact
  /// along with the value of each.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Contacts;
  /// 
  /// class ContactManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Retrieve a borrower based on a command-line argument
  ///       Contact contact = session.Contacts.Open(int.Parse(args[0]), ContactType.Borrower);
  /// 
  ///       // Loop over the defined custom fields
  ///       foreach (ContactCustomField field in contact.CustomFields)
  ///       {
  ///          // Display the name of the field and the value for this contact
  ///          Console.WriteLine(field.Name + " = " + field.Value);
  /// 
  ///          // If this is a drop-down field, display the possible options
  ///          if ((field.Type == CustomFieldType.DROPDOWN) ||
  ///             (field.Type == CustomFieldType.DROPDOWN_EDITABLE))
  ///          {
  ///             for (int i = 0; i < field.PossibleValues.Count; i++)
  ///                Console.WriteLine("   " + field.PossibleValues[i]);
  ///          }
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class ContactCustomField : SessionBoundObject, IContactCustomField
  {
    private Contact contact;
    private string fieldValue;
    private ContactCustomFieldInfo fieldInfo;
    private StringList valueList;

    internal ContactCustomField(
      Contact contact,
      EllieMae.EMLite.ClientServer.Contacts.ContactCustomField field,
      ContactCustomFieldInfo fieldInfo)
      : base(contact.Session)
    {
      this.contact = contact;
      this.fieldInfo = fieldInfo;
      this.fieldValue = field != null ? field.FieldValue : "";
      if (fieldInfo.FieldOptions == null)
        return;
      this.valueList = (StringList) new ContactCustomField.ReadOnlyStringList(fieldInfo.FieldOptions);
    }

    /// <summary>Gets or sets the value of the custom field.</summary>
    /// <remarks>
    /// When setting this property, the value will be automatically reformatted or converted,
    /// if possible, based on the <see cref="P:EllieMae.Encompass.BusinessObjects.Contacts.ContactCustomField.Type" /> of the field. For example, if the property
    /// has type <see cref="F:EllieMae.Encompass.BusinessObjects.Contacts.CustomFieldType.ZIPCODE" /> and you set the Value to "123456789", it will automatically be
    /// reformatted as "12345-6789".
    /// <p>
    /// If you attempt to set this property to a value which is cannot be converted to a value which is appropriate for the
    /// field's <see cref="P:EllieMae.Encompass.BusinessObjects.Contacts.ContactCustomField.Type" />, an exception will be raised. For example, attempting to
    /// set the Value to "X" for a field whose type is <see cref="F:EllieMae.Encompass.BusinessObjects.Contacts.CustomFieldType.YN" /> will
    /// cause an exception to be raised. Similarly, if the field's <see cref="P:EllieMae.Encompass.BusinessObjects.Contacts.ContactCustomField.Type" /> is
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Contacts.CustomFieldType.DROPDOWN" />, then attempting to set the field's value
    /// to something other than one of the values in the <see cref="P:EllieMae.Encompass.BusinessObjects.Contacts.ContactCustomField.PossibleValues" /> list
    /// (or to the empty string) will result in an exception.
    /// </p>
    /// </remarks>
    /// <example>
    /// The following code lists all of the custom fields associated with a contact
    /// along with the value of each.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Retrieve a borrower based on a command-line argument
    ///       Contact contact = session.Contacts.Open(int.Parse(args[0]), ContactType.Borrower);
    /// 
    ///       // Loop over the defined custom fields
    ///       foreach (ContactCustomField field in contact.CustomFields)
    ///       {
    ///          // Display the name of the field and the value for this contact
    ///          Console.WriteLine(field.Name + " = " + field.Value);
    /// 
    ///          // If this is a drop-down field, display the possible options
    ///          if ((field.Type == CustomFieldType.DROPDOWN) ||
    ///             (field.Type == CustomFieldType.DROPDOWN_EDITABLE))
    ///          {
    ///             for (int i = 0; i < field.PossibleValues.Count; i++)
    ///                Console.WriteLine("   " + field.PossibleValues[i]);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string Value
    {
      get => this.fieldValue;
      set
      {
        value = value ?? "";
        if (this.Type == CustomFieldType.DROPDOWN && value != "")
        {
          bool flag = false;
          for (int index = 0; index < this.PossibleValues.Count && !flag; ++index)
          {
            if (value == this.PossibleValues[index])
              flag = true;
          }
          if (!flag)
            throw new ArgumentException("Specified value is not in the range of possible values.");
        }
        string str = value;
        if (value != "")
        {
          bool needsUpdate = false;
          str = Utils.FormatInput(value, this.fieldInfo.FieldType, ref needsUpdate);
          if (str == "")
            throw new ArgumentException("Specified value cannot be converted to the required type");
        }
        this.fieldValue = str;
      }
    }

    /// <summary>Gets the name of the custom field.</summary>
    /// <example>
    /// The following code lists all of the custom fields associated with a contact
    /// along with the value of each.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Retrieve a borrower based on a command-line argument
    ///       Contact contact = session.Contacts.Open(int.Parse(args[0]), ContactType.Borrower);
    /// 
    ///       // Loop over the defined custom fields
    ///       foreach (ContactCustomField field in contact.CustomFields)
    ///       {
    ///          // Display the name of the field and the value for this contact
    ///          Console.WriteLine(field.Name + " = " + field.Value);
    /// 
    ///          // If this is a drop-down field, display the possible options
    ///          if ((field.Type == CustomFieldType.DROPDOWN) ||
    ///             (field.Type == CustomFieldType.DROPDOWN_EDITABLE))
    ///          {
    ///             for (int i = 0; i < field.PossibleValues.Count; i++)
    ///                Console.WriteLine("   " + field.PossibleValues[i]);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string Name => this.fieldInfo.Label;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.CustomFieldType" /> of the field.
    /// </summary>
    /// <example>
    /// The following code lists all of the custom fields associated with a contact
    /// along with the value of each.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Retrieve a borrower based on a command-line argument
    ///       Contact contact = session.Contacts.Open(int.Parse(args[0]), ContactType.Borrower);
    /// 
    ///       // Loop over the defined custom fields
    ///       foreach (ContactCustomField field in contact.CustomFields)
    ///       {
    ///          // Display the name of the field and the value for this contact
    ///          Console.WriteLine(field.Name + " = " + field.Value);
    /// 
    ///          // If this is a drop-down field, display the possible options
    ///          if ((field.Type == CustomFieldType.DROPDOWN) ||
    ///             (field.Type == CustomFieldType.DROPDOWN_EDITABLE))
    ///          {
    ///             for (int i = 0; i < field.PossibleValues.Count; i++)
    ///                Console.WriteLine("   " + field.PossibleValues[i]);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public CustomFieldType Type => (CustomFieldType) this.fieldInfo.FieldType;

    /// <summary>
    /// Gets the list of possible values which the field may take on when
    /// the field's <see cref="P:EllieMae.Encompass.BusinessObjects.Contacts.ContactCustomField.Type" /> is either <see cref="F:EllieMae.Encompass.BusinessObjects.Contacts.CustomFieldType.DROPDOWN" /> or
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Contacts.CustomFieldType.DROPDOWN_EDITABLE" />.
    /// </summary>
    /// <remarks>
    /// The returned <see cref="T:EllieMae.Encompass.Collections.StringList" /> cannot be modified. Attempting to do so
    /// will result in an exception.
    /// </remarks>
    /// <example>
    /// The following code lists all of the custom fields associated with a contact
    /// along with the value of each.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Retrieve a borrower based on a command-line argument
    ///       Contact contact = session.Contacts.Open(int.Parse(args[0]), ContactType.Borrower);
    /// 
    ///       // Loop over the defined custom fields
    ///       foreach (ContactCustomField field in contact.CustomFields)
    ///       {
    ///          // Display the name of the field and the value for this contact
    ///          Console.WriteLine(field.Name + " = " + field.Value);
    /// 
    ///          // If this is a drop-down field, display the possible options
    ///          if ((field.Type == CustomFieldType.DROPDOWN) ||
    ///             (field.Type == CustomFieldType.DROPDOWN_EDITABLE))
    ///          {
    ///             for (int i = 0; i < field.PossibleValues.Count; i++)
    ///                Console.WriteLine("   " + field.PossibleValues[i]);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public StringList PossibleValues => this.valueList;

    internal EllieMae.EMLite.ClientServer.Contacts.ContactCustomField Unwrap()
    {
      return new EllieMae.EMLite.ClientServer.Contacts.ContactCustomField(this.contact.ID, this.fieldInfo.LabelID, this.Session.UserID, this.fieldValue);
    }

    private class ReadOnlyStringList : StringList
    {
      private bool readOnly;

      public ReadOnlyStringList(string[] list)
        : base((IList) list)
      {
        this.readOnly = true;
      }

      protected override void OnValidate(object value)
      {
        if (this.readOnly)
          throw new InvalidOperationException("Object is read only");
        base.OnValidate(value);
      }
    }
  }
}
