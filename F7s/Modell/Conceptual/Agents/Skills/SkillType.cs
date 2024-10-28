using F7s.Modell.Abstract;

namespace F7s.Modell.Conceptual.Agents.Skills
{
    public class SkillType : AbstractGameValue
    {

        public static readonly SkillType Manual = new SkillType("Manual");
        public static readonly SkillType Mental = new SkillType("Mental");
        public static readonly SkillType Martial = new SkillType("Martial");

        public readonly string name;

        public SkillType(string name)
        {
            this.name = name;
        }
    }
}
