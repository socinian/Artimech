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
    public class editorRenameState : editorBaseState
    {
        stateWindowsNode m_WindowsSelectedNode = null;

        bool m_ActionConfirmed = false;
        bool m_ActionCancelled = false;

        stateRenameWindow m_RenameWindow = null;
        #region Accessors

        /// <summary>  Returns true if the action is confirmed. </summary>
        public bool ActionConfirmed { get { return m_ActionConfirmed; } set { m_ActionConfirmed = value; } }

        /// <summary>  Returns true if the action is cancelled. </summary>
        public bool ActionCancelled { get { return m_ActionCancelled; } set { m_ActionCancelled = value; }}

        #endregion

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public editorRenameState(GameObject gameobject) : base(gameobject)
        {
            m_RenameWindow = new stateRenameWindow(999999);
            //<ArtiMechConditions>
            m_ConditionalList.Add(new editor_Rename_To_Display("Display Windows"));

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
                    float x = m_RenameWindow.WinRect.x;
                    float y = m_RenameWindow.WinRect.y;
                    float width = m_RenameWindow.WinRect.width;
                    float height = m_RenameWindow.WinRect.height;

                    if (ev.mousePosition.x >= x && ev.mousePosition.x <= x + width)
                    {
                        if (ev.mousePosition.y >= y && ev.mousePosition.y <= y + height)
                        {
                            m_RenameWindow.SetPos(ev.mousePosition.x - (width * 0.5f), ev.mousePosition.y - (height * 0.5f));
                            stateEditorUtils.Repaint();
                        }
                    }
                }
            }

            m_RenameWindow.Update(this);

            //stateEditorUtils.Repaint();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            m_WindowsSelectedNode = stateEditorUtils.SelectedNode;
            const float windowSizeX = 300;
            const float windowSizeY = 100;
            m_RenameWindow.Set("Rename the Alias of the State", m_WindowsSelectedNode.GetTransformedPos().x, m_WindowsSelectedNode.GetTransformedPos().y, windowSizeX,windowSizeY);
            m_RenameWindow.ChangeName = m_WindowsSelectedNode.WindowStateAlias;
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