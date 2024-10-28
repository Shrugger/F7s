using F7s.Modell.Abstract;
using F7s.Modell.Handling;
using System;

namespace F7s.Modell.Conceptual.Agents.Skills
{
    public struct SkillValue : IGameValue
    {

        public readonly SkillType SkillType;
        public readonly float Value;

        public SkillValue(SkillType skillType, float value)
        {
            SkillType = skillType;
            Value = value;
        }

        public static implicit operator SkillValue((SkillType, float) typeAndValue)
        {
            return new SkillValue(typeAndValue.Item1, typeAndValue.Item2);
        }

        public void ConfigureContextMenu(ContextMenu contextMenu)
        {
            throw new NotImplementedException();
        }

        public void ConfigureInfoblock(Infoblock infoblock)
        {
            throw new NotImplementedException();
        }
    }
}
