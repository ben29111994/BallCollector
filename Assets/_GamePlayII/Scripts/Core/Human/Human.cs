using FSG.MeshAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStateHuman
{
    Walking,
    Attacking,
    Die,
    Empty
}

public class Human : MonoBehaviour
{
    [Header("Status")]
    public eStateHuman stateHuman;
    public eNameTeam nameTeam;
    public eNameJob nameJob;
    public bool isTakeDamage;
    public bool isNearCastle;
    public bool isKnockBack;
    public int health;
    public int power;
    public float moveSpeed;


    public LayerMask layerBlue = 29;
    public LayerMask layerRed = 30;

    [Header("Materials")]
    public Material[] m_blueTeam;
    public Material[] m_redTeam;

    public Human targetHuman;
    public Human humanAttackMe;
    private MeshAnimatorBase meshAnimator;
    private MeshRenderer meshRenderer;
    private Collider collider;
    private Transform targetCastle;

    private List<Human> listTargetHuman = new List<Human>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameObject a = PoolingII.Instance.GetObject(PoolingII.NameObject.DamageText) as GameObject;
            a.GetComponent<DamageText>().Active(transform.position, 10, nameTeam);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human"))
        {
            TriggerHuman(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Human"))
        {
            TriggerHuman(other);
        }
    }

    private void TriggerHuman(Collider other)
    {
        if (targetHuman != null) return;

        targetHuman = other.GetComponent<Human>();
        listTargetHuman = ListTargetHuman();
        for (int i = 0; i < listTargetHuman.Count; i++)
        {
            if (listTargetHuman[i].humanAttackMe == null)
            {
                targetHuman = listTargetHuman[i];
                break;
            }
        }

        targetHuman.humanAttackMe = this;
      
        Attack();
    }

    private void AutoGetComponent()
    {
        if (collider != null) return;

        meshAnimator = transform.GetChild(0).GetComponent<MeshAnimatorBase>();
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        collider = GetComponent<BoxCollider>();
    }

    public void Active(eNameJob _nameJob, eNameTeam _nameTeam, Vector3 _position, Transform _targetCastle)
    {
        AutoGetComponent();

        // set variable
        stateHuman = eStateHuman.Empty;
        moveSpeed = 2.0f + ((int)_nameJob * 0.2f);
        targetHuman = null;
        targetCastle = _targetCastle;
        isNearCastle = false;
        isKnockBack = false;
        isTakeDamage = false;
        collider.enabled = true;

        SetJob(_nameJob);
        SetTeam(_nameTeam);
        SetTransform(_position);
        SetHealth();
        gameObject.SetActive(true);
        Move();

        if (nameTeam == eNameTeam.Red && HumanController.Instance.listHuman_Red[0] == this)
        {
            moveSpeed = 0.5f;
        }
    }

    private void SetHealth()
    {
        health = 2 + (int)nameJob * 2;
        if (HumanController.Instance.listHuman_Blue.Count - HumanController.Instance.listHuman_Red.Count > 3 !&& GamePlayII.Instance.sureWin) health += 2;
       // if (GamePlayII.Instance.phase_1.IsMaxJob() && nameTeam == eNameTeam.Red) health = (int)(health * 0.6f);
        if(nameTeam == eNameTeam.Red && GamePlayII.Instance.sureWin) health = (int)(health * 0.6f);
        
    }

    private void PlayAnimation(eNameAnimation nameAnimation)
    {
        meshAnimator.Play((int)nameAnimation);
    }

    private void SetJob(eNameJob _nameJob)
    {
        nameJob = _nameJob;
    }

    private void SetTeam(eNameTeam _nameTeam)
    {
        nameTeam = _nameTeam;
        gameObject.layer = nameTeam == eNameTeam.Blue ? 29 : 30;

        Material[] mArray = (int)nameTeam == 0 ? m_blueTeam : m_redTeam;
        meshRenderer.materials = mArray;
    }

    private void SetTransform(Vector3 _position)
    {
        transform.position = _position;
        transform.eulerAngles = (int)nameTeam == 0 ? Vector3.up * 90.0f : Vector3.up * -90.0f;
        float scaleRatio = nameJob == eNameJob.Gient ? 3.0f : 2.0f;
        transform.localScale = Vector3.one * scaleRatio;
    }

    public void Move()
    {
        if (stateHuman == eStateHuman.Walking || isNearCastle || stateHuman == eStateHuman.Die) return;
        StartCoroutine(C_Move());
    }

    private IEnumerator C_Move()
    {
        stateHuman = eStateHuman.Walking;
        yield return null;
        PlayAnimation(eNameAnimation.Human_Walk);

        while (stateHuman == eStateHuman.Walking && !isNearCastle)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            float distanceToCastle = Vector3.Distance(transform.position, targetCastle.position + transform.forward * 3);
            if(distanceToCastle <= 0.1f) isNearCastle = true;
            yield return null;
        }
    }

