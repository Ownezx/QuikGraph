﻿#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuickGraph.Collections;

namespace QuickGraph
{
    /// <summary>
    /// Wraps a vertex list graph (out-edges only) and caches the in-edge dictionary.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public class BidirectionAdapterGraph<TVertex, TEdge>
        : IBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IVertexAndEdgeListGraph<TVertex, TEdge> baseGraph;
        private readonly Dictionary<TVertex, EdgeList<TVertex, TEdge>> inEdges;

        public BidirectionAdapterGraph(IVertexAndEdgeListGraph<TVertex, TEdge> baseGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(baseGraph != null);
#endif

            this.baseGraph = baseGraph;
            this.inEdges = new Dictionary<TVertex, EdgeList<TVertex, TEdge>>(this.baseGraph.VertexCount);
            foreach (var edge in this.baseGraph.Edges)
            {
                EdgeList<TVertex, TEdge> list;
                if (!this.inEdges.TryGetValue(edge.Target, out list))
                    this.inEdges.Add(edge.Target, list = new EdgeList<TVertex, TEdge>());
                list.Add(edge);
            }
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool IsInEdgesEmpty(TVertex v)
        {
            return this.InDegree(v) == 0;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public int InDegree(TVertex v)
        {
            EdgeList<TVertex, TEdge> edges;
            if (this.inEdges.TryGetValue(v, out edges))
                return edges.Count;
            else
                return 0;
        }

        static readonly TEdge[] emptyEdges = new TEdge[0];

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public IEnumerable<TEdge> InEdges(TVertex v)
        {
            EdgeList<TVertex, TEdge> edges;
            if (this.inEdges.TryGetValue(v, out edges))
                return edges;
            else
                return emptyEdges;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            EdgeList<TVertex, TEdge> es;
            if (this.inEdges.TryGetValue(v, out es))
            {
                edges = es;
                return true;
            }

            edges = null;
            return false;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public TEdge InEdge(TVertex v, int index)
        {
            return this.inEdges[v][index];
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public int Degree(TVertex v)
        {
            return this.InDegree(v) + this.OutDegree(v);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return this.baseGraph.ContainsEdge(source, target);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            return this.baseGraph.TryGetEdges(source, target, out edges);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            return this.baseGraph.TryGetEdge(source, target, out edge);
        }

#if SUPPORTS_CONTRACTS
        [Pure] // InterfacePureBug
#endif
        public bool IsOutEdgesEmpty(TVertex v)
        {
            return this.baseGraph.IsOutEdgesEmpty(v);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public int OutDegree(TVertex v)
        {
            return this.baseGraph.OutDegree(v);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            return this.baseGraph.OutEdges(v);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return this.baseGraph.TryGetOutEdges(v, out edges);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public TEdge OutEdge(TVertex v, int index)
        {
            return this.baseGraph.OutEdge(v, index);
        }

        public bool IsDirected
        {
            get { return this.baseGraph.IsDirected; }
        }

        public bool AllowParallelEdges
        {
            get { return this.baseGraph.AllowParallelEdges; }
        }

        public bool IsVerticesEmpty
        {
            get { return this.baseGraph.IsVerticesEmpty; }
        }

        public int VertexCount
        {
            get { return this.baseGraph.VertexCount; }
        }

        public IEnumerable<TVertex> Vertices
        {
            get { return this.baseGraph.Vertices; }
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool ContainsVertex(TVertex vertex)
        {
            return this.baseGraph.ContainsVertex(vertex);
        }

        public bool IsEdgesEmpty
        {
            get { return this.baseGraph.IsEdgesEmpty; }
        }

        public int EdgeCount
        {
            get { return this.baseGraph.EdgeCount; }
        }

        public virtual IEnumerable<TEdge> Edges
        {
            get { return this.baseGraph.Edges; }
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool ContainsEdge(TEdge edge)
        {
            return this.baseGraph.ContainsEdge(edge);
        }
    }
}