using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{

    [SerializeField] private GameObject continueGame;
    [SerializeField] private GameObject levelSelect;

    // Start is called before the first frame update
    void Start()
    {
        //continueGame.gameObject.SetActive(false);
        //sets continue and level select buttons active if game save is available 
        string path = Application.persistentDataPath + "/player.sav";
        if (File.Exists(path))
		{
            continueGame.SetActive(true);
            levelSelect.SetActive(true);
		}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
