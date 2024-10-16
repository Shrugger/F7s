using F7s.Modell.Abstract;
using F7s.Modell.Economics.Resources;
using System.Collections.Generic;

namespace F7s.Modell.Economics.Industry
{
    public class Recipe : GameEntity
    {
        private readonly List<Resource> Input;
        private readonly List<Resource> Output;

        public Recipe(List<Resource> input, List<Resource> output)
        {
            Input = input;
            Output = output;
        }
    }
}
