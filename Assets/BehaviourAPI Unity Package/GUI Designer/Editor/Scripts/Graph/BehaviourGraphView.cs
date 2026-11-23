
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
namespace BehaviourAPI.UnityToolkit.GUIDesigner.Editor.Graphs
{
    using BehaviourAPI.StateMachines;
    using Codice.Client.Commands.WkTree;
    using Framework;
    using System.Drawing.Printing;
    using System.Linq;
    using System.Security.Cryptography;
    using UnityEditor.PackageManager;


    /// <summary>
    /// Class used to represent the nodes of a <see cref="GraphData"/> element in a editor window.
    /// </summary>
    public class BehaviourGraphView : GraphView
    {
        #region -------------------------------------- Static fields --------------------------------------

        private static readonly Vector2 k_miniMapOffset = new Vector2(10, 10);
        private static readonly Vector2 k_miniMapSize = new Vector2(200, 200);

        private static readonly float k_MinZoomScale = 1.5f;
        private static readonly float k_MaxZoomScale = 3f;

        private static readonly string stylePath = "graph.uss";

        #endregion

        #region ---------------------------------------- Properties ---------------------------------------
        public GraphData graphData { get; private set; }

        public bool IsRuntime => m_CurrentGraphNodesProperty == null;

        public Action DataChanged { get; set; }
        public Action<List<int>> SelectionNodeChanged { get; set; }

        public Action<string> UndoRegisterOperationPerformed { get; set; }

        public IEdgeConnectorListener Connector => m_EdgeConnectorListener;

#if HAS_NEWTONSOFT
      Newtonsoft.Json.JsonSerializerSettings settings;
#endif

#endregion

        #region ------------------------------------- Private fields --------------------------------------

        GraphAdapter m_Adapter;

        MiniMap m_Minimap;

        Dictionary<string, NodeView> m_NodeViewMap = new Dictionary<string, NodeView>();

        IEdgeConnectorListener m_EdgeConnectorListener;

        SerializedProperty m_CurrentGraphNodesProperty;

        private EditorWindow m_EditorWindow;

        #endregion

        /// <summary>
        /// Create the default graphView
        /// </summary>
        /// <param name="editorWindow">The editor window that contains the graph view.</param>
        public BehaviourGraphView(EditorWindow editorWindow)
        {
            styleSheets.Add(BehaviourAPISettings.instance.GetStyleSheet(stylePath));

            // Background:
            GridBackground gridBackground = new GridBackground();
            Insert(0, gridBackground);

            // Minimap:
            m_Minimap = new MiniMap() { anchored = true };
            m_Minimap.SetPosition(new Rect(k_miniMapOffset, k_miniMapSize));
            RegisterCallback((GeometryChangedEvent evt) =>
            {
                m_Minimap.SetPosition(new Rect(evt.newRect.max - (k_miniMapOffset + k_miniMapSize), k_miniMapSize));
            });
            Add(m_Minimap);

            // Manipulators;
            SetupZoom(ContentZoomer.DefaultMinScale * k_MinZoomScale, ContentZoomer.DefaultMaxScale * k_MaxZoomScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            // Connector
            m_EdgeConnectorListener = new CustomEdgeConnector<EdgeView>(OnEdgeCreated, OnEdgeCreatedOutsidePort);

#if HAS_NEWTONSOFT
            // Events
            serializeGraphElements += CopyOperation;
            unserializeAndPaste += PasteOperation;

            canPasteSerializedData += CanPaste;
#endif
            nodeCreationRequest = HandleNodeCreationCall;
            graphViewChanged = HandleGraphViewChanged;
            m_EditorWindow = editorWindow;
            Undo.undoRedoPerformed += ReloadView;
#if HAS_NEWTONSOFT
            settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto, // writes $type for polymorphic fields
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                ContractResolver = new NewContractResolver(),
                Converters = { new UnityObjectConverter() },
                Error = { }
            };
#endif
        }

