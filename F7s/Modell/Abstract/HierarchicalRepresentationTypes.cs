namespace F7s.Modell.Abstract {
    public enum HierarchicalRepresentationTypes {
        /// <summary>Throw errors.</summary>
        Undefined,
        /// <summary>Don't represent at all.</summary>
        None,
        /// <summary>Represent body but not in HUD / UI.</summary>
        Subtle,
        /// <summary>Represent all aspects, plus subtle representations for qualified subordinates.</summary>
        Full,
        /// <summary>Represent in full, plus full representations for qualified subordinates.</summary>
        Entourage,
        /// <summary>Represent subtly, plus full representations for all subordinates.</summary>
        Exploded
    }

}
