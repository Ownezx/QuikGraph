﻿#if SUPPORTS_SERIALIZATION || SUPPORTS_CLONEABLE
using System;
#endif
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace QuickGraph.Collections
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IVertexEdgeDictionary<,>))]
#endif
    abstract class IVertexEdgeDictionaryContract<TVertex, TEdge> : IVertexEdgeDictionary<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        IVertexEdgeDictionary<TVertex, TEdge> IVertexEdgeDictionary<TVertex, TEdge>.Clone()
        {
#if SUPPORTS_CONTRACTS
            Contract.Ensures(Contract.Result<IVertexEdgeDictionary<TVertex, TEdge>>() != null);
#endif

            throw new NotImplementedException();
        }

        #region others
        void System.Collections.Generic.IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Add(TVertex key, IEdgeList<TVertex, TEdge> value)
        {
            throw new NotImplementedException();
        }

        bool System.Collections.Generic.IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.ContainsKey(TVertex key)
        {
            throw new NotImplementedException();
        }

        System.Collections.Generic.ICollection<TVertex> System.Collections.Generic.IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Keys
        {
            get { throw new NotImplementedException(); }
        }

        bool System.Collections.Generic.IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Remove(TVertex key)
        {
            throw new NotImplementedException();
        }

        bool System.Collections.Generic.IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.TryGetValue(TVertex key, out IEdgeList<TVertex, TEdge> value)
        {
            throw new NotImplementedException();
        }

        System.Collections.Generic.ICollection<IEdgeList<TVertex, TEdge>> System.Collections.Generic.IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Values
        {
            get { throw new NotImplementedException(); }
        }

        IEdgeList<TVertex, TEdge> System.Collections.Generic.IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.this[TVertex key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Add(System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> item)
        {
            throw new NotImplementedException();
        }

        void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Contains(System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> item)
        {
            throw new NotImplementedException();
        }

        void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.CopyTo(System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Remove(System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> item)
        {
            throw new NotImplementedException();
        }

        System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>> System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

#if SUPPORTS_CLONEABLE
        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
#endif

#if SUPPORTS_SERIALIZATION
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
#endif

        #endregion
    }
}