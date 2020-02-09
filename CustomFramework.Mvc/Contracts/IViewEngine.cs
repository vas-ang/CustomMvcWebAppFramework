namespace CustomFramework.Mvc.Contracts
{
    public interface IViewEngine
    {
        string GetHtml(string templateHtml, object model, string user);
    }
}
