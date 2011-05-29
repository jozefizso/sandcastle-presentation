using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Microsoft.Ddue.Tools.CommandLine {

    public static class XmlExtentions {
        public static string GetNodePath(this XPathNavigator navigator) {
            navigator = navigator.Clone();

            var nodeStack = new Stack<Tuple<string, int, XPathNodeType>>();

            do {
                XPathNodeType type = navigator.NodeType;
                string name = navigator.Name;
                int position = 1;
                while (navigator.MoveToPrevious()) {
                    if (navigator.Name == name)
                        position++;
                }

                nodeStack.Push(Tuple.Create(name, position, type));
            } while (navigator.MoveToParent() && (navigator.NodeType != XPathNodeType.Root));

            StringBuilder sb = new StringBuilder(256);

            foreach (var node in nodeStack) {
                if (node.Item3 == XPathNodeType.Attribute)
                    sb.AppendFormat("/@{0}", node.Item1);
                else
                    sb.AppendFormat("/{0}[{1}]", node.Item1, node.Item2, node.Item3);
            }

            return sb.ToString();
        }
    }

}
