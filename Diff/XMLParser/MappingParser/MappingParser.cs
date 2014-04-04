using System;
using System.Xml;

namespace pgmpm.Diff.XMLParser.MappingParser
{
    class MappingParser
    {
        /// <summary>
        /// Parse a mapping from a file.
        /// </summary>
        /// <param name="filename">File to parse</param>
        /// <returns>Mapping</returns>
        /// <author>Carsten Cordes, Thomas Meents</author>
        public static Mapping ParseMapping(String filename)
        {
            var mapping = new Mapping();

            //Try load document
            var document = new XmlDocument();
            try
            {
                document.Load(filename);
            }
            catch (XmlException e)
            {
                return null;
                throw new Exception("Error while parsing: \n" + e.Message);
                
            }
            catch (ArgumentException e)
            {
                return null;
                throw new Exception("Error while opening file: \n" +e.Message);
            }

            //Find root element of Mapping.
            XmlNode mappingNode = document.GetElementsByTagName("mapping").Item(0);

            //Get Mapping name and representation Namespace.
            if (mappingNode != null)
            {
                if (mappingNode.Attributes != null)
                {
                    for (int i = 0; i < mappingNode.Attributes.Count; i++)
                    {
                        XmlNode attribute = mappingNode.Attributes.Item(i);
                        if (attribute.Name.ToLower().Equals("diagram"))
                            mapping.MappingName = attribute.Value;
                        if (attribute.Name.ToLower().Equals("namespace"))
                            mapping.Namespace = attribute.Value;
                    }
                }

                XmlNode patternsNode = null;
                XmlNode typesNode = null;

                //Mapping has no childs? return null.
                if (!mappingNode.HasChildNodes)
                    return null;

                //Get types and patterns node as entrypoint for types parsing and patterns parsing.
                for (int i = 0; i < mappingNode.ChildNodes.Count; i++)
                {
                    XmlNode currentNode = mappingNode.ChildNodes[i];
                    if (currentNode.Name.ToLower().Equals("patterns"))
                        patternsNode = currentNode;
                    if (currentNode.Name.ToLower().Equals("types"))
                        typesNode = currentNode;
                }

                //No Types? -> NULL
                if (typesNode == null || !typesNode.HasChildNodes)
                {
                    return null;
                }
                //Try Parsing all rules.
                for (int i = 0; i < typesNode.ChildNodes.Count; i++)
                {
                    MappingRule rule = CreateMapping(typesNode.ChildNodes.Item(i));
                    if (!mapping.AddRule(rule))
                    {
                        throw new Exception("Error while Mapping rule.");
                    }
                }

                //Parse Patterns with mapping.
                ParsePatterns(mapping, patternsNode);
            }

            return mapping;
        }

        /// <summary>
        /// Parse Patterns from mapping.
        /// </summary>
        /// <param name="mapping">Current (incomplete) mapping</param>
        /// <param name="patternsNode">Xmlnode Entrypoint for Pattern Parsing</param>
        /// <author>Carsten Cordes, Thomas Meents</author>
        private static void ParsePatterns(Mapping mapping, XmlNode patternsNode)
        {
            //If there are patterns
            if (patternsNode != null && patternsNode.HasChildNodes)
            {
                //Parse them.
                for (int i = 0; i < patternsNode.ChildNodes.Count; i++)
                {
                    XmlNode patternTypeNode = patternsNode.ChildNodes[i];
                    Pattern.PatternTypes patternType;
                    //Which kind of pattern?
                    switch (patternTypeNode.Name.ToLower())
                    {
                        case "sequence":
                            patternType = Pattern.PatternTypes.Sequence;
                            break;
                        case "orsplit":
                            patternType = Pattern.PatternTypes.Orsplit;
                            break;
                        case "orjoin":
                            patternType = Pattern.PatternTypes.Orjoin;
                            break;
                        case "xorsplit":
                            patternType = Pattern.PatternTypes.Xorsplit;
                            break;
                        case "xorjoin":
                            patternType = Pattern.PatternTypes.Xorjoin;
                            break;
                        case "andsplit":
                            patternType = Pattern.PatternTypes.Andsplit;
                            break;
                        case "andjoin":
                            patternType = Pattern.PatternTypes.Andjoin;
                            break;
                        case "forbidden":
                            patternType = Pattern.PatternTypes.Forbidden;
                            break;
                        //ignore non-fitting patterns.
                        default:
                            continue;
                    }
                    //Ignore empty Patterns.
                    if (!patternTypeNode.HasChildNodes)
                        continue;
                    //Map Pattern
                    for (int j = 0; j < patternTypeNode.ChildNodes.Count; j++)
                    {
                      //  mapping.AddPattern(new Pattern(patternTypeNode.ChildNodes[j], mapping, patternType));
                    }
                }
            }
        }


        /// <summary>
        /// Create MappingRule from XmlNode.
        /// </summary>
        /// <param name="node">Rule-Xml-Node</param>
        /// <returns></returns>
        /// <author>Carsten Cordes, Thomas Meents</author>
        private static MappingRule CreateMapping(XmlNode node)
        {
            MappingRule rule = null;
            String nodeName = node.Name.ToLower();
            //Which kind of node?
            switch (nodeName)
            {
                //Do source in standardized way:
                //Only parse representation and add name and type "source"
                case "source":
                    if (node.Attributes == null)
                        return null;
                    rule = new MappingRule
                    {
                        IsEdge = false,
                        TypeName = "source",
                        IdentifierName = "source"
                    };
                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        XmlNode attribute = node.Attributes.Item(i);
                        if (attribute.Name.ToLower().Equals("representation"))
                            rule.RepresentationType = attribute.Value;
                    }
                    break;

                //Do sink in standardized way:
                //Only parse representation and add name and type "sink"
                case "sink":
                    if (node.Attributes == null)
                        return null;
                    rule = new MappingRule
                    {
                        IsEdge = false,
                        TypeName = "sink",
                        IdentifierName = "sink"
                    };
                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        XmlNode attribute = node.Attributes.Item(i);
                        if (attribute.Name.ToLower().Equals("representation"))
                            rule.RepresentationType = attribute.Value;
                    }
                    break;
                //Parse Edge- and Node-Mapping equally.
                case "edge":
                case "node":
                    if (node.Attributes == null)
                        return null;
                    rule = new MappingRule
                    {
                        IsEdge = false
                    };
                    //Get values from Xml.
                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        XmlNode attribute = node.Attributes.Item(i);
                        if (attribute.Name.ToLower().Equals("type"))
                            rule.TypeName = attribute.Value;
                        if (attribute.Name.ToLower().Equals("representation"))
                            rule.RepresentationType = attribute.Value;
                        if (attribute.Name.ToLower().Equals("identifier"))
                            rule.IdentifierName = attribute.Value;
                    }
                    //Edge? -> Say it is an edge.
                    if (nodeName.Equals("edge"))
                        rule.IsEdge = true;
                    break;
            }

            return rule;
        }
    }
}