        private void ReloadView()
        {
            if (m_CurrentGraphNodesProperty != null)
            {
                //m_CurrentGraphNodesProperty.serializedObject.Update();
                UpdateGraph(graphData, m_CurrentGraphNodesProperty);
            }
        }

        /// <summary>
        /// Change the selected graph
        /// </summary>
        public void UpdateGraph(GraphData graphData, SerializedProperty serializedProperty = null)
        {
            ClearView();
            this.graphData = graphData;

            if (graphData == null)
            {
                m_CurrentGraphNodesProperty = null;
                return;
            }

            m_Adapter = GraphAdapter.GetAdapter(graphData.graph.GetType());

            if( serializedProperty == null && graphData.nodes.All(n => n.position == Vector2.zero))
            {
                AutoLayoutGraph();

            }

            m_CurrentGraphNodesProperty = serializedProperty;
            DrawGraph();
        }

        private void OnEdgeCreatedOutsidePort(EdgeView edge, Vector2 position)
        {
        }

        private void OnEdgeCreated(EdgeView edge)
        {
            var edgesToDelete = new HashSet<Edge>();

            foreach (Edge connection in edge.input.connections)
            {
                if (edge.input.capacity == Port.Capacity.Single || connection.output == edge.output)
                {
                    if (connection != edge) edgesToDelete.Add(connection);
                }
            }

            foreach (Edge connection in edge.output.connections)
            {
                if (edge.output.capacity == Port.Capacity.Single || connection.input == edge.input)
                {
                    if (connection != edge) edgesToDelete.Add(connection);
                }
            }

            if (edgesToDelete.Count > 0) DeleteElements(edgesToDelete);


            edge.input.Connect(edge);
            edge.output.Connect(edge);

            CreateConnection(edge);
        }

        private void HandleNodeCreationCall(NodeCreationContext ctx)
        {
            var nodeCreationWindowProvider = ElementCreatorWindowProvider.Create<NodeCreationWindow>(type => CreateNode(type, ctx.screenMousePosition));
            nodeCreationWindowProvider.SetHierarchy(m_Adapter.NodeHierarchy);
            SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), nodeCreationWindowProvider);
        }

        private GraphViewChange HandleGraphViewChanged(GraphViewChange change)
        {
            if (IsRuntime)
            {
                change.elementsToRemove = null;
            }

            List<NodeView> nodeViewsRemoved = new List<NodeView>();
            if (change.elementsToRemove != null)
            {
                UndoRegisterOperationPerformed?.Invoke("Deleted elements");
                //m_EditorWindow.RegisterOperation("Deleted elements");
                foreach (var element in change.elementsToRemove)
                {
                    if (element is NodeView nodeView)
                    {
                        graphData.nodes.Remove(nodeView.data);
                        m_NodeViewMap.Remove(nodeView.data.id);
                        nodeViewsRemoved.Add(nodeView);
                    }
                    else if (element is EdgeView edge)
                    {
                        var source = (NodeView)edge.output.node;
                        var target = (NodeView)edge.input.node;
                        source.OnDisconnected(edge);
                        target.OnDisconnected(edge);
                    }
                }
                SelectionNodeChanged?.Invoke(new List<int>());
            }
            else if (change.movedElements != null)
            {
                if (!IsRuntime)
                    UndoRegisterOperationPerformed?.Invoke("Moved elements");
                //m_EditorWindow.RegisterOperation("Moved elements");

                foreach (var element in change.movedElements)
                {
                    if (element is NodeView nodeView)
                    {
                        nodeView.OnMoved();
                    }
                }
            }

            foreach (var nodeRemoved in nodeViewsRemoved) nodeRemoved.OnDeleted();

            if (!IsRuntime)
                m_CurrentGraphNodesProperty.serializedObject.Update();

            if (nodeViewsRemoved.Count > 0)
            {
                RefreshProperties();
                RefreshViews();
            }

            if (!IsRuntime)
                DataChanged?.Invoke();

            return change;
        }

        public void UpdateProperties()
        {
            if (m_CurrentGraphNodesProperty != null)
                m_CurrentGraphNodesProperty.serializedObject.Update();

            DataChanged?.Invoke();
        }

        private void CreateNode(Type type, Vector2 pos)
        {
            if (m_CurrentGraphNodesProperty == null) return;

            var localPos = contentViewContainer.WorldToLocal(pos - m_EditorWindow.position.position);
            NodeData data = new NodeData(type, localPos);
            if (data != null)
            {
                UndoRegisterOperationPerformed?.Invoke("Created node");
                //m_EditorWindow.RegisterOperation("Created node");
                graphData.nodes.Add(data);
                m_CurrentGraphNodesProperty.serializedObject.Update();
                DrawNode(data);
            }
        }

        private void CreateConnection(EdgeView edgeView)
        {
            var source = edgeView.output.node as NodeView;
            var target = edgeView.input.node as NodeView;

            source.OnConnected(edgeView);
            target.OnConnected(edgeView);

            UndoRegisterOperationPerformed?.Invoke("Created connection");
            //m_EditorWindow.RegisterOperation("Created connection");

            AddElement(edgeView);
            UpdateProperties();
        }
