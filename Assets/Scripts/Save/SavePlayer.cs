using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavePlayer : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SaveFile() {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) {
            file = File.OpenWrite(destination);
        } else {
            file = File.Create(destination);
        }

        SaveData data = new SaveData(this.GetComponent<PlayerEntity>());
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }
}