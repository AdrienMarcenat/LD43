
public class GameNode : Node<NodeResource>
{}

public class GameEdge : Edge<NodeResource, EdgeResource>
{
    public GameEdge (EdgeResource data, Node<NodeResource> start, Node<NodeResource> end, bool isOriented = false)
        : base(data, start, end, isOriented)
    {}
}

public class Graph : Graph<NodeResource, EdgeResource>
{}