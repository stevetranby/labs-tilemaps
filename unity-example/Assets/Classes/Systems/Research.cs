using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System;

namespace ST
{
    /// <summary>
    /// Everything related to "research" both generating various types as well as the data structures to handle hierarchies and trees.
    /// 
    /// Research Types
    /// - technologies (RTS/SimCity)
    /// - experience (FF)
    /// - jobs (FFT)
    /// - sociopolitical (Civ)
    /// </summary>
    public class Research : MonoBehaviour {
        // treeExample;
    }

    public class TreeNode<T>
    {
        private readonly T _value;
        private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();
        
        public TreeNode(T value)
        {
            _value = value;
        }
        
        public TreeNode<T> this[int i]
        {
            get { return _children[i]; }
        }
        
        public TreeNode<T> Parent { get; private set; }
        
        public T Value { get { return _value; } }
        
        public ReadOnlyCollection<TreeNode<T>> Children
        {
            get { return _children.AsReadOnly(); }
        }
        
        public TreeNode<T> AddChild(T value)
        {
            var node = new TreeNode<T>(value) {Parent = this};
            _children.Add(node);
            return node;
        }
        
        public TreeNode<T>[] AddChildren(params T[] values)
        {
            var enumerable = values.Select(x => AddChild (x));
            return enumerable.ToArray();
        }
        
        public bool RemoveChild(TreeNode<T> node)
        {
            return _children.Remove(node);
        }
        
        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in _children)
                child.Traverse(action);
        }
        
        public IEnumerable<T> Flatten()
        {
            return new[] {Value}.Union(_children.SelectMany(x => x.Flatten()));
        }
    }

    delegate void TreeVisitor<T>(T nodeData);
    class NTree<T>
    {
        T data;
        LinkedList<NTree<T>> children;
        
        public NTree(T data)
        {
            this.data = data;
            children = new LinkedList<NTree<T>>();
        }
        
        public void addChild(T data)
        {
            children.AddFirst(new NTree<T>(data));
        }
        
        public NTree<T> getChild(int i)
        {
            foreach (NTree<T> n in children)
                if (--i == 0) return n;
            return null;
        }
        
        public void traverse(NTree<T> node, TreeVisitor<T> visitor)
        {
            visitor(node.data);
            foreach (NTree<T> kid in node.children)
                traverse(kid, visitor);
        }        
    }

}