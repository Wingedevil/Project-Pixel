using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadPlayer : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        LoadFile();
    }

    // Update is called once per frame
    void Update() {

    }
    
    public void LoadFile() {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) {
            file = File.OpenRead(destination);
        } else {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        SaveData data = (SaveData)bf.Deserialize(file);
        file.Close();

        PlayerEntity playerEntity = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerEntity>();

        playerEntity.EquipItem(Instantiate(Resources.Load<GameObject>("Prefabs/pickups/wp/" + data.PlayerWeaponItem)), false, false);

        playerEntity.EquipItem(Instantiate(Resources.Load<GameObject>("Prefabs/pickups/oh/" + data.PlayerOffhandItem)), false, false);

        playerEntity.EquipItem(Instantiate(Resources.Load<GameObject>("Prefabs/pickups/eq/" + data.PlayerArmorItem)), false, false);

        //playerEntity.WeaponItem = Instantiate(Resources.Load<GameObject>("Prefabs/pickups/wp/" + data.PlayerWeaponItem), playerEntity.WeaponSlot.transform);
        //playerEntity.WeaponItem.SetActive(false);

        //playerEntity.OffhandItem = Instantiate(Resources.Load<GameObject>("Prefabs/pickups/oh/" + data.PlayerOffhandItem), playerEntity.OffhandSlot.transform);
        //playerEntity.OffhandItem.SetActive(false);

        //playerEntity.ArmorItem = Instantiate(Resources.Load<GameObject>("Prefabs/pickups/eq/" + data.PlayerArmorItem), playerEntity.ArmorSlot.transform);
        //playerEntity.ArmorItem.SetActive(false);

        //playerEntity.InstantiateItems();
    }
}