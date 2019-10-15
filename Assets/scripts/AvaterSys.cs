using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvaterSys : MonoBehaviour {

    public static AvaterSys _instance;
    public GameObject girlPanle;
    public GameObject boyPanle;
    //girl
    private GameObject girlSource;
    private Transform girlSourceTrans;
    private GameObject girlTarget;//骨架物体，换装的人
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> girlData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
    //小女孩所有的资源信息<部位，编号，SkinnedMeshRenderer>
    private Transform[] girlHips;//小女孩的骨骼信息
    private Dictionary<string, SkinnedMeshRenderer> girlSmr = new Dictionary<string, SkinnedMeshRenderer>();//Target身上的skm的信息；
    private string[,] girlStr = new string[,] { { "eyes", "1" }, { "hair", "1" }, { "top", "1" }, { "pants", "1" }, { "shoes", "1" }, { "face", "1" } };

    //bor
    private GameObject boySource;
    private Transform boySourceTrans;
    private GameObject boyTarget;//骨架物体，换装的人
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> boyData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
    private Transform[] boyHips;
    private Dictionary<string, SkinnedMeshRenderer> boySmr = new Dictionary<string, SkinnedMeshRenderer>();//Target身上的skm的信息；
    private string[,] boyStr = new string[,] { { "eyes", "1" }, { "hair", "1" }, { "top", "1" }, { "pants", "1" }, { "shoes", "1" }, { "face", "1" } };

    public int showIdx = 0;//显示的是小女孩还是小男孩

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);//不删除游戏物体
    }
    // Use this for initialization
    void Start () {
        InstantiateSource();
        InstantiateTarget();
        saveData();
        boyTarget.AddComponent<SpinWithMouse>();
        girlTarget.AddComponent<SpinWithMouse>();
        InitAvator(showIdx);

        //boyTarget.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    int num = Random.Range(1, 7);
        //    changeMeshForGirl("top", num.ToString());
        //}
	}

    void InstantiateSource()
    {
        //girl
        girlSource = Instantiate(Resources.Load("FemaleModel")) as GameObject;//加载资源物体
        girlSourceTrans = girlSource.transform;
        girlSource.transform.position = Vector3.zero;
        girlSource.SetActive(false);
        //boy
        boySource = Instantiate(Resources.Load("MaleModel")) as GameObject;//加载资源物体
        boySourceTrans = boySource.transform;
        boySource.transform.position = Vector3.zero;
        boySource.SetActive(false);
        
    }

    void InstantiateTarget()
    {
        //girl
        girlTarget = Instantiate(Resources.Load("FemaleTarget")) as GameObject;
        girlTarget.transform.position = Vector3.zero;
        girlHips = girlTarget.GetComponentsInChildren<Transform>();
        //boy
        boyTarget = Instantiate(Resources.Load("MaleTarget")) as GameObject;
        boyTarget.transform.position = Vector3.zero;
        boyHips = boyTarget.GetComponentsInChildren<Transform>();
    }

    public void  AvatorGirlAndBoy()
    {
        InstantiateSource();
        InstantiateTarget();
        saveData();
        InitAvator(showIdx);
    }

    void saveData()
    {   
        if(girlSourceTrans == null || boySourceTrans == null)
        {
            return;
        }
        girlData.Clear();
        girlSmr.Clear();
        //girl
        SkinnedMeshRenderer[] girl_parts = girlSourceTrans.GetComponentsInChildren<SkinnedMeshRenderer>();//遍历所有子物体有SkinnedMeshRenderer，进行存储
        SkinnedMeshRenderer[] boy_parts = boySourceTrans.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var part in girl_parts)
        {
            string[] names = part.name.Split('-');
            if (!girlData.ContainsKey(names[0]))
            {   
                //生成对应的部位且只生成一个
                GameObject partGirl = new GameObject();
                partGirl.name = names[0];
                partGirl.transform.parent = girlTarget.transform;//为Target生成各部位的子对象
                //把骨骼Target身上的skm信息存储
                girlSmr.Add(names[0], partGirl.AddComponent<SkinnedMeshRenderer>());
                girlData.Add(names[0], new Dictionary<string, SkinnedMeshRenderer>());
            }
            girlData[names[0]].Add(names[1],part);//存储所有的Skm到数据里边
        }
        boyData.Clear();
        boySmr.Clear();
        //boy
        foreach (var part in boy_parts)
        {
            string[] names = part.name.Split('-');
            if (!boyData.ContainsKey(names[0]))
            {
                //生成对应的部位且只生成一个
                GameObject partBoy = new GameObject();
                partBoy.name = names[0];
                partBoy.transform.parent = boyTarget.transform;//为Target生成各部位的子对象
                //把骨骼Target身上的skm信息存储
                boySmr.Add(names[0], partBoy.AddComponent<SkinnedMeshRenderer>());
                boyData.Add(names[0], new Dictionary<string, SkinnedMeshRenderer>());
            }
            boyData[names[0]].Add(names[1], part);//存储所有的Skm到数据里边
        }
    }
    //girl
    void changeMeshForGirl(string part,string idx)//传入部位，编号，从girlData里边读取对应的skm
    {
        SkinnedMeshRenderer skm = girlData[part][idx];//要更换的部位
        List<Transform> bones = new List<Transform>();
        foreach(var trans in skm.bones)
        {
            foreach(var bone in girlHips)
            {
                if(bone.name == trans.name)
                {
                    bones.Add(bone);
                    //continue;
                    break;
                }
            }
        }
        //换装实现
        girlSmr[part].bones = bones.ToArray();
        girlSmr[part].materials = skm.materials;
        girlSmr[part].sharedMesh = skm.sharedMesh;
        saveCloth(part, idx, girlStr);
    }
    //boy
    void changeMeshForBoy(string part, string idx)//传入部位，编号，从girlData里边读取对应的skm
    {
        SkinnedMeshRenderer skm = boyData[part][idx];//要更换的部位
        List<Transform> bones = new List<Transform>();
        foreach (var trans in skm.bones)
        {
            foreach (var bone in boyHips)
            {
                if (bone.name == trans.name)
                {
                    bones.Add(bone);
                    //continue;
                    break;
                }
            }
        }
        //换装实现
        boySmr[part].bones = bones.ToArray();
        boySmr[part].materials = skm.materials;
        boySmr[part].sharedMesh = skm.sharedMesh;
        saveCloth(part, idx, boyStr);
    }

    void InitAvator(int Idx)//初始化Target
    {
        int lengh = girlStr.GetLength(0);
        //girl
        for(int i = 0; i < lengh; i++)
        {
            changeMeshForGirl(girlStr[i, 0], girlStr[i, 1]);//穿上衣服
        }
        //boy
        for (int i = 0; i < lengh; i++)
        {
            changeMeshForBoy(boyStr[i, 0], girlStr[i, 1]);//穿上衣服
        }
        if(Idx == 0)
        {
            girlTarget.SetActive(true);
            boyTarget.SetActive(false);
        }
        else
        {
            girlTarget.SetActive(false);
            boyTarget.SetActive(true);
        }
    }

    public void OnChange(string part, string num)
    {
        if(showIdx == 0)
        {
            changeMeshForGirl(part, num);
        }
        else
        {
            changeMeshForBoy(part, num);
        }
    }
    public  void sexChange() {
        if(showIdx == 1)
        {
            boyTarget.SetActive(true);
            boyPanle.SetActive(true);
            girlTarget.SetActive(false);
            girlPanle.SetActive(false);
        }
        else
        {
            boyTarget.SetActive(false);
            boyPanle.SetActive(false);
            girlTarget.SetActive(true);
            girlPanle.SetActive(true);
        }
    }
    public void setShowIdx(string sex)
    {
        if(sex == "boy")
        {
            showIdx = 1;
        }
        else
        {
            showIdx = 0;
        }
    }
    public void playAnimation(string anim)
    {
        Animation player = GameObject.FindWithTag("Player").GetComponent<Animation>();
        if (!player.IsPlaying(anim))
        {
            player.Play(anim);
            player.PlayQueued("idle1");
        }
    }
    void saveCloth(string part,string num,string[,] str)//保存当前衣服
    {
        int lenth = girlStr.GetLength(0);
        for(int i = 0; i < lenth; i++)
        {
            if(str[i,0] == part)
            {
                str[i, 1] = num;
            }
        }
    }
}
