using System.Collections.Generic;

namespace F7s.Modell.Abstract {

    public interface Hierarchical<T> where T : Hierarchical<T> {
        public T HierarchyMember ();
        public T HierarchySuperior ();
        public List<T> HierarchySubordinates ();

        public Hierarchical<T> HierarchyRoot ();
        public T HierarchicalLowestCommonSuperior (T other);
    }

}
