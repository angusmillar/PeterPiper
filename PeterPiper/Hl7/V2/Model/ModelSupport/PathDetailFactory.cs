using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model.Implementation;
using PeterPiper.Hl7.V2.Support.Content;

namespace PeterPiper.Hl7.V2.Model.ModelSupport
{
  internal static class PathDetailFactory
  {
    public static PathDetailBase GetPathDetail(ModelBase item)
    {
      return LoadPathDetailData(item);      
    }
    private static PathDetailBase LoadPathDetailData(ModelBase item)
    {
      PathDetailBase _Info = new PathDetailBase();  
      if (item is Content)
      {
        GetContentInfo(item, _Info);
      }
      else if (item is SubComponent)
      {
        GetSubComponentInfo(item, _Info);
      }
      else if (item is Component)
      {
        GetComponentInfo(item, _Info);
      }
      else if (item is Field)
      {
        GetFieldInfo(item, _Info);
      }
      else if (item is Element)
      {
        GetElementInfo(item, _Info);
      }
      else if (item is Segment)
      {
        GetSegmentInfo(item, _Info);
      }
      else if (item is Message)
      {
        GetMessageInfo(item, _Info);
      }
      return _Info;
    }
    private static void GetContentInfo(ModelBase item, PathDetailBase _Info)
    {
      var Content = item as Content;
      _Info._ContentPosition = Content._Index;        
      if (item._Parent != null)
        GetSubComponentInfo(item._Parent,_Info);     
    }
    private static void GetSubComponentInfo(ModelBase item, PathDetailBase _Info)
    {
      var SubComponent = item as SubComponent;
      _Info._SubComponentPosition = SubComponent._Index;
      if (item._Parent != null)
        GetComponentInfo(item._Parent, _Info);
    }
    private static void GetComponentInfo(ModelBase item, PathDetailBase _Info)
    {
      var Component = item as Component;
      _Info._ComponentPosition = Component._Index;
      _Info._SubComponentCount = Component.SubComponentCount;
      if (item._Parent != null)
        GetFieldInfo(item._Parent, _Info);
    }
    private static void GetFieldInfo(ModelBase item, PathDetailBase _Info)
    {
      var Field = item as Field;
      _Info._RepeatPosition = Field._Index;
      _Info._ComponentCount = Field.ComponentCount;
      if (item._Parent != null)
        GetElementInfo(item._Parent, _Info);
    }
    private static void GetElementInfo(ModelBase item, PathDetailBase _Info)
    {
      var Element = item as Element;
      _Info._FieldPosition = Element._Index;
      _Info._RepeatCount = Element.RepeatCount;
      if (item._Parent != null)
        GetSegmentInfo(item._Parent, _Info);
    }
    private static void GetSegmentInfo(ModelBase item, PathDetailBase _Info)
    {
      var Segment = item as Segment;
      int SegmentCount = 0;
      if (Segment._Parent != null)
      {
        var Msg = Segment._Parent as Message;
        SegmentCount = Msg.SegmentCount(Segment.Code); 
      }
      _Info._SegmentPosition = Segment._Index;
      _Info._SegmentTypePosition = SegmentCount;
      _Info._SegmentCode = Segment.Code;      
      if (item._Parent != null)
        GetMessageInfo(item._Parent, _Info);
    }
    private static void GetMessageInfo(ModelBase item, PathDetailBase _Info)
    {
      var Message = item as Message;
      _Info._MessageType = Message.MessageType;
      _Info._MessageEvent = Message.MessageTrigger;
      _Info._MessageVersion = Message.MessageVersion;
     
    }
  }  
}
 
