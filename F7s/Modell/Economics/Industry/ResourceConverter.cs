using F7s.Modell.Abstract;
using System.Collections.Generic;

namespace F7s.Modell.Economics.Industry
{
    public class ResourceConverter : GameEntity
    {
        private readonly Recipe recipe;

        public ResourceConverter(Recipe recipe)
        {
            this.recipe = recipe;
        }
    }
}
