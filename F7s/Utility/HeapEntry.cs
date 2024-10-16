using System;

namespace Assets.Utility.DataStructure {

	[Serializable]
	public struct HeapEntry<ItemType, ComparableType>
		where ComparableType : IComparable {

		public HeapEntry(ItemType item, ComparableType priority) {
			this.Item = item;
			this.Priority = priority;
		}

		public ItemType Item { get; private set; }

		public ComparableType Priority { get; private set; }

		public void Clear() {
			this.Item = default;
			this.Priority = default;
		}

	}

}