    public void TakeDamage()
    {
        if (stateHuman == eStateHuman.Die || health < 0 || isTakeDamage) return;

        health--;
        DamageText();
        StartCoroutine(C_DelayTakeDamage());
        StartCoroutine(C_KnockBack());

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator C_DelayTakeDamage()
    {
        isTakeDamage = true;
        yield return new WaitForSeconds(0.2f);
        isTakeDamage = false;
    }

    private void DamageText()
    {
        int damage = (int)nameJob * 2 + Random.Range(2, 5) + Random.Range(2, 5) * GamePlayII.Instance.phase_2.LevelPower;
        GameObject a = PoolingII.Instance.GetObject(PoolingII.NameObject.DamageText) as GameObject;
        a.GetComponent<DamageText>().Active(transform.position, damage, nameTeam);
    }

    public void Attack()
    {
        if (stateHuman == eStateHuman.Attacking || stateHuman == eStateHuman.Die) return;
        StartCoroutine(C_Attack());
    }

    private IEnumerator C_Attack()
    {
        stateHuman = eStateHuman.Attacking;
        float timeDelayAttack = 0.2f;
        float timeHitDamage = 0.5f;
        PlayAnimation(eNameAnimation.Human_Attack);

        while (targetHuman != null && targetHuman.stateHuman != eStateHuman.Die)
        {
            yield return new WaitForSeconds(timeHitDamage);
            targetHuman.TakeDamage();

            yield return new WaitForSeconds(0.1f);

            if (isKnockBack)
            {
                isKnockBack = false;
                break;
            }

            if (stateHuman == eStateHuman.Die)
            {
                targetHuman.humanAttackMe = null;
                targetHuman = null;
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(timeDelayAttack);
                meshAnimator.RestartAnim();
            }
        }

        targetHuman.humanAttackMe = null;
        targetHuman = null;
        Move();
    }

    private IEnumerator C_KnockBack()
    {
        if (targetHuman == null) yield break;

        isKnockBack = true;
        Vector3 pos = transform.position;
        Vector3 tarPos = pos - transform.forward * (0.5f + (int)nameJob * 0.1f);
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime * 5.0f;
            transform.position = Vector3.Lerp(pos, tarPos, t);
            yield return null;
        }
    }

    public void Die()
    {
        if (stateHuman == eStateHuman.Die) return;
        StartCoroutine(C_Die());
    }

    private IEnumerator C_Die()
    {
        stateHuman = eStateHuman.Die;
        collider.enabled = false;
        PlayAnimation(eNameAnimation.Human_Die);
        yield return new WaitForSeconds(1.5f);
        Hide();
    }

    public void Hide()
    {
        if(nameTeam == eNameTeam.Blue) HumanController.Instance.listHuman_Blue.Remove(this);
        else HumanController.Instance.listHuman_Red.Remove(this);
        gameObject.SetActive(false);
    }

    private List<Human> ListTargetHuman()
    {
        Vector3 origin = transform.position + Vector3.up * 1.0f;
        Vector3 direction = transform.forward;
        Ray ray = new Ray(origin,direction);
        int layer = nameTeam == eNameTeam.Blue ? layerRed : layerBlue;
        RaycastHit[] hits = Physics.RaycastAll(ray, 3.0f, layer);
        List<Human> result = new List<Human>();
        foreach (RaycastHit hit in hits) result.Add(hit.collider.gameObject.GetComponent<Human>());
        return result;
    }
}