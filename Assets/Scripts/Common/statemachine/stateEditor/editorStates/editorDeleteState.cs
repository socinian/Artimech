using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <name>nada</name>
    <posX>20</posX>
    <posY>40</posY>
    <sizeX>150</sizeX>
    <sizeY>80</sizeY>
  </State>
</stateMetaData>

#endif

#endregion

/// <summary>
/// This looks for the up click and then a condition is added and
/// the state is returned to 'Display Windows'
/// </summary>
namespace artiMech
{
    public class editorDeleteState : editorBaseState
    {
        stateWindowsNode m_WindowsSelectedNode = null;
        stateDeleteWindow m_DeleteWindowMessageBox = null;

        bool m_ActionConfirmed = false;
        bool m_ActionCancelled = false;
        #region Accessors

        /// <summary>  Returns true if the action is confirmed. </summary>
        public bool ActionConfirmed { get { return m_ActionConfirmed; } set { m_ActionConfirmed = value; } }

        /// <summary>  Returns true if the action is cancelled. </summary>
        public bool ActionCancelled { get { return m_ActionCancelled; } set { m_ActionCancelled = value; } }

        #endregion

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public editorDeleteState(GameObject gameobject) : base(gameobject)
        {
            m_DeleteWindowMessageBox = new stateDeleteWindow(999998);
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editor_Delete_To_Display("Display Windows"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// Fixed Update for physics and such from the game object.
        /// </summary>
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {
            base.UpdateEditorGUI();

            if (m_WindowsSelectedNode == null)
                return;

            Event ev = Event.current;
            stateEditorUtils.MousePos = ev.mousePosition;

            //Debug.Log("ev.type = " + ev.type.ToString());
            if (ev.button == 0)
            {
                //Debug.Log("ev = " + ev.button.ToString());
                if (ev.type == EventType.MouseDrag)
                {
                    float x = m_DeleteWindowMessageBox.WinRect.x;
                    float y = m_DeleteWindowMessageBox.WinRect.y;
                    float width = m_DeleteWindowMessageBox.WinRect.width;
                    float height = m_DeleteWindowMessageBox.WinRect.height;

                    if (ev.mousePosition.x >= x && ev.mousePosition.x <= x + width)
                    {
                        if (ev.mousePosition.y >= y && ev.mousePosition.y <= y + height)
                        {
                            m_DeleteWindowMessageBox.SetPos(ev.mousePosition.x - (width * 0.5f), ev.mousePosition.y - (height * 0.5f));
                            stateEditorUtils.Repaint();
                        }
                    }
                }
            }
        
            m_DeleteWindowMessageBox.Update(this);
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_WindowsSelectedNode = stateEditorUtils.SelectedNode;
            const float windowSizeX = 300;
            const float windowSizeY = 120;
            m_DeleteWindowMessageBox.Set("Delete State", m_WindowsSelectedNode.GetTransformedPos().x, m_WindowsSelectedNode.GetTransformedPos().y, windowSizeX, windowSizeY);
            m_DeleteWindowMessageBox.StateToDeleteName = m_WindowsSelectedNode.WindowStateAlias;
            m_DeleteWindowMessageBox.InitImage();
            m_ActionConfirmed = false;
            m_ActionCancelled = false;
            stateEditorUtils.Repaint();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {

        }
    }
}