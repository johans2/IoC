using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using Cakewalk.IoC.Core;
using Cakewalk.IoC;
using System;
using System.Linq;

public class NodeBasedEditor : EditorWindow {
    private List<Node> nodes;
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    [MenuItem("Window/Node Based Editor")]
    private static void OpenWindow() {
        NodeBasedEditor window = GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Dependency graph");
    }

    private void OnEnable() {
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);
        nodeStyle.alignment = TextAnchor.MiddleCenter;

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
        selectedNodeStyle.alignment = TextAnchor.MiddleCenter;

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);

        CreateNodeForAllClassesDEBUG();
        LayoutGraph();
    }

    private void CreateNodeForAllClassesDEBUG() {
        Type[] types = Assembly.GetAssembly(typeof(BootStrapper)).GetTypes();

        List<Type> testTypes = new List<Type>();

        for(int i = 0; i < types.Length; i++) {
            if(types[i].IsSubclassOf(typeof(TestClass))) {
                testTypes.Add(types[i]);
            }
        }

        if(nodes == null) {
            nodes = new List<Node>();
        }

        if(connections == null) {
            connections = new List<Connection>();
        }
        if(drawnNodes == null) {
            drawnNodes = new Dictionary<Type, Node>();
        }

        int row_ = 0;

        for(int i = 0; i < testTypes.Count; i++) {

            parentNode = null;
            column = 0;

            if(!drawnNodes.ContainsKey(testTypes[i])) {
                DrawNodeRecursive(testTypes[i], row_);
            }
        }
    }





    Vector2 startPos = new Vector2(1, 1);
    int counterX = 0;
    int column = 0;
    Node parentNode;
    Dictionary<Type, Node> drawnNodes;

    // TODO: DO this.. https://en.wikipedia.org/wiki/Coffman%E2%80%93Graham_algorithm
    // 1. Lägg till incoming out outgoing nodes i node klassen.
    // 2. Skriv ut detta på boxarna och verifiera att det funkar.
    // 3. Implementera algoritmen i länken.

    private void DrawNodeRecursive(Type type, int row) {
        // Ta en type.
        // Rita låda för type.

        if(type.Name == "F") {
            int a = 1;
        }


        // Draw the node if it has not already been drawn.
        Node node;
        if(!drawnNodes.TryGetValue(type, out node)) {
            node = new Node(type.Name, startPos + new Vector2(200f * row, 100f * column), 200, 50, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle/*, OnClickInPoint, OnClickOutPoint*/);
            nodes.Add(node);
            drawnNodes.Add(type, node);
            Debug.Log("Drew node: " + node.title);
        }

        // If a parent is set, make a connection to it.
        if(parentNode != null) {
            connections.Add(new Connection(parentNode, node, null));
        }

        // Go lower
        column++;

        // Get all dependencies.
        FieldInfo[] depFields = Container.GetDependencyFields(type);

        // Recursion 
        // Go over all child nodes and do the same thing.
        for(int i = 0; i < depFields.Length; i++) {
            parentNode = node;
            DrawNodeRecursive(depFields[i].FieldType, i);
        }
    }

    List<Node> nodeOrdering = new List<Node>();
    private void LayoutGraph() {

        List<Node> allNodes = drawnNodes.Values.ToList();
        /*
        1. Construct a topological ordering of G in which the vertices are ordered lexicographically by the set of positions of their incoming neighbors.
        To do so, add the vertices one at a time to the ordering, 
        at each step choosing a vertex v to add such that the incoming neighbors of v are all already part of the partial ordering, 
        and such that the most recently added incoming neighbor of v is earlier than the most recently added incoming neighbor of any other vertex that could be added in place of v.
        If two vertices have the same most recently added incoming neighbor, the algorithm breaks the tie in favor of the one whose second most recently added incoming neighbor is earlier, etc.
        */


        // Add all nodes with no connections at all
        nodeOrdering.AddRange(allNodes.FindAll((n) => n.incomingDeps.Count == 0 && n.outgoingDeps.Count == 0));

        // Add all nodes with only outgoing connections.
        nodeOrdering.AddRange(allNodes.FindAll((n) => n.incomingDeps.Count == 0 && n.outgoingDeps.Count > 0));

        // Remove already added nodes list.
        allNodes.RemoveAll(n => nodeOrdering.Contains(n));

        // Go through all other nodes.

        int debugMaxIter = 200;

        while(allNodes.Count > 0) {

            List<Node> validNextNodes = allNodes.FindAll((n) => NodeOrderingContainsAllDeps(n));

            // För alla valid nodes n. Hämta incomingdep n' som ligger höst upp i högen . 
            List<TopOfOrderingNeighbor> topOfOrderingNodes = new List<TopOfOrderingNeighbor>();

            foreach(var validNextNode in validNextNodes) {

                int topOfOrderingListDepIndex = 0;
                // Vilken av mina incomingdeps ligger högst upp i högen?
                foreach(var dNode in validNextNode.incomingDeps) {
                    if(nodeOrdering.IndexOf(dNode) >= topOfOrderingListDepIndex) {
                        topOfOrderingListDepIndex = nodeOrdering.IndexOf(dNode);
                    }
                }

                topOfOrderingNodes.Add(new TopOfOrderingNeighbor() { index = topOfOrderingListDepIndex, node = validNextNode });
            }

            // Den nod n vars n' ligger längst ner i högen. Lägg till denna i högen.
            // aka ta den     TopOfOrderingNeighbor  som har lägst n.
            // TODO: This logic does not care about the 2nd degree neighbors if the first one has the same index.
            Node bestNode = null;
            int lowestTopIndex = int.MaxValue;
            foreach(var topOrdNode in topOfOrderingNodes) {
                if(topOrdNode.index < lowestTopIndex) {
                    bestNode = topOrdNode.node;
                    lowestTopIndex = topOrdNode.index;
                }
            }
            

            nodeOrdering.Add(bestNode);
            allNodes.Remove(bestNode);

            debugMaxIter--;
            if(debugMaxIter < 0) {
                Debug.LogWarning("Reached max iterations in layout algorithm.");
                return;
            }

        }


        /*
        2.Assign the vertices of G to levels in the reverse of the topological ordering constructed in the previous step.For each vertex v,
        add v to a level that is at least one step higher than the highest level of any outgoing neighbor of v, that does not already have W elements assigned to it,
        and that is as low as possible subject to these two constraints.
        */
        
        int W = 3;

        foreach(var node in nodeOrdering) {

            // Vilken av mina outgoing deps har högst level?
            int highestNeighborLevel = 0;
            foreach(var neighborNode in node.outgoingDeps) {
                if(neighborNode.level > highestNeighborLevel) {
                    highestNeighborLevel = neighborNode.level;
                }
            }
            
            bool foundGoodLevel = false;
            int nextLevel = 0;

            int debugMaxLevelIter = 500;

            while(!foundGoodLevel) {
                int levelToPlace = highestNeighborLevel + nextLevel;

                int numNodesOnThisLvl = nodeOrdering.FindAll(n => n.level == levelToPlace).Count;

                if(numNodesOnThisLvl < W) {
                    node.level = levelToPlace;
                    foundGoodLevel = true;
                }
                else {
                    nextLevel++;    
                }

                debugMaxLevelIter--;
                if(debugMaxLevelIter < 0) {
                    Debug.LogWarning("Reached max iterations in level algorithm.");
                    return; ;
                }

            }
        }

        // All nodes should now have levels..
        foreach(var node in nodeOrdering) {
            Debug.Log("Node: " + node.className + "  level: " + node.level);
        }

    }


    struct Level{

        public List<Node> nodes;
        public int num;
        public Level(int num) {
            nodes = new List<Node>();
            this.num = num;
        }
    }

    struct TopOfOrderingNeighbor {
        public int index;
        public Node node;
    }

    private bool NodeOrderingContainsAllDeps(Node n) {
        foreach(var node in n.incomingDeps) {
            if(!nodeOrdering.Contains(node)) {
                return false;
            }
        }
        return true;
    }

    private void OnGUI()
    {
        DrawConnections();
        DrawNodes();

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawNodes()
    {
        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }

    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        if (nodes != null)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }

        //nodes.Add(new Node(mousePosition, 200, 50, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint));
    }
    /*
    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }*/
    /*
    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }
    */
    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    private void CreateConnection(Node nodeA, Node nodeB)
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        connections.Add(new Connection(nodeA, nodeB, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }
}