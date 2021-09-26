using System.Collections.Generic;

public class LRUCache<TKey, TValue>
{
    private class Node
    {
        public Node prevNode;
        public Node nextNode;

        public TKey key;
        public TValue value;

        public Node(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }

    private Dictionary<TKey, Node> _nodes;
    private int _capacity;
    private Node _head;
    private Node _tail;

    public LRUCache(int capacity)
    {
        _capacity = capacity;
        _nodes = new Dictionary<TKey, Node>(capacity);
        _head = null;
        _tail = null;
    }

    public bool Contains(TKey key)
    {
        return _nodes.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        Node node;
        if (!_nodes.TryGetValue(key, out node))
        {
            value = default;

            return false;
        }

        MoveToHead(node);
        value = node.value;
        
        return true;
    }

    public void Add(TKey key, TValue value)
    {
        Node node = new Node(key, value);
        node.prevNode = null;
        node.nextNode = _head;

        if (_head != null)
        {
            _head.prevNode = node;
        }

        _head = node;

        if (_nodes.Count == _capacity)
        {
            _nodes.Remove(_tail.key);

            _tail = _tail.prevNode;
            _tail.nextNode = null;
        }

        _nodes.Add(key, node);

        if (_tail == null)
        {
            _tail = node;
        }
    }

    private void MoveToHead(Node node)
    {
        if (node != _head)
        {
            node.prevNode.nextNode = node.nextNode;

            if (node.nextNode != null)
            {
                node.nextNode.prevNode = node.prevNode;
            }
            else //tail
            {
                _tail = node.prevNode;
            }

            _head = node;
        }
    }
}
