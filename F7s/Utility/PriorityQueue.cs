//**********************************************************
//* PriorityQueue                                          *
//* Copyright (c) Julian M Bucknall 2004                   *
//* All rights reserved.                                   *
//* This code can be used in your applications, providing  *
//*    that this copyright comment box remains as-is       *
//**********************************************************
//* .NET priority queue class (heap algorithm)             *
//**********************************************************

using Assets.Utility.DataStructure;
using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;

namespace F7s.Utility {

	[Serializable]
	public class PriorityQueue<ItemType, ComparableType> : ICollection, ISerializable
		where ComparableType : IComparable {

		public enum HeapType { MinHeap, MaxHeap }

		private const string capacityName = "capacity";
		private const string countName = "count";
		private const string heapTypeName = "heapType";
		private const string heapName = "heap";
		private readonly HeapType heapType;
		private int capacity;
		private HeapEntry<ItemType, ComparableType>[] heap;
		private int version;

		public PriorityQueue(HeapType heapType) {
			capacity = 15; // 15 is equal to 4 complete levels
			heap = new HeapEntry<ItemType, ComparableType>[capacity];
			this.heapType = heapType;
		}

		protected PriorityQueue(SerializationInfo info, StreamingContext context) {
			capacity = info.GetInt32(name: capacityName);
			Count = info.GetInt32(name: countName);
			heapType = (HeapType)info.GetValue(name: heapTypeName, typeof(HeapType));

			HeapEntry<ItemType, ComparableType>[] heapCopy =
				(HeapEntry<ItemType, ComparableType>[])info.GetValue(
																	  name:
																		 heapName,
																	  type: typeof(HeapEntry<ItemType, ComparableType>[]
																	  )
																	 );
			heap = new HeapEntry<ItemType, ComparableType>[capacity];

			Array.Copy(
					   sourceArray: heapCopy,
					   sourceIndex: 0,
					   destinationArray: heap,
					   destinationIndex: 0,
					   length: Count
					  );
			version = 0;
		}

		#region IEnumerable implementation

		public IEnumerator GetEnumerator() {
			return new PriorityQueueEnumerator(pq: this);
		}

		#endregion

		#region ISerializable implementation

		private static HeapType GetHeapTypeFromSerializationData(SerializationInfo info, StreamingContext context) {
			throw new NotImplementedException();
		}


		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue(name: capacityName, value: capacity);
			info.AddValue(name: countName, value: Count);
			info.AddValue(name: heapTypeName, value: heapType);
			HeapEntry<ItemType, ComparableType>[] heapCopy = new HeapEntry<ItemType, ComparableType>[Count];

			Array.Copy(
					   sourceArray: heap,
					   sourceIndex: 0,
					   destinationArray: heapCopy,
					   destinationIndex: 0,
					   length: Count
					  );

			info.AddValue(
						  name: heapName,
						  value: heapCopy,
						  type: typeof(HeapEntry<ItemType, ComparableType>[])
						 );
		}

		#endregion

		public ItemType PeekItem() {
			if (Count == 0) {
				throw new InvalidOperationException();
			}

			return heap[0].Item;
		}

		public ComparableType PeekPriority() {
			if (Count == 0) {
				throw new InvalidOperationException(message: "Queue is empty.");
			}

			return heap[0].Priority;
		}

		public ItemType Dequeue() {
			if (Count == 0) {
				throw new InvalidOperationException();
			}

			ItemType result = heap[0].Item;
			Count--;
			trickleDown(index: 0, he: heap[Count]);
			heap[Count].Clear();
			version++;
			return result;
		}

		public void Enqueue(ItemType item, ComparableType priority) {
			if (priority == null) {
				throw new ArgumentNullException(paramName: "priority");
			}

			if (Count == capacity) {
				growHeap();
			}

			Count++;

			bubbleUp(
						  index: Count - 1,
						  he: new HeapEntry<ItemType, ComparableType>(item: item, priority: priority)
						 );
			version++;
		}

		private void bubbleUp(int index, HeapEntry<ItemType, ComparableType> he) {
			int parent = getParent(index: index);

			// note: (index > 0) means there is a parent
			while (index > 0
				&& (heapType == HeapType.MaxHeap && heap[parent].Priority.CompareTo(obj: he.Priority) < 0
				 || heapType == HeapType.MinHeap && heap[parent].Priority.CompareTo(obj: he.Priority) > 0)) {
				heap[index] = heap[parent];
				index = parent;
				parent = getParent(index: index);
			}

			heap[index] = he;
		}

		private int getLeftChild(int index) {
			return index * 2 + 1;
		}

		private int getParent(int index) {
			return (index - 1) / 2;
		}

		private void growHeap() {
			capacity = capacity * 2 + 1;
			HeapEntry<ItemType, ComparableType>[] newHeap = new HeapEntry<ItemType, ComparableType>[capacity];

			Array.Copy(
					   sourceArray: heap,
					   sourceIndex: 0,
					   destinationArray: newHeap,
					   destinationIndex: 0,
					   length: Count
					  );
			heap = newHeap;
		}

		private void trickleDown(int index, HeapEntry<ItemType, ComparableType> he) {
			int child = getLeftChild(index: index);

			while (child < Count) {
				if (child + 1 < Count
				 && (heapType == HeapType.MaxHeap
				  && heap[child].Priority.CompareTo(obj: heap[child + 1].Priority) < 0
				  || heapType == HeapType.MinHeap
				  && heap[child].Priority.CompareTo(obj: heap[child + 1].Priority) > 0)) {
					child++;
				}

				heap[index] = heap[child];
				index = child;
				child = getLeftChild(index: index);
			}

			bubbleUp(index: index, he: he);
		}

		public string ReportContents(bool multiline = false) {
			string report = Count + " items in queue: ";

			foreach (HeapEntry<ItemType, ComparableType> he in heap.OrderBy(i => i.Priority)) {
				if (he.Item == null) {
					report += "null".PadLeft(totalWidth: 2) + " " + "null" + (multiline ? "\n" : ", ");
				} else {

					report +=
						Math.Round(float.Parse(he.Priority.ToString())).ToString().PadRight(totalWidth: 6) +
						" " +
						he.Item.ToString() +
						(multiline ? "\n" : ", ");
				}
			}

			return report;
		}

		#region Priority Queue enumerator

		[Serializable]
		private class PriorityQueueEnumerator : IEnumerator {

			private readonly PriorityQueue<ItemType, ComparableType> pq;
			private int index;
			private int version;

			public PriorityQueueEnumerator(PriorityQueue<ItemType, ComparableType> pq) {
				this.pq = pq;
				Reset();
			}

			private void checkVersion() {
				if (version != pq.version) {
					throw new InvalidOperationException();
				}
			}

			#region IEnumerator Members

			public void Reset() {
				index = -1;
				version = pq.version;
			}

			public object Current {
				get {
					checkVersion();

					return pq.heap[index].Item;
				}
			}

			public bool MoveNext() {
				checkVersion();

				if (index + 1 == pq.Count) {
					return false;
				}

				index++;

				return true;
			}

			#endregion

		}

		#endregion

		#region ICollection implementation

		public int Count { get; private set; }

		public void CopyTo(Array array, int index) {
			Array.Copy(
					   sourceArray: heap,
					   sourceIndex: 0,
					   destinationArray: array,
					   destinationIndex: index,
					   length: Count
					  );
		}

		public object SyncRoot => this;

		public bool IsSynchronized => false;

		#endregion

	}

}
