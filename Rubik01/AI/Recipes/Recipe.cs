using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;

namespace Rubik01.AI
{
    internal abstract class Recipe
    {
        public string code;
        public int[] ingredients;

        // return true if was able to apply
        public abstract bool TryToApply(Cube cube, params int[] parameters);

        public virtual void Apply(Cube cube, params int[] parameters) { }
    }
}
