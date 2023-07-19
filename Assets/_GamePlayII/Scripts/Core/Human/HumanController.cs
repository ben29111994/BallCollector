using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eNameJob
{
    Sword,
    Archer,
    Gient,
    Gun,
    Minigun
}

public enum eNameAnimation
{
    Human_Attack,
    Human_Die,
    Human_Walk
}

public enum eNameTeam
{
    Blue,
    Red
}

public class HumanController : MonoBehaviour
{
    public static HumanController Instance => instance;
    private static HumanController instance;

    public int humanCount;
    private int blueTeamCount;

    [Header("References")]
    public Transform startPoint_blueTeam;
    public Transform startPoint_redTeam;
    public List<Human> listHuman_Blue = new List<Human>();
    public List<Human> listHuman_Red = new List<Human>();

    public List<HumanInfomation> listHumanInfomation = new List<HumanInfomation>();

    public class HumanInfomation
    {
        public eNameJob nameJob;
        public eNameTeam nameTeam;
    }

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        StartCoroutine(C_AutoSpawnHuman());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N)) SpawnHuman((eNameJob)0, eNameTeam.Blue);

    }

    public void SpawnStartRedTeam()
    {
        StartCoroutine(C_SpawnStartRedTeam());
    }

    private IEnumerator C_SpawnStartRedTeam()
    {
        yield return new WaitForSeconds(4.0f);
        SpawnHumanRedTeam(eNameJob.Sword);
    }

    public void SpawnHuman(eNameJob _nameJob, eNameTeam _nameTeam)
    {
        HumanInfomation newHumanInfomation = new HumanInfomation();
        newHumanInfomation.nameJob = _nameJob;
        newHumanInfomation.nameTeam = _nameTeam;
        listHumanInfomation.Add(newHumanInfomation);
    }

    private IEnumerator C_AutoSpawnHuman()
    {
        while (true)
        {
            while(listHumanInfomation.Count > 0)
            {
                HumanInfomation humanInfomation = listHumanInfomation[0];
                eNameJob _nameJob = humanInfomation.nameJob;
                eNameTeam _nameTeam = humanInfomation.nameTeam;

                Human human = HumanPooling.Instance.GetObject(_nameJob);
                Vector3 startPosition = (_nameTeam == eNameTeam.Blue) ? startPoint_blueTeam.position : startPoint_redTeam.position;
                Transform targetCastle = (_nameTeam == eNameTeam.Blue) ? startPoint_redTeam : startPoint_blueTeam;
                human.Active(_nameJob, _nameTeam, startPosition, targetCastle);
                listHuman_Blue.Add(human);
                blueTeamCount++;
                SpawnHumanRedTeam(_nameJob);
                listHumanInfomation.RemoveAt(0);
                yield return new WaitForSeconds(0.1f);
            }


            yield return null;
        }
    }

    private void SpawnHumanRedTeam(eNameJob _nameJob)
    {
        StartCoroutine(C_SpawnHumanRedTeam(_nameJob));
    }

    private IEnumerator C_SpawnHumanRedTeam(eNameJob _nameJob)
    {
        int index = (int)_nameJob;
        float r = Random.value;
        if (r < 0.05f) index += 2;
        else if (r < 0.5f) index += 1;
        if (index >= 5) index = 4;
        _nameJob = (eNameJob)index;

        Human human = HumanPooling.Instance.GetObject(_nameJob);
        listHuman_Red.Add(human);
        eNameTeam _nameTeam = eNameTeam.Red;
        Vector3 startPosition = (_nameTeam == eNameTeam.Blue) ? startPoint_blueTeam.position : startPoint_redTeam.position;
        Transform targetCastle = (_nameTeam == eNameTeam.Blue) ? startPoint_redTeam : startPoint_blueTeam;
        human.Active(_nameJob, _nameTeam, startPosition, targetCastle);
        listHuman_Red.Add(human);

        yield return new WaitForSeconds(0.5f);

        if (blueTeamCount + 1 % 5 == 0 && GamePlayII.Instance.phase_1.IsMaxJob() == false)
        {
            human.Active(_nameJob, _nameTeam, startPosition, targetCastle);
            listHuman_Red.Add(human);
        }
    }

    public void Refresh()
    {
        blueTeamCount = 0;
        listHumanInfomation.Clear();
        listHuman_Blue.Clear();
        listHuman_Red.Clear();
    }

    public bool IsBlueTeamAttackingRedCastle()
    {
        for (int i = 0; i < listHuman_Blue.Count; i++) if (listHuman_Blue[i].isNearCastle) return true;
        return false;
    }

    public bool IsRedTeamAttackingBlueCastle()
    {
        for (int i = 0; i < listHuman_Red.Count; i++) if (listHuman_Red[i].isNearCastle) return true;
        return false;
    }
    public bool IsBlueTeamDieAll()
    {
        return listHuman_Blue.Count == 0 ? true : false;
    }

    public bool IsRedTeamDieAll()
    {
        return listHuman_Red.Count == 0 ? true : false;
    }
}