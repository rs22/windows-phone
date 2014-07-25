﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace OwnCloud.WebDAV {
    public class DAVRequestBody {
        private Item _rootElement;

        public void WriteToStream(Stream requestStream) {
            using (var writer = XmlWriter.Create(requestStream)) {
                writer.WriteStartDocument();
                InsertNode(writer, _rootElement);
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// Creates a new request body.
        /// </summary>
        /// <param name="rootElement"></param>
        public DAVRequestBody(Item rootElement) {
            _rootElement = rootElement;
        }

        private void InsertNode(XmlWriter writer, Item item) {
            if (item.IsNamespaced) {
                writer.WriteStartElement(item.LocalName, item.Namespace);
            } else {
                writer.WriteStartElement(item.LocalName);
            }

            foreach (Item.AttributeNode attr in item.Attributes) {
                if (attr.IsNamespaced) {
                    writer.WriteStartAttribute(attr.LocalName, attr.Namespace);
                    writer.WriteAttributeString(attr.LocalName, attr.Namespace, attr.Value);
                } else {
                    writer.WriteStartAttribute(attr.LocalName);
                    writer.WriteAttributeString(attr.LocalName, attr.Value);
                }
                writer.WriteEndAttribute();
            }

            if (item.HasContent) {
                if (item.IsNamespaced) {
                    writer.WriteElementString(item.LocalName, item.Namespace, item.LocalValue.ToString());
                } else {
                    writer.WriteElementString(item.LocalName, item.LocalValue.ToString());
                }
            }

            if (item.HasChildren) {
                foreach (Item child in item.Children) {
                    InsertNode(writer, child);
                }
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// Creates a listening request with all default properties.
        /// </summary>
        /// <returns></returns>
        static public DAVRequestBody CreateAllPropertiesListing() {
            return new DAVRequestBody(
                new Item(Elements.PropertyFind, new List<Item>() {
                    new Item(Elements.Properties, new List<Item> {
                        new Item(RequestProperties.SupportedLock),
                        new Item(RequestProperties.LockDiscovery),
                        new Item(RequestProperties.GetContentLength),
                        new Item(RequestProperties.GetContentType),
                        new Item(RequestProperties.GetETag),
                        new Item(RequestProperties.GetLastModified),
                        new Item(RequestProperties.ResourceType),
                        new Item(RequestProperties.QuotaAvailableBytes),
                        new Item(RequestProperties.QuotaUsedBytes),
                        new Item(RequestProperties.GetContentLanguage),
                        new Item(RequestProperties.CreationDate),
                        new Item(RequestProperties.DisplayName)
                    })
                })
            );
        }

        /// <summary>
        /// Item-Sub-Class
        /// </summary>
        public class Item {

            public string LocalName = "";
            public object LocalValue = null;
            public string Namespace = "";

            public struct AttributeNode {
                public string Namespace;
                public string LocalName;
                public string Value;

                /// <summary>
                /// Finds out if we are using a namespace.
                /// </summary>
                public bool IsNamespaced {
                    get {
                        return Namespace.Length > 0;
                    }
                    private set {
                    }
                }

                /// <summary>
                /// Creates a new namespaced Attribute.
                /// </summary>
                /// <param name="localName">The local name.</param>
                /// <param name="ns">The namespace URI.</param>
                /// <param name="value">A value.</param>
                public AttributeNode(string localName, object value, string ns = XmlNamespaces.NsDav) {
                    LocalName = localName;
                    Value = value.ToString();
                    Namespace = ns;
                }
            }

            /// <summary>
            /// Holds the item attributes.
            /// </summary>
            public List<AttributeNode> Attributes {
                get;
                set;
            }

            public List<Item> Children {
                get;
                set;
            }

            /// <summary>
            /// Determines if there are any children.
            /// </summary>
            /// <returns></returns>
            public bool HasChildren {
                get {
                    return Children.Count > 0;
                }
                private set {
                }
            }

            public bool HasContent {
                get {
                    return LocalValue != null && !string.IsNullOrWhiteSpace(LocalValue.ToString());
                }
                set {
                }
            }

            /// <summary>
            /// Finds out if we are using a namespace.
            /// </summary>
            public bool IsNamespaced {
                get {
                    return Namespace.Length > 0;
                }
                private set {
                }
            }

            /// <summary>
            /// Creates a new item.
            /// </summary>
            /// <param name="localName">The local node name</param>
            /// <param name="value">Text or another element.</param>
            /// <param name="ns">The Namespace URI</param>
            public Item(string localName, string value = "", string ns = XmlNamespaces.NsDav) {
                Children = new List<Item>();
                Attributes = new List<AttributeNode>();
                LocalName = localName;
                LocalValue = value;
                Namespace = ns;
            }

            /// <summary>
            /// Creates a new item.
            /// </summary>
            /// <param name="localName">The local node name</param>
            /// <param name="children">Child element</param>
            /// <param name="value">Text or another element.</param>
            /// <param name="ns">The Namespace URI</param>
            public Item(string localName, Item children, string value = "", string ns = XmlNamespaces.NsDav) {
                Children = new List<Item>() { children };
                Attributes = new List<AttributeNode>();
                LocalName = localName;
                LocalValue = value;
                Namespace = ns;
            }

            /// <summary>
            /// Creates a new item.
            /// </summary>
            /// <param name="localName">The local node name</param>
            /// <param name="children">Child elements</param>
            /// <param name="value">Text or another element.</param>
            /// <param name="ns">The Namespace URI</param>
            public Item(string localName, List<Item> children, string value = "", string ns = XmlNamespaces.NsDav) {
                Children = children;
                Attributes = new List<AttributeNode>();
                LocalName = localName;
                LocalValue = value;
                Namespace = ns;
            }
        }
    }
}
