//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
// Fix: Jarjin Lee 
// Use: Put this script on the UIGrid object.
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace LuaFramework {
    public class WrapGrid : MonoBehaviour {
        Transform mTrans;
        UIPanel mPanel;
        UIScrollView mScroll;
        bool mHorizontal = false;
        bool mFirstTime = true;
        List<Transform> mChildren = new List<Transform>();

        /// <summary>
        /// Initialize everything and register a callback with the UIPanel to be notified when the clipping region moves.
        /// </summary>

        protected virtual void Start() {
            InitGrid();
            mFirstTime = false;
        }

        /// <summary>
        /// Cache the scroll view and return 'false' if the scroll view is not found.
        /// </summary>
        public void InitGrid() {
            mTrans = transform;
            mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
            mScroll = mPanel.GetComponent<UIScrollView>();

            if (mScroll != null) {
                mScroll.GetComponent<UIPanel>().onClipMove = OnMove;
            }

            mChildren.Clear();
            for (int i = 0; i < mTrans.childCount; ++i)
                mChildren.Add(mTrans.GetChild(i));

            // Sort the list of children so that they are in order
            mChildren.Sort(UIGrid.SortByName);

            if (mScroll == null) return;
            if (mScroll.movement == UIScrollView.Movement.Horizontal) mHorizontal = true;
            else if (mScroll.movement == UIScrollView.Movement.Vertical) mHorizontal = false;

            WrapContent();
        }

        /// <summary>
        /// Callback triggered by the UIPanel when its clipping region moves (for example when it's being scrolled).
        /// </summary>

        protected virtual void OnMove(UIPanel panel) { WrapContent(); }

        void WrapContent() {
            Vector3[] corners = mPanel.worldCorners;

            for (int i = 0; i < 4; ++i) {
                Vector3 v = corners[i];
                v = mTrans.InverseTransformPoint(v);
                corners[i] = v;
            }
            Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);

            if (mHorizontal) {  //横向
                for (int i = 0, imax = mChildren.Count; i < imax; ++i) {
                    Transform t = mChildren[i];
                    float distance = t.localPosition.x - center.x;
                    float min = corners[0].x - 100;
                    float max = corners[2].x + 100;

                    distance += mPanel.clipOffset.x - mTrans.localPosition.x;
                    if (!UICamera.IsPressed(t.gameObject)) {
                        NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
                    }
                }
            } else {            //竖向
                for (int i = 0, imax = mChildren.Count; i < imax; ++i) {
                    Transform t = mChildren[i];
                    float distance = t.localPosition.y - center.y;
                    float min = corners[0].y - 100;
                    float max = corners[2].y + 100;

                    distance += mPanel.clipOffset.y - mTrans.localPosition.y;
                    if (!UICamera.IsPressed(t.gameObject)) {
                        bool active = t.gameObject.activeSelf;
                        bool willactive = distance > min && distance < max;
                        if (active == willactive) continue;
                        NGUITools.SetActive(t.gameObject, willactive, false);
                    }
                }
            }
        }
    }
}