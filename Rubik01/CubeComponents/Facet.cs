using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace Rubik01.CubeComponents
{
    internal class Facet
    {
        public Vector3D normal;
        public int color;

        public Facet()
        {
            normal = new Vector3D(0, 0, 0);
            color = -1;
        }

        public Facet(Vector3D center, int color = -1)
        {
            normal = center;
            this.color = color;
        }
    }
}
