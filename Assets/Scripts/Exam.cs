using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Exam
{
    public class MaterialData
    {
        public ItemData item;   //合成所需的物品
        public int count;       //合成所需的该物品的数量
    }

    public class ItemData
    {
        public int                id;           //物品 ID
        public int                count;        //当前拥有的物品数量
        public int                costGold;     //合成该物品所需的金币
        public List<MaterialData> materialList; //合成该物品所需的材料
    }

    /// <summary>
    /// 计算用 totalGold 金币最多可以合成的 item 装备的数量
    /// </summary>
    /// <param name="item">要合成的装备</param>
    /// <param name="totalGold">拥有的金币</param>
    /// <returns>可合成的 item 装备的最大数量</returns>
    public int Run(ItemData item, int totalGold)
    {
        ownedMaterials = new Dictionary<int, int>();
        var costGold = CalculateAllMaterialsAndGold(item, out var costMaterials);
        
        // 找出供合成次数最少的原材料
        var leastNum = totalGold / costGold;
        foreach (var key in costMaterials.Keys)
        {
            // 如果缺少所需原材料，合成失败
            if (!ownedMaterials.TryGetValue(key, out var material))
            {
                return 0;
            }

            var craftNum = material / costMaterials[key];
            if (craftNum < leastNum)
                leastNum = craftNum;
        }

        return leastNum;
    }

    private Dictionary<int, int> ownedMaterials;

    /// <summary>
    /// 递归计算一次最终合成所需所有基础材料。这种算法的好处在于无论计算多大的数据只用遍历树一次。
    /// </summary>
    /// <param name="item"></param>
    /// <param name="costMaterials"></param>
    /// <returns></returns>
    private int CalculateAllMaterialsAndGold(ItemData item, out Dictionary<int, int> costMaterials)
    {
        if (item.materialList == null || item.materialList.Count == 0)
        {
            // 如果有重复的原料，需要避免冲突
            if (ownedMaterials.ContainsKey(item.id))
                ownedMaterials[item.id] += item.count;
            else
                ownedMaterials.Add(item.id, item.count);
            costMaterials = new Dictionary<int, int>{ { item.id, 1 } };
            return item.costGold;
        }

        var totalGold = item.costGold; // 物品合成费用
        costMaterials = new Dictionary<int, int>();
        for (var i = 0; i < item.materialList.Count; i++)
        {
            var material = item.materialList[i];
            var costGoldPerMaterial = CalculateAllMaterialsAndGold(material.item, out var costMaterialsPerMaterial); 
            totalGold += costGoldPerMaterial * material.count; // 花费金钱 += 子材料所需金钱乘上个数
            costMaterialsPerMaterial = Multiply(costMaterialsPerMaterial, material.count); // 花费材料 = 子材料乘上个数  
            costMaterials = Combine(costMaterials, costMaterialsPerMaterial); // 合并材料消耗
        }
        return totalGold;
    }
    
    private static Dictionary<int, int> Combine(Dictionary<int, int> a, Dictionary<int, int> b)
    {
        if (b == null)
            return a;
        foreach (var key in b.Keys)
        {
            if (a.ContainsKey(key))
            {
                a[key] += b[key];
            }
            else
            {
                a.Add(key, b[key]);
            }
        }
        return a;
    }

    private Dictionary<int, int> Multiply(Dictionary<int, int> a, int count)
    {
        if (count == 0)
            Debug.Log("不应该有0");
        if (a == null)
            return null;

        foreach (var key in a.Keys.ToArray())
        {
            a[key] *= count;
        }

        return a;
    }
}