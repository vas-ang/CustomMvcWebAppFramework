namespace CustomFramework.Http.Elements
{
    using System.Web;
    using System.Linq;
    using System.Collections.Generic;

    public class HttpFormData
    {
        private IDictionary<string, string> data = new Dictionary<string, string>();

        public string this[string key]
        {
            get
            {
                return this.data[key.ToLower()];
            }
            set
            {
                this.data[key.ToLower()] = value;
            }
        }

        public bool ContainsKey(string key)
        {
            return this.data.ContainsKey(key.ToLower());
        }

        public static HttpFormData Parse(string data)
        {
            return new HttpFormData()
            { // TODO: Remove System.Web class;
                data = HttpUtility.UrlDecode(data).Split('&')
                    .Select(x => x.Split('='))
                    .ToDictionary(d => d[0], d => d[1]),
            };
        }

        public override string ToString()
        {
            return string.Join("&", this.data.Select(d => $"{d.Key}={d.Value}"));
        }
    }
}
