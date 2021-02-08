using System;

namespace Spice.Models
{
    internal class ForeignkeyAttribute : Attribute
    {
        private string v;

        public ForeignkeyAttribute(string v)
        {
            this.v = v;
        }
    }
}