#if HAS_NEWTONSOFT
        private NodeData CopyNode(NodeData ogNode)
        {
            NodeData newData = ogNode.DeepCopy(settings);
            newData.id = Guid.NewGuid().ToString();
            //Debug.Log("old ID: "+ogNode.id);
            //Debug.Log("new ID: "+newData.id);
            newData.position = newData.position + new Vector2(50, 50);
            if (newData != null)
            {
                // UndoRegisterOperationPerformed?.Invoke("Created node");
                //m_EditorWindow.RegisterOperation("Created node");
                //data.node = (BehaviourAPI.Core.Node)oldData.node.Clone();
                //data.node = node;
                //State oldState = data.node as State;
                //State state = data.node as State;
                //state.setAction(oldState.Action);
               
                graphData.nodes.Add(newData);
              //  Debug.Log("sent ID: " + newData.id);
                return newData;
            }
            return null;
        }
#endif
        private void ClearView()
        {
            foreach (var nodeView in m_NodeViewMap.Values)
            {
                nodeView.OnDestroy();
            }

            m_NodeViewMap.Clear();
            graphElements.ForEach(RemoveElement);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> validPorts = new List<Port>();
            NodeView startNodeView = startPort.node as NodeView;

            if (startNodeView == null) return validPorts;

            var bannedNodes = new HashSet<NodeData>();

            if (!graphData.graph.CanCreateLoops)
            {
                bannedNodes = startPort.direction == Direction.Input ? graphData.GetChildPathing(startNodeView.data) :
                graphData.GetParentPathing(startNodeView.data);
            }
            else if (!graphData.graph.CanRepeatConnection)
            {
                bannedNodes = startPort.direction == Direction.Input ? graphData.GetDirectChildren(startNodeView.data) :
                graphData.GetDirectParents(startNodeView.data);
            }

            foreach (Port port in ports)
            {
                if (startPort.direction == port.direction) continue;
                if (startPort.node == port.node) continue;

                NodeView otherNodeView = port.node as NodeView;

                if (bannedNodes.Contains(otherNodeView.data)) continue;
                if (startPort.direction == Direction.Input && !port.portType.IsAssignableFrom(startPort.portType)) continue;
                if (startPort.direction == Direction.Output && !startPort.portType.IsAssignableFrom(port.portType)) continue;


                validPorts.Add(port);
            }
            return validPorts;
        }

        #region ------------------------------- Draw elements -------------------------------

        private void DrawGraph()
        {
            if (graphData == null) return;

            for (int i = 0; i < graphData.nodes.Count; i++)
            {
                DrawNode(graphData.nodes[i]);
            }

            for (int i = 0; i < graphData.nodes.Count; i++)
            {
                NodeData nodeData = graphData.nodes[i];
                DrawConnections(nodeData);
            }
        }

        private void DrawNode(NodeData nodeData)
        {
            NodeDrawer drawer = NodeDrawer.Create(nodeData.node);
            var index = graphData.nodes.IndexOf(nodeData);
            //Debug.Log(index);
            NodeView mNodeView = new NodeView(nodeData, drawer, this, m_CurrentGraphNodesProperty?.GetArrayElementAtIndex(index));

            if (IsRuntime)
            {
                mNodeView.capabilities -= Capabilities.Deletable;
            }

            m_NodeViewMap.TryAdd(nodeData.id, mNodeView);
           // Debug.Log(JsonUtility.ToJson(nodeData));
            AddElement(mNodeView);
        }

        private void DrawConnections(NodeData nodeData)
        {
            if (nodeData.node != null && nodeData.node.MaxInputConnections == 0) return;

            if (!m_NodeViewMap.TryGetValue(nodeData.id, out NodeView sourceNodeView)) return;

            for (int i = 0; i < nodeData.childIds.Count; i++)
            {
                if (!m_NodeViewMap.TryGetValue(nodeData.childIds[i], out NodeView targetNodeView)) return;

                Port sourcePort = sourceNodeView.GetBestPort(targetNodeView, Direction.Output);
                Port targetPort = targetNodeView.GetBestPort(sourceNodeView, Direction.Input);
                EdgeView edge = sourcePort.ConnectTo<EdgeView>(targetPort);

                if (IsRuntime)
                {
                    edge.capabilities -= Capabilities.Deletable;
                    edge.capabilities -= Capabilities.Selectable;
                }

                AddElement(edge);

                sourceNodeView.OnConnected(edge, false);
                targetNodeView.OnConnected(edge, false);
            }
        }

        #endregion

        #region --------------------------------- Selection ---------------------------------

        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
            OnSelectionChange();
        }

        public override void RemoveFromSelection(ISelectable selectable)
        {
            base.RemoveFromSelection(selectable);
            OnSelectionChange();
        }

        public override void ClearSelection()
        {
            base.ClearSelection();
            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            List<int> selectedNodeIndex = new List<int>();
            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i] is NodeView nodeView)
                {
                    int id = graphData.nodes.IndexOf(nodeView.data);
                    selectedNodeIndex.Add(id);
                }
            }
            SelectionNodeChanged?.Invoke(selectedNodeIndex);
        }

        #endregion

        #region --------------- Other methods ----------------

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (evt.target is GraphView || evt.target is Node || evt.target is Group)
            {
#if HAS_NEWTONSOFT
                evt.menu.AppendSeparator();
                evt.menu.AppendAction("Copy.", delegate
                {
                    CopySelectionCallback();
                }, (DropdownMenuAction a) => canCopySelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendAction("Paste", delegate
                {
                    PasteCallback();
                }, (DropdownMenuAction a) => canPaste ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendAction("Cut", delegate
                {
                    CutSelectionCallback();
                }, (DropdownMenuAction a) => canCopySelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendAction("Duplicate", delegate
                {
                    DuplicateSelectionCallback();
                }, (DropdownMenuAction a) => canDuplicateSelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendAction("Delete", delegate
                {
                    DeleteSelectionCallback(AskUser.DontAskUser);
                }, (DropdownMenuAction a) => canDeleteSelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendSeparator();
            
#endif
            }
            evt.menu.AppendAction("Create Node", dma =>
            {
                nodeCreationRequest(new NodeCreationContext() { screenMousePosition = dma.eventInfo.mousePosition + m_EditorWindow.position.position, target = null, index = -1 });
            }, IsRuntime ? DropdownMenuAction.Status.Hidden :
                (m_CurrentGraphNodesProperty != null) ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled
            );
            evt.menu.AppendAction("Auto layout", _ => AutoLayoutGraph());
        }
        private class ClipboardContainer 
        {
            public State state;
            public Vector2 pos;
            public string name;
        }

#if HAS_NEWTONSOFT
        private string CopyOperation(IEnumerable<GraphElement> elements)
        {
            string data = string.Empty;
            List<string> nodeList = new List<string>();
            ClipboardData clipboardData = new ClipboardData();
            clipboardData.mostRestrictiveNodeType = typeof(BehaviourAPI.Core.BehaviourGraph);
         //   List<string> datalist = new List<string>();
            foreach (GraphElement element in elements)
            {
               // Debug.Log(element);
                //EdgeView edgeView = element as EdgeView;
                //if (edgeView != null)
                //{
                //    NodeView input = (NodeView)edgeView.input.node;
                //    NodeView output = (NodeView)edgeView.output.node;

                //    Debug.Log(input.data.id);
                //    Debug.Log(output.data.id);

                //}
                NodeView view = element as NodeView;
                if (view != null)
                {
                    if (view.data.node.GraphType.IsSubclassOf(clipboardData.mostRestrictiveNodeType))
                    {
                        clipboardData.mostRestrictiveNodeType = view.data.node.GraphType;
                    }
                    string s = Newtonsoft.Json.JsonConvert.SerializeObject(view.data, settings);
                    nodeList.Add(s);
                }
            }
            clipboardData.data = nodeList.ToArray();
            data = Newtonsoft.Json.JsonConvert.SerializeObject(clipboardData, settings);
            return data;
        }

        private void PasteOperation(string operationName, string data)
        {
          //  Debug.Log(data);
            List<string> allNodesIds = new List<string>();
            if (data != "")
            {
                //data=  data.Substring(1, data.Length - 1);
                //data.Remove(data.Length - 1);
                //data.Remove(0);
                ClipboardData clipboardData = Newtonsoft.Json.JsonConvert.DeserializeObject<ClipboardData>(data, settings);

                string[] datalist = clipboardData.data;

                //NodeData nodeData = new NodeData();
                //EditorJsonUtility.FromJsonOverwrite(data, nodeData);
                //NodeView nodeview = element as NodeView;
                //DrawNode(nodeview.data);
                Dictionary<string, string> oldIdtoNewId = new Dictionary<string, string>();
                List<NodeData> allNodes = new List<NodeData>();
                foreach (string s in datalist)
                {
                    NodeData nodeData = Newtonsoft.Json.JsonConvert.DeserializeObject<NodeData>(s, settings);
                    if (nodeData != null)
                    {
                        if (nodeData.references != null)
                        {
                            foreach (var reference in nodeData.references)
                            {
                                if (reference == null || string.IsNullOrEmpty(reference.FieldName))
                                {
                                    Debug.LogWarning($"Referencia erronea: {Newtonsoft.Json.JsonConvert.SerializeObject(reference)}");
                                    continue;
                                }

                                var field = nodeData.node.GetType().GetField(
                                    reference.FieldName,
                                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                if (field == null)
                                {
                                    Debug.LogWarning($"Campo '{reference.FieldName}' no existe para {nodeData.node.GetType()}");
                                    continue;
                                }
                                var value = reference.Value;
                                if (value != null && !field.FieldType.IsInstanceOfType(value))
                                    continue;

                                field.SetValue(nodeData.node, value);
                            }
                        }
                        NodeData newData = CopyNode(nodeData);

                        if (newData != null)
                        {
                            oldIdtoNewId.Add(nodeData.id, newData.id);
                            allNodes.Add(newData);
                            //Debug.Log("saved ID: " + newData.id);
                            allNodesIds.Add(newData.id);
                        }
                        else
                        {
                            Debug.Log("Selection is null");
                        }
                    }
                }
             
                // CreateNode(element.data.node.GetType(), element.data.position);
                foreach (NodeData nodeData in allNodes)
                {
                    swapFromDictionary(oldIdtoNewId, nodeData.childIds);
                    swapFromDictionary(oldIdtoNewId, nodeData.parentIds);
                }
            }

            m_CurrentGraphNodesProperty.serializedObject.Update();
            ClearView();
            DrawGraph();
            ClearSelection();
            foreach (string id in allNodesIds)
            {
                m_NodeViewMap.TryGetValue(id, out var node);
                if (node != null)
                    AddToSelection(node);
            }
            CopySelectionCallback();
        }
        public bool CanPaste(string data)
        {
           // Debug.Log(data);
            try
            {
                ClipboardData clipboardData =Newtonsoft.Json.JsonConvert.DeserializeObject<ClipboardData>(data, settings);
                if (clipboardData == null || clipboardData.mostRestrictiveNodeType == null || clipboardData.data == null)
                    return false;
                return checkGraphType(clipboardData.mostRestrictiveNodeType);
            }
            catch
            {
                return false;
            }
           
        }
#endif

        private Type getGraphType()
        {
            return graphData.graph.GetType();
        }
        private bool checkGraphType(Type clipboardType)
        {
            if (getGraphType().Equals(clipboardType) || getGraphType().IsSubclassOf(clipboardType))
            {
                return true;
            }
            return false;
        }
        private void swapFromDictionary(Dictionary<string, string> dict,List<string> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                string key = list[i];
                if(dict.TryGetValue(key, out var value)){
                    list[i] = value;
                }
                else
                {
                    list.RemoveAt(i);
                }
            }
        }

        private void AutoLayoutGraph()
        {
            UndoRegisterOperationPerformed?.Invoke("Auto layout graph");
            //m_EditorWindow.RegisterOperation("Auto layout graph");
            m_Adapter.AutoLayout(graphData);

            if (!IsRuntime)
                m_CurrentGraphNodesProperty.serializedObject.Update();

            RefreshViews();
        }

        /// <summary>
        /// Change the index of a node.
        /// </summary>
        public void SetNodeAsFirst(NodeView nodeView)
        {
            UndoRegisterOperationPerformed?.Invoke("Change first node");
            //m_EditorWindow.RegisterOperation("Change first node");
            graphData.nodes.MoveAtFirst(nodeView.data);
            m_CurrentGraphNodesProperty.serializedObject.Update();

            RefreshViews();
            RefreshProperties();
        }

        private void RefreshViews()
        {
            for (int i = 0; i < graphData.nodes.Count; i++)
            {
                var data = graphData.nodes[i];
                if(m_NodeViewMap.TryGetValue(data.id, out var view)) 
                {
                    view.RefreshView();
                }
            }
        }

        private void RefreshProperties()
        {
            for (int i = 0; i < graphData.nodes.Count; i++)
            {
                var data = graphData.nodes[i];
                var view = m_NodeViewMap[data.id];
                view.UpdateSerializedProperty(m_CurrentGraphNodesProperty.GetArrayElementAtIndex(i));
            }
        }

        public void RefreshSelectedNodesProperties()
        {
            foreach (var elem in selection)
            {
                if (elem is NodeView nodeView)
                {
                    nodeView.OnPropertyChanged();
                }
            }
        }

        public void RegisterUndo(string operationName)
        {
            UndoRegisterOperationPerformed?.Invoke(operationName);
            //m_EditorWindow.RegisterOperation(operationName);
        }

        public void SetMinimapVisibility(bool active)
        {
            if (active) m_Minimap.Enable();
            else m_Minimap.Disable();
        }

        internal void DuplicateSelection()
        {
            DuplicateSelectionCallback();
        }

        internal void CopySelection()
        {
            CopySelectionCallback();
        }
        internal void PasteSelection()
        {
            PasteCallback();
        }
        internal void CutSelection()
        {
            CutSelectionCallback();
        }
        internal bool canPasteSelection() { return canPaste; }
#endregion
    }
    public class ClipboardData
    {
       public Type mostRestrictiveNodeType;
       public string[] data;
    }
}
