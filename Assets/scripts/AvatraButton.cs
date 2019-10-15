using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatraButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onValueChange(bool isOn)
    {
        if (isOn)
        {   
            if(gameObject.name == "boy" || gameObject.name == "girl")
            {
                AvaterSys._instance.setShowIdx(gameObject.name);
                AvaterSys._instance.sexChange();
                return;
            }
            Debug.Log(gameObject);
            string[] names = gameObject.name.Split('-');
            AvaterSys._instance.OnChange(names[0], names[1]);
            switch (names[0])
            {
                case "pants":
                    AvaterSys._instance.playAnimation("item_boots");
                    break;
                case "shoes":
                    AvaterSys._instance.playAnimation("item_pants");
                    break;
                case "top":
                     AvaterSys._instance.playAnimation("item_shirt");
                    break;
                default:
                     //AvaterSys._instance.playAnimation("walk");
                    break;
            }
        }
    }
    public void loadScences()
    {
        SceneManager.LoadScene("main02");
    }
}
