using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests related to "equals" of graphs.
    /// </summary>
    [TestFixture]
    internal class EquatableGraphTests
    {
        #region Test helpers

        private class VertexTestComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }
        }

        private class EdgeTestComparer : IEqualityComparer<Edge<int>>
        {
            public bool Equals(Edge<int> x, Edge<int> y)
            {
                if (x is null)
                    return y is null;
                if (y is null)
                    return false;
                return x.Source == y.Source && x.Target == y.Target;
            }

            public int GetHashCode(Edge<int> obj)
            {
                return obj.GetHashCode();
            }
        }

        #endregion

        #region Test cases

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> EquateWithComparerTestCases
        {
            [UsedImplicitly]
            get
            {
                var vertexComparer = new VertexTestComparer();
                var edgeComparer = new EdgeTestComparer();

                #region Same graph type

                yield return new TestCaseData(null, null, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                var emptyAdjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                var emptyAdjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                yield return new TestCaseData(emptyAdjacencyGraph1, null, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(null, emptyAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(emptyAdjacencyGraph1, emptyAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(emptyAdjacencyGraph1, emptyAdjacencyGraph2, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(emptyAdjacencyGraph2, emptyAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                var adjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph1.AddVertex(1);

                var adjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph2.AddVertex(1);

                yield return new TestCaseData(emptyAdjacencyGraph1, adjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph1, emptyAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph1, adjacencyGraph2, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                var edge12 = new Edge<int>(1, 2);
                var adjacencyGraph3 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph3.AddVerticesAndEdge(edge12);

                var adjacencyGraph4 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph4.AddVerticesAndEdge(edge12);

                var adjacencyGraph5 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph5.AddVertexRange(new[] { 1, 2 });

                yield return new TestCaseData(adjacencyGraph1, adjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph4, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph4, adjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph5, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph5, adjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                var edge12Bis = new Edge<int>(1, 2);
                var adjacencyGraph6 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph6.AddVerticesAndEdge(edge12Bis);

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph6, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph6, adjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                var edge13 = new Edge<int>(1, 3);
                var adjacencyGraph7 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph7.AddVerticesAndEdge(edge13);

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph7, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph7, adjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                #endregion

                #region Graph not constructed the same way

                var edge34 = new Edge<int>(3, 4);
                var edge42 = new Edge<int>(4, 2);
                var adjacencyGraph8 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph8.AddVertex(1);
                adjacencyGraph8.AddVertex(6);
                adjacencyGraph8.AddVerticesAndEdge(edge12);
                adjacencyGraph8.AddVertexRange(new[] { 3, 5, 4 });
                adjacencyGraph8.AddEdgeRange(new[] { edge42, edge34 });

                var adjacencyGraph9 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph9.AddVertexRange(new[] { 1, 2, 3, 4, 5, 6 });
                adjacencyGraph9.AddEdgeRange(new[] { edge12, edge34, edge42 });

                yield return new TestCaseData(adjacencyGraph8, adjacencyGraph9, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph9, adjacencyGraph8, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                #endregion

                #region Mixed graph types

                // Array adjacency
                var wrappedAdjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                wrappedAdjacencyGraph1.AddVertexRange(new[] { 0, 1, 2, 3, 4 });
                wrappedAdjacencyGraph1.AddEdgeRange(new[] { edge12, edge34 });
                var arrayAdjacencyGraph1 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedAdjacencyGraph1);
                var arrayAdjacencyGraph2 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedAdjacencyGraph1);

                yield return new TestCaseData(arrayAdjacencyGraph1, arrayAdjacencyGraph2, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(arrayAdjacencyGraph2, arrayAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                var wrappedAdjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                wrappedAdjacencyGraph2.AddVertexRange(new[] { 0, 3, 1, 2, 4 });
                wrappedAdjacencyGraph2.AddEdgeRange(new[] { edge34, edge12 });
                var arrayAdjacencyGraph3 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedAdjacencyGraph2);

                yield return new TestCaseData(arrayAdjacencyGraph1, arrayAdjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(arrayAdjacencyGraph3, arrayAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                // => Content check (not type check)
                yield return new TestCaseData(arrayAdjacencyGraph1, wrappedAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(wrappedAdjacencyGraph1, arrayAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                var wrappedAdjacencyGraph3 = new AdjacencyGraph<int, Edge<int>>();
                wrappedAdjacencyGraph3.AddVertexRange(new[] { 1, 2 });
                wrappedAdjacencyGraph3.AddEdge(edge12);
                var arrayAdjacencyGraph4 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedAdjacencyGraph3);
                
                yield return new TestCaseData(arrayAdjacencyGraph4, arrayAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(arrayAdjacencyGraph1, arrayAdjacencyGraph4, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                #endregion

                // TODO
            }
        }

        #endregion

        [TestCaseSource(nameof(EquateWithComparerTestCases))]
        public bool EquateWithComparer(
            [NotNull] IEdgeListGraph<int, Edge<int>> g,
            [NotNull] IEdgeListGraph<int, Edge<int>> h,
            [NotNull] IEqualityComparer<int> vertexEquality,
            [NotNull] IEqualityComparer<Edge<int>> edgeEquality)
        {
            return EquateGraphs.Equate(g, h, vertexEquality, edgeEquality);
        }

        [Test]
        public void EquateWithComparer_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => EquateGraphs.Equate<int, Edge<int>>(null, null, EqualityComparer<int>.Default, null));
            Assert.Throws<ArgumentNullException>(
                () => EquateGraphs.Equate<int, Edge<int>>(null, null, null, EqualityComparer<Edge<int>>.Default));
            Assert.Throws<ArgumentNullException>(
                () => EquateGraphs.Equate<int, Edge<int>>(null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #region Test cases

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> EquateTestCases
        {
            [UsedImplicitly]
            get
            {
                #region Same graph type

                yield return new TestCaseData(null, null)
                {
                    ExpectedResult = true
                };

                var emptyAdjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                var emptyAdjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                yield return new TestCaseData(emptyAdjacencyGraph1, null)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(null, emptyAdjacencyGraph1)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(emptyAdjacencyGraph1, emptyAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(emptyAdjacencyGraph1, emptyAdjacencyGraph2)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(emptyAdjacencyGraph2, emptyAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                var adjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph1.AddVertex(1);

                var adjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph2.AddVertex(1);

                yield return new TestCaseData(emptyAdjacencyGraph1, adjacencyGraph1)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph1, emptyAdjacencyGraph1)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph1, adjacencyGraph2)
                {
                    ExpectedResult = true
                };

                var edge12 = new Edge<int>(1, 2);
                var adjacencyGraph3 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph3.AddVerticesAndEdge(edge12);

                var adjacencyGraph4 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph4.AddVerticesAndEdge(edge12);

                var adjacencyGraph5 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph5.AddVertexRange(new[] { 1, 2 });

                yield return new TestCaseData(adjacencyGraph1, adjacencyGraph3)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph1)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph4)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph4, adjacencyGraph3)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph5)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph5, adjacencyGraph3)
                {
                    ExpectedResult = false
                };

                var edge12Bis = new Edge<int>(1, 2);
                var adjacencyGraph6 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph6.AddVerticesAndEdge(edge12Bis);

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph6)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph6, adjacencyGraph3)
                {
                    ExpectedResult = false
                };

                #endregion

                #region Graph not constructed the same way

                var edge34 = new Edge<int>(3, 4);
                var edge42 = new Edge<int>(4, 2);
                var adjacencyGraph7 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph7.AddVertex(1);
                adjacencyGraph7.AddVertex(6);
                adjacencyGraph7.AddVerticesAndEdge(edge12);
                adjacencyGraph7.AddVertexRange(new[] { 3, 5, 4 });
                adjacencyGraph7.AddEdgeRange(new[] { edge42, edge34 });

                var adjacencyGraph8 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph8.AddVertexRange(new[] { 1, 2, 3, 4, 5, 6 });
                adjacencyGraph8.AddEdgeRange(new[] { edge12, edge34, edge42 });

                yield return new TestCaseData(adjacencyGraph7, adjacencyGraph8)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph8, adjacencyGraph7)
                {
                    ExpectedResult = true
                };

                #endregion

                #region Mixed graph types

                // Array adjacency
                var wrappedAdjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                wrappedAdjacencyGraph1.AddVertexRange(new[] { 0, 1, 2, 3, 4 });
                wrappedAdjacencyGraph1.AddEdgeRange(new[] { edge12, edge34 });
                var arrayAdjacencyGraph1 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedAdjacencyGraph1);
                var arrayAdjacencyGraph2 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedAdjacencyGraph1);

                yield return new TestCaseData(arrayAdjacencyGraph1, arrayAdjacencyGraph2)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(arrayAdjacencyGraph2, arrayAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                var wrappedAdjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                wrappedAdjacencyGraph2.AddVertexRange(new[] { 0, 3, 1, 2, 4 });
                wrappedAdjacencyGraph2.AddEdgeRange(new[] { edge34, edge12 });
                var arrayAdjacencyGraph3 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedAdjacencyGraph2);

                yield return new TestCaseData(arrayAdjacencyGraph1, arrayAdjacencyGraph3)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(arrayAdjacencyGraph3, arrayAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                // => Content check (not type check)
                yield return new TestCaseData(arrayAdjacencyGraph1, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(wrappedAdjacencyGraph1, arrayAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                var wrappedAdjacencyGraph3 = new AdjacencyGraph<int, Edge<int>>();
                wrappedAdjacencyGraph3.AddVertexRange(new[] { 1, 2 });
                wrappedAdjacencyGraph3.AddEdge(edge12);
                var arrayAdjacencyGraph4 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedAdjacencyGraph3);
                
                yield return new TestCaseData(arrayAdjacencyGraph4, arrayAdjacencyGraph1)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(arrayAdjacencyGraph1, arrayAdjacencyGraph4)
                {
                    ExpectedResult = false
                };

                #endregion

                #region Other kind of graphs

                // Bidirectional
                var bidirectionalGraph1 = new BidirectionalGraph<int, Edge<int>>();
                bidirectionalGraph1.AddVertexRange(new[] { 0, 1, 2, 3, 4 });
                bidirectionalGraph1.AddEdgeRange(new[] { edge12, edge34 });

                yield return new TestCaseData(wrappedAdjacencyGraph1, bidirectionalGraph1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(bidirectionalGraph1, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                var bidirectionalGraph2 = new BidirectionalGraph<int, Edge<int>>();
                bidirectionalGraph2.AddVertexRange(new[] { 1, 2 });
                bidirectionalGraph2.AddEdge(edge12);
                
                yield return new TestCaseData(wrappedAdjacencyGraph1, bidirectionalGraph2)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(bidirectionalGraph2, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = false
                };
                
                // Array bidirectional
                var arrayBidirectionalGraph1 = new ArrayBidirectionalGraph<int, Edge<int>>(bidirectionalGraph1);
                
                yield return new TestCaseData(wrappedAdjacencyGraph1, arrayBidirectionalGraph1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(arrayBidirectionalGraph1, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                var arrayBidirectionalGraph2 = new ArrayBidirectionalGraph<int, Edge<int>>(bidirectionalGraph2);
                
                yield return new TestCaseData(wrappedAdjacencyGraph1, arrayBidirectionalGraph2)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(arrayBidirectionalGraph2, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = false
                };

                // Bidirectional adapter
                var bidirectionalAdapterGraph1 = new BidirectionalAdapterGraph<int, Edge<int>>(wrappedAdjacencyGraph1);

                yield return new TestCaseData(wrappedAdjacencyGraph1, bidirectionalAdapterGraph1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(bidirectionalAdapterGraph1, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                var bidirectionalAdapterGraph2 = new BidirectionalAdapterGraph<int, Edge<int>>(wrappedAdjacencyGraph3);

                yield return new TestCaseData(wrappedAdjacencyGraph1, bidirectionalAdapterGraph2)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(bidirectionalAdapterGraph2, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = false
                };

                // Matrix graph
                var matrixGraph1 = new BidirectionalMatrixGraph<Edge<int>>(5);
                matrixGraph1.AddEdgeRange(new[] { edge12, edge34 });

                yield return new TestCaseData(wrappedAdjacencyGraph1, matrixGraph1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(matrixGraph1, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = true
                };
                
                var matrixGraph2 = new BidirectionalMatrixGraph<Edge<int>>(5);
                matrixGraph2.AddEdge(edge12);

                yield return new TestCaseData(wrappedAdjacencyGraph1, matrixGraph2)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(matrixGraph2, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = false
                };

                // Cluster graph
                var clusterWrappedAdjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                clusterWrappedAdjacencyGraph1.AddVertexRange(new[] { 0, 1, 2, 3, 4 });
                clusterWrappedAdjacencyGraph1.AddEdgeRange(new[] { edge12, edge34 });
                var cluster1 = new ClusteredAdjacencyGraph<int, Edge<int>>(clusterWrappedAdjacencyGraph1);

                yield return new TestCaseData(wrappedAdjacencyGraph1, cluster1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(cluster1, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                var clusterWrappedAdjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                clusterWrappedAdjacencyGraph2.AddVertexRange(new[] { 0, 1, 2, 3, 4 });
                clusterWrappedAdjacencyGraph2.AddEdgeRange(new[] { edge12, edge34 });
                var cluster2 = new ClusteredAdjacencyGraph<int, Edge<int>>(clusterWrappedAdjacencyGraph2);
                cluster2.AddCluster();
                cluster2.AddCluster();

                yield return new TestCaseData(wrappedAdjacencyGraph1, cluster2)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(cluster2, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                var clusterWrappedAdjacencyGraph3 = new AdjacencyGraph<int, Edge<int>>();
                clusterWrappedAdjacencyGraph3.AddVertexRange(new[] { 0, 1, 2, 3, 4 });
                clusterWrappedAdjacencyGraph3.AddEdgeRange(new[] { edge12, edge34 });
                var cluster3 = new ClusteredAdjacencyGraph<int, Edge<int>>(clusterWrappedAdjacencyGraph3);
                var subGraph31 = cluster3.AddCluster();
                subGraph31.AddVertex(6);

                yield return new TestCaseData(wrappedAdjacencyGraph1, cluster3)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(cluster3, wrappedAdjacencyGraph1)
                {
                    ExpectedResult = false
                };

                // Edge list
                var adjacencyGraph = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph.AddVerticesAndEdgeRange(new[] { edge12, edge34 });
                var edgeListGraph1 = new EdgeListGraph<int, Edge<int>>();
                edgeListGraph1.AddEdgeRange(new[] { edge12, edge34 });

                yield return new TestCaseData(adjacencyGraph, edgeListGraph1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(edgeListGraph1, adjacencyGraph)
                {
                    ExpectedResult = true
                };

                var edgeListGraph2 = new EdgeListGraph<int, Edge<int>>();
                edgeListGraph2.AddEdge(edge12);
                
                yield return new TestCaseData(adjacencyGraph, edgeListGraph2)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(edgeListGraph2, adjacencyGraph)
                {
                    ExpectedResult = false
                };

                // TODO 2 undirected
                //var undirectedGraph1 = new UndirectedGraph<int, Edge<int>>();
                //undirectedGraph1.AddVertexRange(new[] { 1, 2, 3, 4, 5 });
                //undirectedGraph1.AddEdgeRange(new[] { edge12, edge35 });

                //yield return new TestCaseData(wrappedAdjacencyGraph1, undirectedGraph1)
                //{
                //    ExpectedResult = true
                //};

                //yield return new TestCaseData(undirectedGraph1, wrappedAdjacencyGraph1)
                //{
                //    ExpectedResult = true
                //};

                //var undirectedGraph2 = new UndirectedGraph<int, Edge<int>>();
                //undirectedGraph2.AddVertexRange(new[] { 1, 2 });
                //undirectedGraph2.AddEdge(edge12);

                //yield return new TestCaseData(wrappedAdjacencyGraph1, undirectedGraph2)
                //{
                //    ExpectedResult = false
                //};

                //yield return new TestCaseData(undirectedGraph2, wrappedAdjacencyGraph1)
                //{
                //    ExpectedResult = false
                //};

                #endregion
            }
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> ReversedGraphEquateTestCases
        {
            [UsedImplicitly]
            get
            {
                var edge12 = new Edge<int>(1, 2);
                var edge34 = new Edge<int>(3, 4);

                var bidirectionalGraph1 = new BidirectionalGraph<int, Edge<int>>();
                bidirectionalGraph1.AddVertexRange(new[] { 0, 1, 2, 3, 4 });
                bidirectionalGraph1.AddEdgeRange(new[] { edge12, edge34 });
                var bidirectionalGraph2 = new BidirectionalGraph<int, Edge<int>>();
                bidirectionalGraph2.AddVertexRange(new[] { 1, 2 });
                bidirectionalGraph2.AddEdge(edge12);

                // Reversed graph
                var reversedGraph1 = new ReversedBidirectionalGraph<int, Edge<int>>(bidirectionalGraph1);
                var reversedGraph2 = new ReversedBidirectionalGraph<int, Edge<int>>(bidirectionalGraph1);

                yield return new TestCaseData(reversedGraph2, reversedGraph1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(reversedGraph1, reversedGraph2)
                {
                    ExpectedResult = true
                };

                var reversedGraph3 = new ReversedBidirectionalGraph<int, Edge<int>>(bidirectionalGraph2);

                yield return new TestCaseData(reversedGraph1, reversedGraph3)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(reversedGraph3, reversedGraph1)
                {
                    ExpectedResult = false
                };
            }
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> CompressedGraphEquateTestCases
        {
            [UsedImplicitly]
            get
            {
                var edge12 = new SEquatableEdge<int>(1, 2);
                var edge34 = new SEquatableEdge<int>(3, 4);

                var adjacencyGraph1 = new AdjacencyGraph<int, SEquatableEdge<int>>();
                adjacencyGraph1.AddVertexRange(new[] { 0, 1, 2, 3, 4 });
                adjacencyGraph1.AddEdgeRange(new[] { edge12, edge34 });
                var adjacencyGraph2 = new AdjacencyGraph<int, SEquatableEdge<int>>();
                adjacencyGraph2.AddVertexRange(new[] { 1, 2 });
                adjacencyGraph2.AddEdge(edge12);

                // Compressed graph
                var compressedGraph1 = CompressedSparseRowGraph<int>.FromGraph(adjacencyGraph1);

                yield return new TestCaseData(adjacencyGraph1, compressedGraph1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(compressedGraph1, adjacencyGraph1)
                {
                    ExpectedResult = true
                };

                var compressedGraph2 = CompressedSparseRowGraph<int>.FromGraph(adjacencyGraph2);

                yield return new TestCaseData(adjacencyGraph1, compressedGraph2)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(compressedGraph2, adjacencyGraph1)
                {
                    ExpectedResult = false
                };
            }
        }

        #endregion

        [TestCaseSource(nameof(EquateTestCases))]
        public bool Equate(
            [NotNull] IEdgeListGraph<int, Edge<int>> g,
            [NotNull] IEdgeListGraph<int, Edge<int>> h)
        {
            return EquateGraphs.Equate(g, h);
        }

        [TestCaseSource(nameof(ReversedGraphEquateTestCases))]
        public bool EquateReversedGraph(
            [NotNull] IEdgeListGraph<int, SReversedEdge<int, Edge<int>>> g,
            [NotNull] IEdgeListGraph<int, SReversedEdge<int, Edge<int>>> h)
        {
            return EquateGraphs.Equate(g, h);
        }

        [TestCaseSource(nameof(CompressedGraphEquateTestCases))]
        public bool EquateCompressedGraph(
            [NotNull] IEdgeListGraph<int, SEquatableEdge<int>> g,
            [NotNull] IEdgeListGraph<int, SEquatableEdge<int>> h)
        {
            return EquateGraphs.Equate(g, h);
        }
    }
}