using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public string PlayerArmorItem;
    public string PlayerWeaponItem;
    public string PlayerOffhandItem;
    public SaveData(PlayerEntity playerEntity) {
        PlayerArmorItem = playerEntity.ArmorItem.GetComponent<Item>().Name;
        PlayerWeaponItem = playerEntity.WeaponItem.GetComponent<Item>().Name;
        PlayerOffhandItem = playerEntity.OffhandItem.GetComponent<Item>().Name;
    }
}