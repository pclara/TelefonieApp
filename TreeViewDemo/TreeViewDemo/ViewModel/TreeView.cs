using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeViewDemo.ViewModel
{
    public class TreeViewNodeVM
    {
        public TreeViewNodeVM()
        {
            ChildNode = new List<TreeViewNodeVM>();
        }

        public long SiteCode { get; set; }
        public string SiteName { get; set; }
        public string NodeName
        {
            get { return GetNodeName(); }
        }
        public IList<TreeViewNodeVM> ChildNode { get; set; }

        public string GetNodeName()
        {
            string temp = ChildNode.Count > 0 ? "    This employee manages " + ChildNode.Count : "    This employee is working without westing time in managing.";
            return this.SiteCode.ToString() + "  " + this.SiteName + temp;
        }
    }

    public class TreeNodesViewModel
    {
        public string treeVariables { get; set; }
        public string pathToVariables { get; set; }
        public string SelectedVariables { get; set; }
        public string type { get; set; }
    }

    public class JsTreeModel
    {
        public JsTreeData data { get; set; }
        public string state { get; set; }
        public string id { get; set; }
        public JsTreeAttribute attr { get; set; }
        public List<JsTreeModel> children { get; set; }


        public JsTreeModel(JsTreeData jstreeData, string nodeState, string nodeID, JsTreeAttribute nodeAttr, List<JsTreeModel> nodeChildren)
        {
            data = jstreeData;
            state = nodeState;
            id = nodeID;
            attr = nodeAttr;
            children = nodeChildren;

        }

        public JsTreeModel()
        {
            children = new List<JsTreeModel>();
            // TODO: Complete member initialization
        }

    }

    public class JsTreeAttribute
    {
        public string id { get; set; }
        public bool selected { get; set; }
        public string style { get; set; }
        public string description { get; set; }
        public string parentJudet { get; set; }

        public JsTreeAttribute(string attrID, bool attrSelected, string attrStyle, string attrDescription = "", string parentJudet = "")
        {
            id = attrID;
            selected = attrSelected;
            style = attrStyle;
            description = attrDescription;
            parentJudet = parentJudet;
        }

        public JsTreeAttribute()
        {
            // TODO: Complete member initialization
        }
    }
    public class JsTreeData
    {
        public string title { get; set; }
        public string icon { get; set; }
        public JsTreeData(string dataTitle, string dataIcon)
        {
            title = dataTitle;
            icon = dataIcon;
        }

        public JsTreeData()
        {
            // TODO: Complete member initialization
        }

    }


}