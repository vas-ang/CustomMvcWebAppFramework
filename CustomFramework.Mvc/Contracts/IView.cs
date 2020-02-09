namespace CustomFramework.Mvc.Contracts
{
    public interface IView
    {
        string GetHtml(object model, string user);
    }
}
