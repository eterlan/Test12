using System;
using Opsive.UltimateInventorySystem.Core;
using Opsive.UltimateInventorySystem.UI.Grid;
using Opsive.UltimateInventorySystem.UI.Item;
using Opsive.UltimateInventorySystem.UI.Item.DragAndDrop;
using Opsive.UltimateInventorySystem.UI.Item.ItemViewModules;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using EventHandler = Opsive.Shared.Events.EventHandler;

public class Test1 : MonoBehaviour
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
         
        if (dropHandler.DropSuccess)
            itemInfo.PrevRotation = itemInfo.Rotation;
        else
            itemInfo.Rotation = itemInfo.PrevRotation;
        
        
        grid.Draw();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightControl) && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("paused");
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
        
        if (!Input.GetKeyDown(KeyCode.R)) return;
        var floatingItemView = cursorManager.FloatingItemView;
        if (floatingItemView == null) return;
        floatingItemView.ItemInfo.Rotate();
        // 旋转记录之后，选中才会更新Preview
        var itemViewSlot = EventSystem.current.currentSelectedGameObject.GetComponent<ItemViewSlot>();
        if (itemViewSlot == null) return;
        itemViewSlot.Select();
        
        // 不对，这个画的是背包中的物品
        // GetComponent<ItemShapeGrid>().Draw();
        
        // 刷新拖拽物品的旋转. 不能这样做，因为这样会丢失offset
        //floatingItemView.Refresh();
        for (var i = 0; i < floatingItemView.Modules.Count; i++)
        {
            if (floatingItemView.Modules[i] is ItemShapeItemView itemShapeItemView)
            {
                itemShapeItemView.SetRotation(floatingItemView.ItemInfo, true);
            }
        }



        // Debug.Log($"{cursorManager.FloatingItemView.ItemInfo.ItemStack.Rotation.ToString()}");
    }
}
