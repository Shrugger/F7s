using F7s.Modell.Abstract;
using F7s.Modell.Conceptual.Agents.Skills;
using System.Collections.Generic;

namespace F7s.Modell.Conceptual.Agents.Activities {
    public class WorkType : AbstractGameValue {


        public static readonly WorkType Farmhand = new WorkType("Farmhand", (SkillType.Manual, 1.0f));
        public static readonly WorkType Trader = new WorkType("Trader", (SkillType.Mental, 1.0f));
        public static readonly WorkType Fighter = new WorkType("Fighter", (SkillType.Martial, 1.0f));

        public readonly string name;

        public enum WorkBaseTypes { Undefined, Manual, Mental, Martial }
        private readonly List<SkillValue> skills = new List<SkillValue>();

        public WorkType (string name, params SkillValue[] skills) {
            this.name = name;
            this.skills.AddRange(skills);
        }

        public override string ToString () {
            return name;
        }
    }
}
