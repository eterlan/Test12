using Opsive.UltimateInventorySystem.Core;
using Opsive.UltimateInventorySystem.UI.Grid;
using Opsive.UltimateInventorySystem.UI.Item;
using Opsive.UltimateInventorySystem.UI.Item.DragAndDrop;
using Opsive.UltimateInventorySystem.UI.Item.ItemViewModules;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using EventHandler = Opsive.Shared.Events.EventHandler;

namespace Opsive.UltimateInventorySystem.UI
{
    public class ItemShapeRotate : MonoBehaviour
    {
        [SerializeField] private ItemViewSlotCursorManager cursorManager;
        private                  ItemViewDropHandler       dropHandler;
        private                  ItemShapeGrid             grid;
        private void OnEnable()
        {
            dropHandler = GetComponent<ItemViewDropHandler>();
            grid = GetComponent<ItemShapeGrid>();
            EventHandler.RegisterEvent(cursorManager.gameObject, EventNames.c_ItemViewSlotCursorManagerGameobject_EndMove, HandleOnDrop);
        }

        private void OnDisable()
        {
            EventHandler.UnregisterEvent(cursorManager.gameObject, EventNames.c_ItemViewSlotCursorManagerGameobject_EndMove, HandleOnDrop);
        }

        private void HandleOnDrop()
        {
            if (cursorManager == null || cursorManager.FloatingItemView == null)
                return;
            var itemInfo = cursorManager.FloatingItemView.ItemInfo;
            if (dropHandler == null)
                return;
         
            // 分清楚在拖拽的时候的旋转和物品栏中原物体的旋转信息, 并且在drop的时候根据结果决定用哪个旋转信息.
            if (dropHandler.DropSuccess)
                itemInfo.PrevRotation = itemInfo.Rotation;
            else
                itemInfo.Rotation = itemInfo.PrevRotation;
        
        
            grid.Draw();
        }

        void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.RightControl) && UnityEngine.Input.GetKey(KeyCode.RightShift) && UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("paused");
                EditorApplication.isPaused = !EditorApplication.isPaused;
            }
        
            if (!UnityEngine.Input.GetKeyDown(KeyCode.R)) return;
            var floatingItemView = cursorManager.FloatingItemView;
            if (floatingItemView == null) return;
            
            // 更新旋转信息
            floatingItemView.ItemInfo.Rotate();

            // 旋转记录之后，选中才会更新PreviewColor
            var itemViewSlot = EventSystem.current.currentSelectedGameObject.GetComponent<ItemViewSlot>();
            if (itemViewSlot == null) return;
            itemViewSlot.Select();
        
            // 更新拖摘物体的位置和旋转
            for (var i = 0; i < floatingItemView.Modules.Count; i++)
            {
                if (floatingItemView.Modules[i] is ItemShapeItemView itemShapeItemView)
                {
                    itemShapeItemView.SetRotation(floatingItemView.ItemInfo, true);
                }
            }
        }
    }
}