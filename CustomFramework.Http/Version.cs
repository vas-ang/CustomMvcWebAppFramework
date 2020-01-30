using System.Collections.Generic;

namespace CustomFramework.Http
{
    public class Version
    {
        private readonly string version;

        public Version(string version)
        {
            this.version = version;
        }

        public override bool Equals(object obj)
        {
            if (obj is Version v)
            {
                return this.version == v.version;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 1380981039 + EqualityComparer<string>.Default.GetHashCode(version);
        }

        public override string ToString()
        {
            return this.version;
        }
    }
}