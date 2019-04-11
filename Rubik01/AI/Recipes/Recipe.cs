using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rubik01.CubeComponents;

namespace Rubik01.AI.Recipes
{
    internal interface IRecipe
    {
        bool TryToApply(Cube cube, params int[] parameters);
        void Apply(Cube cube, params int[] parameters);
    }

    internal abstract class Recipe: IRecipe
    {
        public string _code;
        public int[] _ingredients;

        protected Recipe(string code, IEnumerable<int> ingredients)
        {
            _code = code;
            _ingredients = ingredients.ToArray();
        }

        // return true if was able to apply
        public abstract bool TryToApply(Cube cube, params int[] parameters);

        public virtual void Apply(Cube cube, params int[] parameters) { }

        protected abstract void CheckParameterCount(int paramCount);

        protected void CheckIngredientCount(int expectedCount)
        {
            if (_ingredients.Length < expectedCount)
                throw new ArgumentException($@"Error: {GetType().Name} should have {expectedCount / 3} ingredients", "ingredients");
        }

        protected static int GetOppositeFacet(int facetID)
        {
            if (facetID == 2)
                return 5;
            if (facetID == 5)
                return 2;

            return 4 - facetID;
        }
    }

    internal abstract class RecipeWithDirection: Recipe
    {
        protected readonly bool _direction;

        protected RecipeWithDirection(string code, bool direction, IEnumerable<int> ingredients) : base(code, ingredients)
        {
            _direction = direction;
        }
    }
}
