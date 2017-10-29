/// Artimech
/// 
/// Copyright © <2017> <George A Lancaster>
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
/// and associated documentation files (the "Software"), to deal in the Software without restriction, 
/// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
/// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
/// is furnished to do so, subject to the following conditions:
/// The above copyright notice and this permission notice shall be included in all copies 
/// or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
/// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
/// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
/// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
/// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
/// OTHER DEALINGS IN THE SOFTWARE.
/// 

using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace artiMech
{
    public class stateEditor : stateEditorBase
    {
        public stateEditor() : base()
        {
            stateEditorUtils.StateEditor = this;
            stateEditor window = (stateEditor)EditorWindow.GetWindow(typeof(stateEditor), true, "My Empty Window");
            window.Show();
        }


        /// <summary>
        /// Editor Update.
        /// </summary>
        new void Update()
        {
            stateEditorUtils.EditorCurrentGameObject = stateEditorUtils.GameObject;

            base.Update();

            //sets the 'was' gameobject so as to dectect a gameobject swap.
            stateEditorUtils.WasGameObject = stateEditorUtils.GameObject;

        }

        new void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawToolBar();
            GUILayout.EndHorizontal();

            // render populated state windows
            BeginWindows();
            m_CurrentState.UpdateEditorGUI();
            EndWindows();
        }

        void OnFocus()
        {

        }

        void DrawNodeWindow(int id)
        {
            GUI.DragWindow();
        }

        void DrawToolBar()
        {

            GUILayout.FlexibleSpace();

#pragma warning disable CS0618 // Type or member is obsolete
            stateEditorUtils.GameObject = (GameObject)EditorGUI.ObjectField(new Rect(3, 1, position.width - 50, 16), "Game Object", stateEditorUtils.GameObject, typeof(GameObject));
#pragma warning restore CS0618 // Type or member is obsolete

            if (GUILayout.Button("File", EditorStyles.toolbarDropDown))
            {
                GenericMenu toolsMenu = new GenericMenu();

                //In the wait state and an object is selected.
                if (m_CurrentState.m_StateName == "Wait" && stateEditorUtils.GameObject != null)
                    toolsMenu.AddItem(new GUIContent("Create"), false, OnCreateStateMachine);
                else
                    toolsMenu.AddDisabledItem(new GUIContent("Create"));

                if (m_CurrentState.m_StateName == "Display Windows" && stateEditorUtils.GameObject != null)
                    toolsMenu.AddItem(new GUIContent("Save"), false, OnSaveMetaData);
                else
                    toolsMenu.AddDisabledItem(new GUIContent("Save"));

                toolsMenu.AddSeparator("");

                if (m_CurrentState.m_StateName == "Display Windows" && stateEditorUtils.GameObject != null)
                    toolsMenu.AddItem(new GUIContent("Recenter"), false, OnRecenter);
                else
                    toolsMenu.AddDisabledItem(new GUIContent("Recenter"));

                toolsMenu.AddSeparator("");
                toolsMenu.AddItem(new GUIContent("About"), false, PrintAboutToConsole);
                toolsMenu.AddItem(new GUIContent("Wiki"), false, OnWiki);
                
                toolsMenu.DropDown(new Rect(Screen.width - 154, 0, 0, 16));

                EditorGUIUtility.ExitGUI();
            }

        }

        /// <summary>
        /// Callback to save the metadata for the window system.
        /// </summary>
        void OnSaveMetaData()
        {
            if(CurrentState is editorDisplayWindowsState)
            {
                editorDisplayWindowsState theState = (editorDisplayWindowsState)CurrentState;
                theState.Save = true;
            }

            for(int i=0;i<stateEditorUtils.StateList.Count;i++)
            {
                stateEditorUtils.StateList[i].SaveMetaData();
            }
        }

        void OnRecenter()
        {
            stateEditorUtils.TranslationMtx.Identity();
        }

        void OnCreateStateMachine()
        {
            if(m_CurrentState is editorWaitState)
            {
                editorWaitState waitState = m_CurrentState as editorWaitState;
                waitState.CreateBool = true;
            }
        }

        void PrintAboutToConsole()
        {
            Debug.Log(
            "<b><color=navy>Artimech (c) 2017 by George A Lancaster \n</color></b>"
            + "<i><color=grey>Click to view details</color></i>"
            + "\n"
            + "<color=blue>An Opensource Visual State Editor\n</color><b>"
            + "</b>"
            + "<color=teal>developed in Unity 5.x</color>"
            + " .\n\n");

         
        }

        void OnWiki()
        {
            Help.BrowseURL("https://github.com/Scramasax/Artimech/wiki");
        }

        public void EditorRepaint()
        {
            Repaint();
        }
    }
}
#endif