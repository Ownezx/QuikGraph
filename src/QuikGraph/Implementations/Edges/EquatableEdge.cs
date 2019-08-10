using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// An <see cref="IEdge{TVertex}"/> implementation that supports equality.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}->{" + nameof(Target) + "}")]
    public class EquatableEdge<TVertex> : Edge<TVertex>, IEquatable<EquatableEdge<TVertex>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquatableEdge{TVertex}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        public EquatableEdge([NotNull] TVertex source, [NotNull] TVertex target)
            : base(source, target)
        {
        }

        /// <inheritdoc />
        public virtual bool Equals(EquatableEdge<TVertex> other)
        {
            if (other is null)
                return false;
            return Source.Equals(other.Source) && Target.Equals(other.Target);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as EquatableEdge<TVertex>);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCodeHelpers.Combine(Source.GetHashCode(), Target.GetHashCode());
        }
    }
}