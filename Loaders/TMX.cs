using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Loaders
{
    public static class TMX
    {
        public static TMXEntity Create(Activity activity, string filename)
        {
            return new TMXEntity(activity);
        }
    }

    public class TMXEntity : Entity
    {
        public TMXEntity(Activity activity) : base(activity)
        {
        }
    }
}
