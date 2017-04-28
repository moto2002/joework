using System.Collections.Generic;
using System.IO;

public class XmlRW
{
    private Dictionary<string, Node> root = new Dictionary<string, Node>();

    private Dictionary<string, Node> noPathNodes = new Dictionary<string, Node>();

    private string txt;

    private int indent;

    private int indentNum;

    public class Node
    {
        public string name;

        public string text;

        public string upPath;

        public string path;

        public Dictionary<string, Node> child = new Dictionary<string, Node>();
    }

    #region Find

    public Node findNode(string p)
    {
        Node node = null;
        this.recursiveFind(this.noPathNodes, p, ref node);
        if (node == null)
        {
            this.recursiveFind(this.root, p, ref node);
        }
        return node;
    }

    private void recursiveFind(Dictionary<string, Node> tmpUis, string p, ref Node outUi)
    {
        if (tmpUis.Count > 0)
        {
            if (tmpUis.ContainsKey(p))
            {
                outUi = tmpUis[p];
                return;
            }
            foreach (KeyValuePair<string, Node> current in tmpUis)
            {
                this.recursiveFind(current.Value.child, p, ref outUi);
                if (outUi != null)
                {
                    break;
                }
            }
        }
    }

    public List<Node> findSameNameNode(string n)
    {
        return this.findSameNameNode(string.Empty, n);
    }

    public List<Node> findSameNameNode(string p, string n)
    {
        List<Node> result = new List<Node>();
        Dictionary<string, Node> tmpUis = (p == string.Empty) ? this.root : this.findNode(p).child;
        this.recursiveSameName(tmpUis, n, ref result);
        return result;
    }

    private void recursiveSameName(Dictionary<string, Node> tmpUis, string n, ref List<Node> outUi)
    {
        foreach (KeyValuePair<string, Node> current in tmpUis)
        {
            if (current.Value.name == n)
            {
                outUi.Add(current.Value);
            }
            else
            {
                this.recursiveSameName(current.Value.child, n, ref outUi);
            }
        }
    }

    public List<Node> getAllNode()
    {
        List<Node> result = new List<Node>();
        this.recursiveAll(this.root, ref result);
        return result;
    }

    private void recursiveAll(Dictionary<string, Node> tmpUis, ref List<Node> outUi)
    {
        foreach (KeyValuePair<string, Node> current in tmpUis)
        {
            outUi.Add(current.Value);
            this.recursiveAll(current.Value.child, ref outUi);
        }
    }

    public List<Node> getAllChildNode(string p)
    {
        Node node = this.findNode(p);
        List<Node> result = new List<Node>();
        this.recursiveAllChild(node.child, ref result);
        return result;
    }

    public List<Node> getAllChildNode(Dictionary<string, Node> tmpUis)
    {
        List<Node> result = new List<Node>();
        this.recursiveAllChild(tmpUis, ref result);
        return result;
    }

    private void recursiveAllChild(Dictionary<string, Node> tmpUis, ref List<Node> outUi)
    {
        foreach (KeyValuePair<string, Node> current in tmpUis)
        {
            outUi.Add(current.Value);
            this.recursiveAllChild(current.Value.child, ref outUi);
        }
    }

    #endregion

    public XmlRW()
    {
        this.init();
    }

    private void init()
    {
        this.txt = string.Empty;
        this.indent = 1;
        this.indentNum = 0;
    }

    public void createTempNode(string n, string text)
    {
        this.nodeAdds(true, string.Empty, n, text);
    }

    public void addTempNode(string upPath, string NodePath)
    {
        Node node = findNode(NodePath);
        Node node2 = findNode(upPath);
        List<Node> allChildNode = getAllChildNode(NodePath);
        foreach (Node current in allChildNode)
        {
            current.upPath = node2.path + "/" + current.upPath;
            current.path = node2.path + "/" + current.path;
        }
        node.upPath = node2.path + "/" + node.upPath;
        node.path = node2.path + "/" + node.path;
        node2.child.Add(node.path, node);
    }

    public void addRootNode(string n, string text)
    {
        this.nodeAdds(false, string.Empty, n, text);
    }

    public void addChildNode(string upPath, string n, string text)
    {
        this.nodeAdds(false, upPath, n, text);
    }

    public void addDictToNode(Dictionary<string, string> dict)
    {
        this.addDictToNode(string.Empty, dict);
    }

