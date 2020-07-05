using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceManager : MonoBehaviour
{
    public enum PaceMatchScore { PERFECT, GOOD, NORMAL, MISS }
    public GameObject targetPrefab;
    public PaceBullet bulletPrefab;
    public BattleManager battleManager;
    int bulletNo;
    float buff;

    GameObject targetGO;
    PaceBullet[] bulletGOs;

    // play an attack
    // for now, 1/1/11
    public IEnumerator Play()
    {
        buff = 1;
        bulletNo = 4;

        targetGO = Instantiate(targetPrefab, this.transform);
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;

        // 0s: 2s, 1s: 2s, 3s: 2s, 4s: 1.5s
        bulletGOs = new PaceBullet[4];
        Vector3 pos = transform.position + new Vector3(6f, 0, 0);
        bulletGOs[0] = (PaceBullet) Instantiate(bulletPrefab, pos, Quaternion.identity, transform);
        float dis = Vector3.Distance(targetGO.transform.position, bulletGOs[0].transform.position);
        float speed0 = dis / 2f, speed1 = dis / 1.5f;

        bulletGOs[0].SetSpeed(speed0);
        yield return new WaitForSeconds(1f);

        bulletGOs[1] = (PaceBullet)Instantiate(bulletPrefab, pos, Quaternion.identity, transform);
        bulletGOs[1].SetSpeed(speed0);
        yield return new WaitForSeconds(1f);

        bulletGOs[2] = (PaceBullet)Instantiate(bulletPrefab, pos, Quaternion.identity, transform);
        bulletGOs[2].SetSpeed(speed0);
        yield return new WaitForSeconds(1f);

        bulletGOs[3] = (PaceBullet)Instantiate(bulletPrefab, pos, Quaternion.identity, transform);
        bulletGOs[3].SetSpeed(speed1);
    }

    public void DecBullet(PaceMatchScore score)
    {
        bulletNo -= 1;
        buff += ScoreToBuff(score);
        Debug.Log("MATCH RANK: " + score);
        Debug.Log("Dec Bullet: " + bulletNo + " with current buff: " + buff);
        if (bulletNo <= 0)
        {
            StartCoroutine(battleManager.PlayCallBack(buff));
            Destroy(targetGO);
            foreach (PaceBullet bulletGO in bulletGOs)
                Destroy(bulletGO);
        }
    }

    float ScoreToBuff(PaceMatchScore score)
    {
        float buffDelta = 0f;
        switch (score)
        {
            case PaceMatchScore.PERFECT:
                buffDelta = 0.5f;
                break;
            case PaceMatchScore.GOOD:
                buffDelta = 0.2f;
                break;
            case PaceMatchScore.NORMAL:
                buffDelta = 0f;
                break;
            case PaceMatchScore.MISS:
                buffDelta = -0.1f;
                break;
        }
        return buffDelta;
    }

    public static PaceMatchScore Judge(float delta)
    {
        if (delta < 0.1f)
            return PaceMatchScore.PERFECT;
        if (delta < 0.3f)
            return PaceMatchScore.GOOD;
        if (delta < 0.5f)
            return PaceMatchScore.NORMAL;
        return PaceMatchScore.MISS;
    }
}
