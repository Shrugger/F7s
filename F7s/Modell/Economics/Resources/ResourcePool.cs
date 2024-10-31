using F7s.Modell.Abstract;
using F7s.Modell.Handling;
using F7s.Utility;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Modell.Economics.Resources
{

    public class ResourcePool : AbstractGameValue {
        private Dictionary<ResourceType, float> pool = new Dictionary<ResourceType, float>();

        public ResourcePool () { }

        public override void ConfigureInfoblock (Infoblock infoblock) {
            foreach (KeyValuePair<ResourceType, float> item in pool) {
                infoblock.AddInformation(new Resource(item.Key, item.Value));
            }
        }

        public override string ToString () {
            if (this.pool.Count == 0) {
                return "Nothing";
            }
            if (this.pool.Count == 1) {
                return ItemToString(this.pool.First());
            }
            if (this.pool.Count == 2) {
                return ItemToString(this.pool.First()) + " & " + ItemToString(this.pool.Last());
            }
            return this.pool.Count + " Items";
        }

        private static string ItemToString (KeyValuePair<ResourceType, float> item) {
            return item.Key + " x" + item.Value;
        }

        public bool Matches (Resource resource) {
            if (!Contains(resource.Type)) {
                return false;
            }

            foreach (KeyValuePair<ResourceType, float> item in pool) {
                if (resource.IsMatchedBy(item.Key, item.Value)) {
                    return true;
                }
            }

            return false;
        }

        public bool Contains (ResourceType resourceType) {
            return this.pool.ContainsKey(resourceType);
        }

        public void Add (ResourceType type, float quantity) {
            if (quantity < 0) {
                throw new System.Exception();
            } else if (quantity == 0) {
                return;
            }

            if (Contains(type)) {
                this.pool[type] += quantity;
            } else {
                this.pool.Add(type, quantity);
            }
        }

        public float RemoveQuantity (ResourceType type, float desiredQuantity) {
            if (Contains(type)) {
                float drawn = MM.Clamp(desiredQuantity, 0, this.pool[type]);
                this.pool[type] -= drawn;
                return drawn;
            } else {
                return 0;
            }
        }

        public Resource RemoveResource (Resource resource) {
            float drawn = RemoveQuantity(resource.Type, resource.Quantity);
            if (drawn > 0) {
                return new Resource(resource.Type, drawn);
            } else {
                return null;
            }
        }

        public Resource RemoveResource (ResourceType type, float desiredQuantity) {
            float drawn = RemoveQuantity(type, desiredQuantity);
            if (drawn > 0) {
                return new Resource(type, drawn);
            } else {
                return null;
            }
        }

        public void Add (Resource resource) {
            this.Add(resource.Type, resource.Quantity);
        }

        public void Add (ResourcePool donor, bool depleteDonor) {
            foreach (KeyValuePair<ResourceType, float> item in donor.pool) {
                this.Add(item.Key, item.Value);
            }
            if (depleteDonor) {
                donor.pool.Clear();
            }
        }

        public void Remove (ResourcePool removals, bool addToRemovals) {
            foreach (KeyValuePair<ResourceType, float> item in removals.pool) {
                float drawn = this.RemoveQuantity(item.Key, item.Value);
                if (addToRemovals) {
                    removals.Add(item.Key, drawn);
                }
            }
        }

        public static ResourcePool operator + (ResourcePool pool, Resource resource) {
            pool.Add(resource);
            return pool;
        }

        public static ResourcePool operator + (ResourcePool a, ResourcePool b) {
            return Sum(a, b);
        }

        public static ResourcePool operator - (ResourcePool a, ResourcePool b) {
            return Difference(a, b);
        }

        public ResourcePool Copy () {
            ResourcePool copy = new ResourcePool();
            foreach (KeyValuePair<ResourceType, float> item in pool) {
                copy.Add(item);
            }
            return copy;
        }

        private void Add (KeyValuePair<ResourceType, float> item) {
            this.Add(item.Key, item.Value);
        }

        public static ResourcePool Sum (params ResourcePool[] pools) {
            ResourcePool sum = new ResourcePool();
            foreach (ResourcePool pool in pools) {
                sum.Add(pool, false);
            }
            return sum;
        }

        public static ResourcePool Difference (ResourcePool basis, ResourcePool subtraction) {
            ResourcePool difference = basis.Copy();
            difference.Remove(subtraction, false);
            return basis;
        }

        public static implicit operator int (ResourcePool pool) {
            return pool.pool.Count;
        }
    }
}
