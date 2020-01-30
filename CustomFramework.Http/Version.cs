namespace CustomFramework.Http
{
    public class Version
    {
        private readonly string version;

        public Version(string version)
        {
            this.version = version;
        }
        
        public override string ToString()
        {
            return this.version;
        }
    }
}