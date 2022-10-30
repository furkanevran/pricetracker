namespace PriceTracker.API.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TemplateAttribute : Attribute
{
    public string Template { get; }

    public TemplateAttribute(string template)
    {
        Template = template ?? throw new ArgumentNullException(nameof(template));
    }
}