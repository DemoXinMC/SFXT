using SFXT;
using SFXT.Components.Graphics;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT_Test
{
    public class MapEntity : Entity
    {
        public MapEntity(Activity activity) : base(activity)
        {
        }

        public Vector2 GetRandomPoint()
        {
            return this.GetComponent<Tilemap>().GetRandomPoint();
        }
    }
}
