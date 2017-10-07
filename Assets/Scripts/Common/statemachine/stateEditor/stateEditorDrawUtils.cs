﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace artiMech
{
    /// <summary>
    /// A static class to help with some of the specific code needed for the
    /// state editor and its drawing functions
    /// </summary>
    public static class stateEditorDrawUtils
    {
        public static void DrawArrow(Vector3 startPos, Vector3 endPos,Rect winRectStart, Rect winRectEnd, int lineWidth, Color lineColor, int shadowWidth, Color shadowColor)
        {

            //clip the line through the window rects
            Vector2 colPos = new Vector2();
            if (LineRectIntersection(startPos, endPos, winRectStart, ref colPos))
                startPos = colPos;

            if (LineRectIntersection(startPos, endPos, winRectEnd, ref colPos))
                endPos = colPos;

            //Handles.color = lineColor;
            //Handles.DrawAAPolyLine(lineWidth, 2, arrowLine);

            for (int i = 0; i < 3; i++)
                Handles.DrawBezier(startPos, endPos, endPos, startPos, shadowColor, null, (i + shadowWidth) * 4);

            Handles.DrawBezier(startPos, endPos, endPos, startPos, lineColor, null, lineWidth);

            Handles.color = Color.white;
            for (float i=0;i<10;i+=0.5f)
                Handles.DrawWireCube(startPos, new Vector3(i, i, i));

            Handles.color = lineColor;
            Handles.DrawWireCube(startPos, new Vector3(10,10,10));

        }

        public static void DrawBezierCurve(Vector3 startPos, Vector3 endPos,float inSize,float outSize)
        {
            Vector3 startTan = new Vector3();
            Vector3 endTan = new Vector3();

            if (startPos.x < endPos.x)
            {
                startTan = startPos + Vector3.right * inSize;
                endTan = endPos + Vector3.left * outSize;
            }
            else
            {
                startTan = startPos + Vector3.right * -inSize;
                endTan = endPos + Vector3.left * -outSize;
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);

            //draw shadow
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++)
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 4);
        }

        /// <summary>
        /// A function to draw arrow curver to and from windows.
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="inSize"></param>
        /// <param name="outSize"></param>
        /// <param name="winRectStart"></param>
        /// <param name="winRectEnd"></param>
        /// <param name="color"></param>
        public static void DrawBezierArrowCurveWithWindowOffsets(Vector3 startPos,
                                                                    Vector3 endPos,
                                                                    float inSize,
                                                                    float outSize,
                                                                    Rect winRectStart, 
                                                                    Rect winRectEnd,
                                                                    int lineWidth,
                                                                    Color lineColor,
                                                                    Color shadowColor)
        {

            Vector3 startTan = new Vector3();
            Vector3 endTan = new Vector3();

            // bend in the right direction
            if (startPos.x < endPos.x)
            {
                startTan = startPos + Vector3.right * inSize;
                endTan = endPos + Vector3.left * outSize;
            }
            else
            {
                startTan = startPos + Vector3.right * -inSize;
                endTan = endPos + Vector3.left * -outSize;
            }


            Vector2 colPos = new Vector2();
            if(LineRectIntersection(startPos,endPos,winRectStart,ref colPos))
            {
                startPos = colPos;

            }

            if (LineRectIntersection(startPos, endPos, winRectEnd, ref colPos))
            {
                endPos = colPos;
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, lineColor, null, lineWidth);

            //draw shadow
            for (int i = 0; i < 3; i++)
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowColor, null, (i + 1) * 4); 

            
        }

        /// <summary>
        /// Line to AABB intersection function.  Returns a collision ref.
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="rect"></param>
        /// <param name="vectOut"></param>
        /// <returns></returns>
        private static bool LineRectIntersection(Vector2 startPos, Vector2 endPos, Rect rect, ref Vector2 vectOut)
        {
            Vector2 minXLineVect = startPos.x <= endPos.x ? startPos : endPos;
            Vector2 maxXLineVect = startPos.x <= endPos.x ? endPos : startPos;
            Vector2 minYLineVect = startPos.y <= endPos.y ? startPos : endPos;
            Vector2 maxYLineVect = startPos.y <= endPos.y ? endPos : startPos;

            double rectMaxX = rect.xMax;
            double rectMinX = rect.xMin;
            double rectMaxY = rect.yMax;
            double rectMinY = rect.yMin;

            if (minXLineVect.x <= rectMaxX && rectMaxX <= maxXLineVect.x)
            {
                double m = (maxXLineVect.y - minXLineVect.y) / (maxXLineVect.x - minXLineVect.x);

                double intersectionY = ((rectMaxX - minXLineVect.x) * m) + minXLineVect.y;

                if (minYLineVect.y <= intersectionY && intersectionY <= maxYLineVect.y && rectMinY <= intersectionY && intersectionY <= rectMaxY)
                {
                    vectOut = new Vector2((float)rectMaxX, (float)intersectionY);
                    return true;
                }
            }

            if (minXLineVect.x <= rectMinX && rectMinX <= maxXLineVect.x)
            {
                double m = (maxXLineVect.y - minXLineVect.y) / (maxXLineVect.x - minXLineVect.x);

                double intersectionY = ((rectMinX - minXLineVect.x) * m) + minXLineVect.y;

                if (minYLineVect.y <= intersectionY && intersectionY <= maxYLineVect.y
                    && rectMinY <= intersectionY && intersectionY <= rectMaxY)
                {
                    vectOut = new Vector2((float)rectMinX, (float)intersectionY);

                    return true;
                }
            }

            if (minYLineVect.y <= rectMaxY && rectMaxY <= maxYLineVect.y)
            {
                double rm = (maxYLineVect.x - minYLineVect.x) / (maxYLineVect.y - minYLineVect.y);

                double intersectionX = ((rectMaxY - minYLineVect.y) * rm) + minYLineVect.x;

                if (minXLineVect.x <= intersectionX && intersectionX <= maxXLineVect.x
                    && rectMinX <= intersectionX && intersectionX <= rectMaxX)
                {
                    vectOut = new Vector2((float)intersectionX, (float)rectMaxY);

                    return true;
                }
            }

            if (minYLineVect.y <= rectMinY && rectMinY <= maxYLineVect.y)
            {
                double rm = (maxYLineVect.x - minYLineVect.x) / (maxYLineVect.y - minYLineVect.y);

                double intersectionX = ((rectMinY - minYLineVect.y) * rm) + minYLineVect.x;

                if (minXLineVect.x <= intersectionX && intersectionX <= maxXLineVect.x
                    && rectMinX <= intersectionX && intersectionX <= rectMaxX)
                {
                    vectOut = new Vector2((float)intersectionX, (float)rectMinY);

                    return true;
                }
            }

            return false;
        }
    }
}