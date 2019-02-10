namespace OWM.Application.Services.Email
{
    public interface ITemplateBuilder
    {
        string Build(string template);
    }
}