    public void addDictToNode(string upPath, Dictionary<string, string> dict)
    {
        string upPath2 = (upPath == string.Empty) ? string.Empty : findNode(upPath).path;
        foreach (KeyValuePair<string, string> current in dict)
        {
            this.nodeAdds(false, upPath2, current.Key, current.Value);
        }
    }

    public void nodeAdds(bool isCreate, string upPath, string n, string text)
    {
        Node node = null;
        Dictionary<string, Node> dictionary;
        if (upPath == string.Empty)
        {
            dictionary = (isCreate ? this.noPathNodes : this.root);
        }
        else
        {
            node = findNode(upPath);
            dictionary = node.child;
        }
        Node node2 = new Node();
        node2.text = text;
        node2.name = n;
        node2.upPath = ((node != null) ? node.path : "");
        node2.path = ((node != null) ? (node2.upPath + "/" + n) : n);
        dictionary.Add(node2.path, node2);
    }

    public void dataWrite(string name)
    {
        this.dataWrite(string.Empty, name);
    }

    public void dataWrite(string valPath, string name)
    {
        this.txt += "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\n";
        this.txt += "<root>\n";
        this.indentNum++;
        this.recursiveWrite(this.root);
        this.indentNum--;
        this.txt += "</root>";
        string path = (valPath == string.Empty) ? (Directory.GetCurrentDirectory() + "\\" + name) : (valPath + "\\" + name);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        StreamWriter streamWriter = new StreamWriter(fileStream);
        streamWriter.Write(this.txt);
        streamWriter.Close();
        fileStream.Close();
    }

    private void recursiveWrite(Dictionary<string, Node> n)
    {
        if (n.Count > 0)
        {
            foreach (KeyValuePair<string, Node> current in n)
            {
                for (int i = 0; i < this.indentNum * this.indent; i++)
                {
                    this.txt += "\t";
                }
                string txt = this.txt;
                this.txt = string.Concat(new string[]
                {
                        txt,
                        "<",
                        current.Value.name,
                        ">",
                        current.Value.text
                });
                this.indentNum++;
                if (current.Value.child.Count > 0)
                {
                    this.txt += "\n";
                }
                this.recursiveWrite(current.Value.child);
                this.indentNum--;
                if (current.Value.child.Count > 0)
                {
                    for (int j = 0; j < this.indentNum * this.indent; j++)
                    {
                        this.txt += "\t";
                    }
                }
                this.txt = this.txt + "</" + current.Value.name + ">";
                this.txt += "\n";
            }
        }
    }

    public void dataRead(string name)
    {
        this.dataRead(string.Empty, name);
    }

    public void dataRead(string valPath, string name)
    {
        string path = (valPath == string.Empty) ? (Directory.GetCurrentDirectory() + "\\" + name) : (valPath + "\\" + name);
        if (File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream);
            this.txt = streamReader.ReadToEnd();
            streamReader.Close();
            fileStream.Close();
        }
        this.analytic();
    }

    private void analytic()
    {
        Dictionary<string, Node> dictionary = new Dictionary<string, Node>();
        int num = this.txt.IndexOf("?>");
        string text = this.txt.Remove(0, num + 2);
        num = text.IndexOf("<root>");
        text = text.Remove(num, 6);
        num = text.IndexOf("</root>");
        text = text.Remove(num, 7);
        while (text.IndexOf("<") > -1)
        {
            string text2 = this.recursive(string.Empty, text, ref dictionary);
            num = text.IndexOf("</" + text2 + ">");
            text = text.Remove(0, num + 3 + text2.Length);
        }
    }

    private string recursive(string path, string content, ref Dictionary<string, Node> dict)
    {
        int num = content.IndexOf("<");
        string text = content.Remove(0, num + 1);
        num = text.IndexOf(">");
        string text2 = text.Remove(num);
        num = content.IndexOf("<" + text2 + ">");
        text = content.Remove(0, num + text2.Length + 2);
        num = text.IndexOf("</" + text2 + ">");
        text = text.Remove(num);
        if (text.IndexOf("<") > -1)
        {
            Node node = new Node();
            node.text = "";
            node.name = text2;
            node.upPath = ((path == string.Empty) ? "" : path);
            node.path = ((path != "") ? (path + "/" + node.name) : node.name);
            dict.Add(node.path, node);
            this.recursive(node.path, text, ref dict);
        }
        else
        {
            Node node2 = new Node();
            node2.text = text;
            node2.name = text2;
            node2.upPath = ((path == string.Empty) ? "" : path);
            node2.path = ((path != "") ? (path + "/" + node2.name) : node2.name);
            dict.Add(node2.path, node2);
        }
        return text2;
    }
}
