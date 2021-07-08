 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class DeleteSave : MonoBehaviour
{
    [SerializeField] private GameObject NoSavePresent;
    [SerializeField] private GameObject ok;
    [SerializeField] private GameObject areYouSure;
    [SerializeField] private GameObject deleted;
    public void delete() { 

        string path = Application.persistentDataPath + "/player.sav";

        if (File.Exists(path))
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            directory.Delete(true);
            Directory.CreateDirectory(path);

            areYouSure.SetActive(false);
            deleted.SetActive(true);
            ok.SetActive(true);
        }
		else
		{
            NoSavePresent.SetActive(true);
            ok.SetActive(true);
            areYouSure.SetActive(false);
        }

    }

}
