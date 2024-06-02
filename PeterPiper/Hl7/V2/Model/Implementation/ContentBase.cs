using PeterPiper.Hl7.V2.Support.Content.Convert;

namespace PeterPiper.Hl7.V2.Model.Implementation;

internal abstract class ContentBase : ModelBase, IContentBase
{
    protected ContentBase(IMessageDelimiters delimiters)
        : base(delimiters)
    {
    }

    protected ContentBase()
    {
    }

    public IConvert Convert => new PeterPiper.Hl7.V2.Support.Content.Convert.Implementation.Convert(this);
}