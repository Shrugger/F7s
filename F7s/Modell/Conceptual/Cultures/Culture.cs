using F7s.Modell.Abstract;
using F7s.Modell.Conceptual.Agents;
using System; using F7s.Utility.Geometry.Double;
using System.Collections.Generic;

namespace F7s.Modell.Conceptual.Cultures {

    public class Culture : GameEntity {

        public abstract class Attribute : GameEntity {

            public class Key : GameEntity {

                public static readonly Key Variance;
                public static readonly Key Uniformity;

                static Key () {
                    Variance = new Key("Variance");
                    Uniformity = new Key("Uniformity");
                    Relate(Uniformity, Variance, -1);
                }

                protected Key (string name) : base(name) {

                }

                private readonly Dictionary<Key, double> relations = new Dictionary<Key, double>();

                public virtual double Relation (Key other) {
                    if (relations.ContainsKey(other)) {
                        return relations[other];
                    } else {
                        return 0;
                    }
                }

                public void Relate (Key other, double relation) {
                    if (relations.ContainsKey(other)) {
                        throw new Exception(this + " already contains a relation to key " + other + ".");
                    } else {
                        relations.Add(other, relation);
                    }
                }

                public static void Relate (Key a, Key b, double relation) {
                    a.Relate(b, relation);
                    b.Relate(a, relation);
                }
            }

            public readonly Key key;
            public double Value { get; private set; }

            private Dictionary<Key, double> relations;

            public void ShiftValue (double value) {
                throw new NotImplementedException();
            }

            public virtual double Relation (Key other) {
                return key.Relation(other);
            }
        }

        private Dictionary<Attribute.Key, Attribute> attributes = new Dictionary<Attribute.Key, Attribute>();
        private List<Institution> GenerateInstitutions () {
            throw new NotImplementedException();
        }

        private Attribute GetAttribute (Attribute.Key key) {
            return attributes[key];
        }

        private void ShiftAttribute (Attribute.Key key, double value) {
            Attribute shiftedAttribute = GetAttribute(key);
            shiftedAttribute.ShiftValue(value);
            foreach (Attribute attribute in attributes.Values) {
                if (attribute == shiftedAttribute) {
                    continue;
                }
                double relationShift = shiftedAttribute.Relation(attribute.key);
                if (relationShift != 0) {
                    attribute.ShiftValue(relationShift);
                }
            }
        }
    }
}
