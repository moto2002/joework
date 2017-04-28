namespace Mono.Xml
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Security;

    public class SecurityParser : SmallXmlParser, SmallXmlParser.IContentHandler
    {
        private SecurityElement current;
        private SecurityElement root;
        private Stack stack = new Stack();

        public void LoadXml(string xml)
        {
            this.root = null;
            this.stack.Clear();
            base.Parse(new StringReader(xml), this);
        }

        public void OnChars(string ch)
        {
            this.current.Text = ch;
        }

        public void OnEndElement(string name)
        {
            this.current = (SecurityElement) this.stack.Pop();
        }

        public void OnEndParsing(SmallXmlParser parser)
        {
        }

        public void OnIgnorableWhitespace(string s)
        {
        }

        public void OnProcessingInstruction(string name, string text)
        {
        }

        public void OnStartElement(string name, SmallXmlParser.IAttrList attrs)
        {
            SecurityElement child = new SecurityElement(name);
            if (this.root == null)
            {
                this.root = child;
                this.current = child;
            }
            else
            {
                ((SecurityElement) this.stack.Peek()).AddChild(child);
            }
            this.stack.Push(child);
            this.current = child;
            int length = attrs.Length;
            for (int i = 0; i < length; i++)
            {
                this.current.AddAttribute(attrs.GetName(i), attrs.GetValue(i));
            }
        }

        public void OnStartParsing(SmallXmlParser parser)
        {
        }

        public SecurityElement ToXml()
        {
            return this.root;
        }
    }
}

