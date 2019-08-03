using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace QuikGraph.Collections
{
    /// <summary>
    /// Disjoint-set implementation with path compression and union-by-rank optimizations.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public class ForestDisjointSet<T> : IDisjointSet<T>
    {
#if DEBUG
        [DebuggerDisplay("{" + nameof(_id) + "}:{" + nameof(Rank) + "}->{" + nameof(Parent) + "}")]
#endif
        private class Element
        {
#if DEBUG
            private readonly int _id;
            // ReSharper disable once StaticMemberInGenericType
            private static int _nextId;
#endif

            [CanBeNull]
            public Element Parent { get; set; }

            public int Rank { get; set; }

            public T Value { get; }

            public Element(T value)
            {
#if DEBUG
                _id = _nextId++;
#endif
                Parent = null;
                Rank = 0;
                Value = value;
            }
        }

        [NotNull]
        private readonly Dictionary<T, Element> _elements;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForestDisjointSet{T}"/> class.
        /// </summary>
        public ForestDisjointSet()
        {
            _elements = new Dictionary<T, Element>();
            SetCount = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForestDisjointSet{T}"/> class.
        /// </summary>
        /// <param name="capacity">Element capacity.</param>
        public ForestDisjointSet(int capacity)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(capacity >= 0 && capacity < int.MaxValue);
#endif

            _elements = new Dictionary<T, Element>(capacity);
            SetCount = 0;
        }

#if SUPPORTS_CONTRACTS
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(SetCount >= 0);
            Contract.Invariant(SetCount <= _elements.Count);
        }
#endif

        #region IDisjointSet<T>

        /// <inheritdoc />
        public int SetCount { get; private set; }

        /// <inheritdoc />
        public int ElementCount => _elements.Count;

        /// <inheritdoc />
        public void MakeSet(T value)
        {
            var element = new Element(value);
            _elements.Add(value, element);
            ++SetCount;
        }

        /// <inheritdoc />
        public T FindSet(T value)
        {
            return Find(_elements[value]).Value;
        }

        /// <inheritdoc />
        public bool AreInSameSet(T left, T right)
        {
            return FindSet(left)?.Equals(FindSet(right)) ?? false;
        }

        /// <inheritdoc />
        public bool Union(T left, T right)
        {
            return Union(_elements[left], _elements[right]);
        }

        /// <inheritdoc />
        public bool Contains(T value)
        {
            return _elements.ContainsKey(value);
        }

        #endregion

        [JetBrains.Annotations.Pure]
        [NotNull]
        private static Element FindNoCompression([NotNull] Element element)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(element != null);
            Contract.Ensures(Contract.Result<Element>() != null);
#endif

            // Find root
            Element current = element;
            while (current.Parent != null)
                current = current.Parent;

            return current;
        }

        /// <summary>
        /// Finds the root parent element, and applies path compression.
        /// </summary>
        /// <param name="element">Element to search parent.</param>
        /// <returns>Root parent element.</returns>
        [NotNull]
        private static Element Find([NotNull] Element element)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(element != null);
            Contract.Ensures(Contract.Result<Element>() != null);
#endif

            Element root = FindNoCompression(element);
            CompressPath(element, root);
            return root;
        }

        private static void CompressPath([NotNull] Element element, [NotNull] Element root)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(element != null);
            Contract.Requires(root != null);
#endif

            // Path compression
            Element current = element;
            while (current != root && current != null)
            {
                Element temp = current;
                current = current.Parent;
                temp.Parent = root;
            }
        }

        private bool Union([NotNull] Element left, [NotNull] Element right)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(left != null);
            Contract.Requires(right != null);
            Contract.Ensures(
                Contract.Result<bool>()
                ? Contract.OldValue(SetCount) - 1 == SetCount
                : Contract.OldValue(SetCount) == SetCount);
            Contract.Ensures(FindNoCompression(left) == FindNoCompression(right));
#endif

            // Shortcut when already unioned
            if (left == right)
                return false;

            Element leftRoot = Find(left);
            Element rightRoot = Find(right);

            // Union by rank
            if (leftRoot.Rank > rightRoot.Rank)
            {
                rightRoot.Parent = leftRoot;
            }
            else if (leftRoot.Rank < rightRoot.Rank)
            {
                leftRoot.Parent = rightRoot;
            }
            else if (leftRoot != rightRoot)
            {
                rightRoot.Parent = leftRoot;
                leftRoot.Rank = leftRoot.Rank + 1;
            }
            else
            {
                return false; // Do not update the SetCount
            }

            SetCount--;
            return true;
        }
    }
}
