﻿using System.Text;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Condensation;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Edge merge condensation graph to DOT renderer.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class EdgeMergeCondensatedGraphRenderer<TVertex, TEdge> : GraphRendererBase<TVertex, MergedEdge<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeMergeCondensatedGraphRenderer{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="graph">Graph to convert to DOT.</param>
        public EdgeMergeCondensatedGraphRenderer(
            [NotNull] IEdgeListGraph<TVertex, MergedEdge<TVertex, TEdge>> graph)
            : base(graph)
        {
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            Graphviz.FormatVertex += OnFormatVertex;
            Graphviz.FormatEdge += OnFormatEdge;
        }

        /// <inheritdoc />
        protected override void Clean()
        {
            Graphviz.FormatEdge -= OnFormatEdge;
            Graphviz.FormatVertex -= OnFormatVertex;

            base.Clean();
        }

        private void OnFormatVertex([NotNull] object sender, [NotNull] FormatVertexEventArgs<TVertex> args)
        {
            args.VertexFormat.Label = Graphviz.Escape(args.Vertex.ToString());
        }

        private void OnFormatEdge([NotNull] object sender, [NotNull] FormatEdgeEventArgs<TVertex, MergedEdge<TVertex, TEdge>> args)
        {
            var builder = new StringBuilder();
            builder.AppendLine(args.Edge.Edges.Count.ToString());
            
            foreach (TEdge edge in args.Edge.Edges)
            {
                builder.AppendLine($"  {edge}");
            }

            args.EdgeFormat.Label.Value = Graphviz.Escape(builder.ToString());
        }
    }
}