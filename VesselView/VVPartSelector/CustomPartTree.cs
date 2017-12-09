﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VVPartSelector
{
    class CustomPartTree
    {
        /*
         * I bet theres a better way to do this, but whatever.
         * Here's the idea: make a sorta-copy of the vessels part tree
         * Apply any changes in the actual vessel to this copy
         * while trying your best to not move the menu cursor too much
         * and allow for folding/unfolding
         * and dont forget the actions
         */

        private CustomPartTreeItem rootItem;
        public CustomPartTreeItem selectedItem;

        public CustomPartTree(Vessel vessel) {

        }

        public bool rootSelected() {
            if (selectedItem == rootItem) return true;
            else return false;
        }

        public void updateTree(Vessel vessel) {
            if (vessel != null)
            {
                if (!vessel.isEVA)
                {
                    Part root = vessel.rootPart;
                    //MonoBehaviour.print("checking nullness");
                    if (rootItem == null)
                    {
                        rootItem = new CustomPartTreeItem(root, null);
                        selectedItem = rootItem;
                    }
                    //MonoBehaviour.print("checking nonequality");
                    if (rootItem.associatedPart != root)
                    {
                        rootItem = new CustomPartTreeItem(root, null);
                        selectedItem = rootItem;
                    }
                    //MonoBehaviour.print("beginning recursion");
                    recursiveUpdate(rootItem);
                    //MonoBehaviour.print("ending recursion");
                }
            }
            else {
                //a dummy root part, because dear god trying to check for null
                //EVERYWHERE would drive me mad
                if (rootItem == null) {
                    rootItem = new CustomPartTreeItem(new Part(), null);
                    selectedItem = rootItem;
                }
            }        
        }

        private bool isSelectedOrChildren(CustomPartTreeItem leaf) {
            if (leaf == selectedItem) return true;
            foreach (CustomPartTreeItem branch in leaf.children) {
                if (isSelectedOrChildren(branch)) return true;
            }
            return false;
        }

        public void recursiveUpdate(CustomPartTreeItem leaf) {
            if (leaf.associatedPart == null)
            {
                selectedItem = leaf.root;
                return;
            }
            //MonoBehaviour.print("now handling "+leaf.associatedPart.name);
            //MonoBehaviour.print("checking children count");
            //first update if either of the expandable menus exists
            if (leaf.associatedPart.children.Count != 0)
            {
                leaf.hasChildrn = true;
            }
            else {
                leaf.hasChildrn = false;
            }
            //MonoBehaviour.print("checking action count");
            if (leaf.getActivableEvents().Count() > 0)
            {
                leaf.hasActions = true;
            }
            else {
                leaf.hasActions = false;
            }
            //MonoBehaviour.print("onto updates");
            //then update the branches depending on any changes in the part list
            //but only bother if the list is expanded
            if (leaf.childrnExpanded) {
                //MonoBehaviour.print("children expanded");
                //first check if the branches even exist
                if (leaf.children == null)
                {
                    //MonoBehaviour.print("children null, creating");
                    //if not, create them
                    List<CustomPartTreeItem> leaves = new List<CustomPartTreeItem>();
                    //CustomPartTreeItem[] leaves = new CustomPartTreeItem[leaf.associatedPart.children.Count];
                    //int i = 0;
                    foreach (Part part in leaf.associatedPart.children)
                    {
                        if (part != null) {
                            leaves.Add(new CustomPartTreeItem(part, leaf));
                        }
                        //leaves[i] = new CustomPartTreeItem(part, leaf);
                    }
                    leaf.children = leaves.ToArray();
                }
                else {
                    //MonoBehaviour.print("children exist, checking for changes");
                    //if they do, check for changes
                    for (int i = 0; i < leaf.children.Count(); i++) {
                        if (leaf.associatedPart == null) {
                            
                        }
                        if (!leaf.associatedPart.children.Contains(leaf.children[i].associatedPart))
                        {
                            MonoBehaviour.print("child " + i + " of " + leaf.children.Count());
                            if (isSelectedOrChildren(leaf.children[i])) {
                               selectedItem = leaf;
                            }
                            leaf.removeBranch(i);
                        }
                        
                        
                    }
                    //MonoBehaviour.print("checking for parts not listed");
                    foreach (Part part in leaf.associatedPart.children)
                    {
                        bool found = false;
                        foreach (CustomPartTreeItem branch in leaf.children) {
                            if (branch.associatedPart == part) {
                                found = true;
                                break;
                            }
                        }
                        if (!found) {
                            leaf.addBranch(new CustomPartTreeItem(part, leaf));
                        }
                    }
                }
                //MonoBehaviour.print("updating recursively");
                //then recursively update
                foreach (CustomPartTreeItem branch in leaf.children)
                {
                    //MonoBehaviour.print("recursion step begin");
                    recursiveUpdate(branch);
                    //MonoBehaviour.print("recursion step end");
                }
            }else{
                //MonoBehaviour.print("children not expanded, nulling");
                leaf.children = null;
            }
            //now the list of actions
            if (leaf.actionsExpanded)
            {
                //MonoBehaviour.print("actions expanded");
                leaf.actionCount = leaf.getActivableEvents().Count();
            }
            else {
                //MonoBehaviour.print("actions not expanded, zeroing");
                leaf.actionCount = 0;
            }
        }
    }
}
