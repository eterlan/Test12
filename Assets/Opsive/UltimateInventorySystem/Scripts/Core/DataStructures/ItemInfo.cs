/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.Core.DataStructures
{
    using Opsive.UltimateInventorySystem.Core.InventoryCollections;
    using System;
    using UnityEngine;

    /// <summary>
    /// Specifies the amount of each item.
    /// </summary>
    [Serializable]
    public struct ItemInfo : IEquatable<ItemInfo>
    {
        // Test1 记录物品旋转角度
        public int            Rotation
        {
            get => ItemStack?.Rotation ?? 0;
            set => ItemStack.Rotation = value;
        }

        /// <summary>
        /// 只有在放置之后才会更新这个值，为了在拖动并旋转时记住之前的状态, 为了解决旋转后摆放在原位置不跳过检测的问题 <see cref="ItemShapeGridData.TryMoveIndex()"/>
        /// </summary>
        public int  PrevRotation
        {
            get => ItemStack?.PrevRotation ?? 0;
            set => ItemStack.PrevRotation = value;
        }

        /// <summary>
        /// 是否转置
        /// </summary>
        public bool IsTransposed => Rotation % 180 != 0;
        /// <summary>
        /// 在拖拽前是否转置
        /// </summary>
        public bool PrevIsTransposed => PrevRotation % 180 != 0;
        
        /// <summary>
        /// 旋转并记录旋转信息
        /// </summary>
        public void Rotate()
        {
            if (ItemStack == null)
            {
                Debug.LogWarning("ItemStack is null");
                return;
            }
            ItemStack.Rotation = (Rotation + 90) % 360;
        }

        /// <summary>
        /// 当拖拽移动失败后回到之前的旋转角度
        /// </summary>
        public void RevertRotation()
        {
            ItemStack.Rotation = ItemStack.PrevRotation;
        }
        
        public static ItemInfo None
        {
            get => new ItemInfo();
            set => throw new NotImplementedException();
        }

        [Tooltip("The item amount.")]
        [SerializeField] private ItemAmount m_ItemAmount;
        [Tooltip("The item collection.")]
        [System.NonSerialized] private ItemCollection m_ItemCollection;
        [Tooltip("The item stack within the itemCollection.")]
        [System.NonSerialized] private ItemStack m_ItemStack;

        public Item           Item
        {
            get => m_ItemAmount.Item;
            set => throw new NotImplementedException();
        }

        public int            Amount
        {
            get => m_ItemAmount.Amount;
            set => throw new NotImplementedException();
        }

        public ItemAmount     ItemAmount
        {
            get => m_ItemAmount;
            set => m_ItemAmount = value;
        }

        public ItemStack      ItemStack
        {
            get => m_ItemStack;
            set => m_ItemStack = value;
        }

        public ItemCollection ItemCollection
        {
            get => m_ItemCollection;
            set => m_ItemCollection = value;
        }

        public IInventory     Inventory
        {
            get => m_ItemCollection?.Inventory;
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="itemAmount">The item amount.</param>
        /// <param name="itemCollection">The item collection where the item comes from.</param>
        /// <param name="itemStack">The item stack where the item comes from within the item collection.</param>
        public ItemInfo(ItemAmount itemAmount, ItemCollection itemCollection, ItemStack itemStack)
        {
            m_ItemAmount = itemAmount;
            m_ItemStack = itemStack;
            m_ItemCollection = itemCollection;
        }

        /// <summary>
        /// Item Info constructor.
        /// </summary>
        /// <param name="itemAmount">The item amount.</param>
        /// <param name="itemCollection">The item collection where the item comes from.</param>
        public ItemInfo(ItemAmount itemAmount, ItemCollection itemCollection)
        {
            m_ItemAmount = itemAmount;
            m_ItemCollection = itemCollection;
            m_ItemStack = null;
        }

        /// <summary>
        /// ItemInfo constructor, copy the item info and change the item amount.
        /// </summary>
        /// <param name="itemAmount">The item amount.</param>
        /// <param name="otherItemInfo">The item info to copy.</param>
        public ItemInfo(ItemAmount itemAmount, ItemInfo otherItemInfo)
        {
            m_ItemAmount = itemAmount;
            m_ItemCollection = otherItemInfo.ItemCollection;
            m_ItemStack = otherItemInfo.ItemStack;
        }

        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="itemAmount">The item amount.</param>
        public ItemInfo(ItemAmount itemAmount)
        {
            m_ItemAmount = itemAmount;
            m_ItemCollection = null;
            m_ItemStack = null;
        }
        
        /// <summary>
        /// ItemInfo constructor, copy the item info and change the item amount.
        /// </summary>
        /// <param name="amount">The item amount.</param>
        /// <param name="otherItemInfo">The item info to copy.</param>
        public ItemInfo(int amount, ItemInfo otherItemInfo)
        {
            m_ItemAmount = new ItemAmount(otherItemInfo.Item, amount);
            m_ItemCollection = otherItemInfo.ItemCollection;
            m_ItemStack = otherItemInfo.ItemStack;
        }
        
        /// <summary>
        /// ItemInfo constructor, copy the item info and change the item amount.
        /// </summary>
        /// <param name="amount">The item amount.</param>
        /// <param name="otherItemInfo">The item info to copy.</param>
        public ItemInfo(ItemInfo otherItemInfo, int amount)
        {
            m_ItemAmount = new ItemAmount(otherItemInfo.Item, amount);
            m_ItemCollection = otherItemInfo.ItemCollection;
            m_ItemStack = otherItemInfo.ItemStack;
        }

        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="itemAmount">The amount.</param>
        public ItemInfo(Item item, int amount)
        {
            m_ItemAmount = (item, amount);
            m_ItemCollection = null;
            m_ItemStack = null;
        }
        
        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="itemAmount">The amount.</param>
        public ItemInfo(int amount, Item item)
        {
            m_ItemAmount = (item, amount);
            m_ItemCollection = null;
            m_ItemStack = null;
        }

        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="itemDefinition">The itemDefinition.</param>
        /// <param name="itemAmount">The amount.</param>
        public ItemInfo(ItemDefinition itemDefinition, int amount)
        {
            m_ItemAmount = (InventorySystemManager.CreateItem(itemDefinition), amount);
            m_ItemCollection = null;
            m_ItemStack = null;
        }
        
        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="itemDefinition">The itemDefinition.</param>
        /// <param name="itemAmount">The amount.</param>
        public ItemInfo(int amount, ItemDefinition itemDefinition)
        {
            m_ItemAmount = (InventorySystemManager.CreateItem(itemDefinition), amount);
            m_ItemCollection = null;
            m_ItemStack = null;
        }

        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="itemName">The itemName.</param>
        /// <param name="itemAmount">The amount.</param>
        public ItemInfo(string itemName, int amount)
        {
            m_ItemAmount = (InventorySystemManager.CreateItem(itemName), amount);
            m_ItemCollection = null;
            m_ItemStack = null;
        }
        
        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="itemName">The itemName.</param>
        /// <param name="itemAmount">The amount.</param>
        public ItemInfo(int amount, string itemName)
        {
            m_ItemAmount = (InventorySystemManager.CreateItem(itemName), amount);
            m_ItemCollection = null;
            m_ItemStack = null;
        }

        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="itemStack">The item stack where the item comes from within the item collection.</param>
        public ItemInfo(ItemStack itemStack)
        {
            if (itemStack == null) {
                m_ItemAmount = ItemAmount.None;
                m_ItemStack = null;
                m_ItemCollection = null;
                return;
            }
            m_ItemAmount = itemStack.ItemAmount;
            m_ItemStack = itemStack;
            m_ItemCollection = itemStack.ItemCollection;
        }
        
        /// <summary>
        /// ItemInfo constructor.
        /// </summary>
        /// <param name="itemAmount">The item amount.</param>
        /// <param name="itemStack">The item stack where the item comes from within the item collection.</param>
        public ItemInfo(int itemAmount, ItemStack itemStack)
        {
            if (itemStack == null) {
                m_ItemAmount = ItemAmount.None;
                m_ItemStack = null;
                m_ItemCollection = null;
                return;
            }
            
            m_ItemAmount = (itemAmount, itemStack.Item);
            m_ItemStack = itemStack;
            m_ItemCollection = itemStack.ItemCollection;
        }

        public static implicit operator ItemInfo((ItemAmount, ItemCollection, ItemStack) x)
            => new ItemInfo(x.Item1, x.Item2, x.Item3);

        public static implicit operator ItemInfo((Item, int, ItemCollection, ItemStack) x)
            => new ItemInfo((x.Item1, x.Item2), x.Item3, x.Item4);

        public static implicit operator ItemInfo((int, Item, ItemCollection, ItemStack) x)
            => new ItemInfo((x.Item1, x.Item2), x.Item3, x.Item4);

        public static implicit operator ItemInfo((ItemAmount, ItemCollection) x)
            => new ItemInfo(x.Item1, x.Item2);

        public static implicit operator ItemInfo((Item, int, ItemCollection) x)
            => new ItemInfo((x.Item1, x.Item2), x.Item3);

        public static implicit operator ItemInfo((int, Item, ItemCollection) x)
            => new ItemInfo((x.Item1, x.Item2), x.Item3);

        public static implicit operator ItemInfo((ItemAmount, ItemInfo) x)
            => new ItemInfo(x.Item1, x.Item2);

        public static implicit operator ItemInfo((Item, int, ItemInfo) x)
            => new ItemInfo((x.Item1, x.Item2), x.Item3);

        public static implicit operator ItemInfo((int, ItemInfo) x)
            => new ItemInfo((x.Item2.Item, x.Item1), x.Item2);

        public static explicit operator ItemInfo(ItemStack x)
            => new ItemInfo(x);
        
        public static explicit operator ItemInfo((int, ItemStack) x)
            => new ItemInfo(x.Item1, x.Item2);

        public static explicit operator ItemInfo(ItemAmount x)
            => new ItemInfo(x);

        public static explicit operator ItemInfo((Item, int) x)
            => new ItemInfo((x.Item1, x.Item2));

        public static explicit operator ItemInfo((int, Item) x)
            => new ItemInfo((x.Item1, x.Item2));

        /// <summary>
        /// Returns a readable format for an item info.
        /// </summary>
        /// <returns>The readable format for an Item Info.</returns>
        public override string ToString()
        {
            if (this == ItemInfo.None) {
                return "None ItemInfo";
            }

            var itemCollection = m_ItemCollection == null ? "ItemCollection is NULL" : m_ItemCollection.ToString();
            var itemStack = m_ItemStack == null ? "ItemStack is NULL" : m_ItemStack.ToString();

            return string.Format("{0} || {1} || {2}", m_ItemAmount, itemCollection, itemStack);
        }

        /// <summary>
        /// Do an Equivalence check.
        /// </summary>
        /// <param name="lhs">Left hand side object.</param>
        /// <param name="rhs">Right hand side object.</param>
        /// <returns>True if equivalent.</returns>
        public static bool operator ==(ItemInfo lhs, ItemInfo rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Do an Equivalence check.
        /// </summary>
        /// <param name="lhs">Left hand side object.</param>
        /// <param name="rhs">Right hand side object.</param>
        /// <returns>True if not equivalent.</returns>
        public static bool operator !=(ItemInfo lhs, ItemInfo rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// Checks if the objects are equivalent, by comparing values and parameters.
        /// </summary>
        /// <param name="other">The other object.</param>
        /// <returns>Returns true if equivalent.</returns>
        public bool Equals(ItemInfo other)
        {
            return m_ItemAmount == other.m_ItemAmount
                   && Equals(m_ItemCollection, other.m_ItemCollection)
                    && Equals(m_ItemStack, other.m_ItemStack);
        }

        /// <summary>
        /// Checks if the object are equivalent.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>Returns true if equivalent.</returns>
        public override bool Equals(object obj)
        {
            return obj is ItemInfo other && Equals(other);
        }

        /// <summary>
        /// Returns the hashcode for the object, computed from the value and parameters.
        /// </summary>
        /// <returns>The computed hashcode.</returns>
        public override int GetHashCode()
        {
            unchecked { return m_ItemAmount.GetHashCode() + m_ItemCollection?.GetHashCode() ?? 0 + m_ItemStack?.GetHashCode() ?? 0; }
        }
    }
}