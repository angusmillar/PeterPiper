using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Model.Implementation;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation
{
  internal class Content : ContentBase, IContent
  {    
    private ModelSupport.ContentTypeInternal _InternalContentType;
    public Support.Content.ContentType ContentType
    {
      get
      {
        if (_InternalContentType == ModelSupport.ContentTypeInternal.Text || _InternalContentType == ModelSupport.ContentTypeInternal.EncodingCharacters || _InternalContentType == ModelSupport.ContentTypeInternal.MainSeparator)
          return Support.Content.ContentType.Text;
        else if (_InternalContentType == ModelSupport.ContentTypeInternal.Escape)
          return Support.Content.ContentType.Escape;
        else
        {
          throw new PeterPiperException(String.Format("Unknown ModelSupport.ContentTypeInternal of :{0}", _InternalContentType.ToString()));
        }
      }
      set
      {
        if (value == Support.Content.ContentType.Text)
          _InternalContentType = ModelSupport.ContentTypeInternal.Text;
        else if (value == Support.Content.ContentType.Escape)
          _InternalContentType = ModelSupport.ContentTypeInternal.Escape;
        else
        {
          throw new PeterPiperException(String.Format("Unknown Support.Content.ContentType of :{0}", value.ToString()));
        }
      }
    }
    
    private EscapeData _EscapeMetaData = null;
    public IEscapeData EscapeMetaData
    {
      get
      {
        return _EscapeMetaData;
      }
    }

    private string _Data = string.Empty;

    //Creator Factory used Constructors
    /// <summary>
    /// Set any type of Content, maybe used for non HL7 Standard escape i.e \Qxx5\
    /// </summary>
    /// <param name="String"></param>
    /// <param name="ContentType"></param>
    internal Content(string String, Support.Content.ContentType ContentType)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      this.ContentType = ContentType;
      if (ValidateData(String))
      {
        if (String == String.Empty)
        {
          if (ContentType == Support.Content.ContentType.Escape)
          {
            throw new PeterPiperException("Attempt to set Content as type 'Escape' yet Content String was empty. Escape type content must have data provided");
          }
        }
        else
        {
          _Data = String;
          if (ContentType == Support.Content.ContentType.Escape)
          {
            _EscapeMetaData = new EscapeData(String);
          }
          else
          {
            _EscapeMetaData = null;
          }
          SetParent();
        }
      }
    }
    internal Content(string String, Support.Content.ContentType ContentType, IMessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      this.ContentType = ContentType;
      if (ValidateData(String))
      {
        if (String != String.Empty)
        {
          _Data = String;
          if (ContentType == Support.Content.ContentType.Escape)
          {
            _EscapeMetaData = new EscapeData(String);            
          }
          else
          {            
            _EscapeMetaData = null;
          }
          SetParent();
        }
        else
        {
          if (ContentType == Support.Content.ContentType.Escape)
          {
            throw new PeterPiperException("Attempt to set Content as type 'Escape' yet Content String was empty. Escape type content must have data provided");
          }
        }
      }
    }
    /// <summary>
    /// Set a Content to normal text, defaults to ContentType = TEXT
    /// </summary>
    /// <param name="String"></param>
    internal Content(string String)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      this.ContentType = Support.Content.ContentType.Text;      
      _EscapeMetaData = null;
      if (ValidateData(String))
      {
        if (String != String.Empty)
        {
          _Data = String;
          SetParent();
        }
      }
    }
    internal Content(string String, IMessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      this.ContentType = Support.Content.ContentType.Text;      
      _EscapeMetaData = null;
      if (ValidateData(String))
      {
        if (String != String.Empty)
        {
          _Data = String;
          SetParent();
        }
      }
    }
    /// <summary>
    /// Set Content as HL7 Standard escape
    /// </summary>
    /// <param name="EscapeType"></param>
    internal Content(Support.Standard.EscapeType EscapeType)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      this.ContentType = Support.Content.ContentType.Escape;
      _EscapeMetaData = new EscapeData(EscapeType, String.Empty);
      _Data = _EscapeMetaData.EscapeTypeCharater.ToString();      
      SetParent();
    }
    internal Content(IEscapeData EscapeMetaData)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      this.ContentType = Support.Content.ContentType.Escape;      
      _EscapeMetaData = EscapeMetaData as EscapeData;
      _Data = String.Format("{0}{1}", _EscapeMetaData.EscapeTypeCharater, _EscapeMetaData.MetaData);
      SetParent();
    }
    internal Content(Support.Standard.EscapeType EscapeType, IMessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      this.ContentType = Support.Content.ContentType.Escape;
      _EscapeMetaData = new EscapeData(EscapeType, String.Empty);
      _Data = _EscapeMetaData.EscapeTypeCharater.ToString();      
      SetParent();
    }

    //Only internal Constructors
    internal Content(string String, ModelSupport.ContentTypeInternal ContentTypeInternal,
                     MessageDelimiters CustomDelimiters, bool Temporary, int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      _InternalContentType = ContentTypeInternal;
      if (ContentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters)
      {
        SetDataToEncodingCharacters();
        _EscapeMetaData = null;        
      }
      else if (ContentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator)
      {
        SetDataToMainSeparator();
        _EscapeMetaData = null;        
      }
      else if (ContentTypeInternal == ModelSupport.ContentTypeInternal.Escape)
      {
        if (String != String.Empty)
        {
          if (ValidateData(String))
          {
            _Data = String;
            _EscapeMetaData = new EscapeData(String);            
          }
          SetParent();
        }
        else
        {
          throw new PeterPiperException("Attempt to set Content as Escape Type yet no Content data given, String was empty.");
        }
      }
      else
      {        
        if (String != String.Empty)
        {
          if (ValidateData(String))
          {            
            _EscapeMetaData = null;
            _Data = String;
          }
          SetParent();
        }
      }
    }
    internal Content(string String, MessageDelimiters CustomDelimiters, bool Temporary, int? Index,
                     ModelBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      _InternalContentType = ModelSupport.ContentTypeInternal.Text;      
      _EscapeMetaData = null;
      if (ValidateData(String))
      {
        if (String != String.Empty)
        {
          _Data = String;
          SetParent();
        }
      }
    }
    internal Content(Support.Standard.EscapeType EscapeType, MessageDelimiters CustomDelimiters,
                     bool Temporary, int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      _InternalContentType = ModelSupport.ContentTypeInternal.Escape;
      _EscapeMetaData = new EscapeData(EscapeType, String.Empty);      
      _Data = Support.Standard.Escapes.ResolveEscapeChararter(EscapeType).ToString();
      SetParent();
    }

    //Instance access
    public IMessageDelimiters MessageDelimiters
    {
      get
      {
        return this.Delimiters;
      }
    } 
    public IContent Clone()
    {
      return new Content(this.AsStringRaw, _InternalContentType, this.Delimiters, true, null, null);
    }
    public override string ToString()
    {
      return this.AsString;
    }
    public override string AsStringRaw
    {
      get
      {
        return this._Data;
      }
      set
      {
        if (value == String.Empty)
        {        
          this._Data = String.Empty;
          this._EscapeMetaData = null;
          this._InternalContentType = ModelSupport.ContentTypeInternal.Text;
          this._Temporary = true;               
          RemoveFromParent();
        }
        else if (ValidateData(value))
        {
          this._Data = value;
          SetParent();
        }
      }
    }
    public override string AsString
    {
      get
      {
        return Support.Standard.Escapes.Decode(this);        
      }
      set
      {
        this.AsStringRaw = Support.Standard.Escapes.Encode(value, this.Delimiters);          
      }
    }
    public bool IsEmpty
    {
      get
      {
        return String.IsNullOrWhiteSpace(this._Data);
        //return (this._Data == String.Empty);
      }
    }
    public bool IsHL7Null
    {
      get
      {
        if (this.AsStringRaw == Support.Standard.Null.HL7Null)
          return true;
        else
          return false;
      }
    }
    public void ClearAll()
    {
      _Data = String.Empty;
      RemoveFromParent();
    }    
        
    //Maintenance
    private void SetParent()
    {
      if (this._Temporary)
      {
        if (this._Parent is SubComponent)
        {
          SubComponent oSubComponent = this._Parent as SubComponent;
          if (oSubComponent.SetToDictonary(this))
          {
            this._Temporary = false;
          }
        }
      }
    }
    private void RemoveFromParent()
    {
      if (this._Index != null)
      {
        try
        {
          SubComponent oSubComponent = this._Parent as SubComponent;
          oSubComponent.RemoveChild(System.Convert.ToInt32(this._Index));
        }
        catch (InvalidCastException oInvalidCastExec)
        {
          throw new PeterPiperException("Casting of Content's parent to SubComponent throws Invalid Cast Exception, check inner exception for more detail", oInvalidCastExec);
        }
      }
    }

    //Parsing and Validation
    private bool ValidateData(string String)
    {
      Char[] CharatersNotAlowed = { this.Delimiters.Field,
                                    this.Delimiters.Component,
                                    this.Delimiters.SubComponent,
                                    this.Delimiters.Repeat,
                                    this.Delimiters.Escape};

      if (String.IndexOfAny(CharatersNotAlowed) != -1)
      {
        throw new PeterPiperArgumentException("Content data cannot contain HL7 V2 Delimiters, maybe you should be using 'AsStringRaw' if you are trying to insert already escaped data.");
      }
      return true;
    }
    private void SetDataToMainSeparator()
    {
      _Data = String.Format("{0}", this.Delimiters.Field);
    }
    private void SetDataToEncodingCharacters()
    {
      _Data = String.Format("{0}{1}{2}{3}", this.Delimiters.Component, this.Delimiters.Repeat, this.Delimiters.Escape, this.Delimiters.SubComponent);
    }
  }
}
