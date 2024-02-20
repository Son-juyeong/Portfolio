using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private MeshFilter[] water;
    [SerializeField]
    private GameObject waterGo;
    //private MeshFilter water;
    public float Amplitude;
    public float waveSpeed;
    private float elapsedTime;
    public static WaveManager Instance;
    private void Awake()
    {
        Instance = this;
        //this.water=waterGo.GetComponent<MeshFilter>();
    }

    private void Update()
    {
        for (int j = 0; j < water.Length; j++)
        {
            this.elapsedTime += Time.deltaTime;
            Vector3[] arrPos = water[j].mesh.vertices;
            for (int i = 0; i < arrPos.Length; i++)
            {
                Vector3 pos = arrPos[i]+this.water[j].gameObject.transform.position;
                arrPos[i].y = CalculateWaveHeight(pos.x, pos.z);
                //Debug.LogFormat("<color=yellow>pos[{0}].x: {1}</color>", i, arrPos[i].x);
                //Debug.LogFormat("<color=cyan>pos[{0}].z: {1}</color>", i, arrPos[i].z);
            }
            water[j].mesh.vertices = arrPos;
        }
    }

    public float GetWaveHeight(Vector3 pos)
    {
        return this.CalculateWaveHeight(pos.x, pos.z)+waterGo.transform.position.y;
    }

    private float CalculateWaveHeight(float x, float z)
    {
        float y1 = Mathf.Sin(x + this.elapsedTime * this.waveSpeed) * this.Amplitude * 0.05f;
        float y2 = Mathf.Sin(z + this.elapsedTime * this.waveSpeed) * this.Amplitude * 0.05f;

        float y3 = Mathf.Sin(x + this.elapsedTime * this.waveSpeed * -0.5f) * this.Amplitude * 0.05f;
        float y4 = Mathf.Sin(z + this.elapsedTime * this.waveSpeed * -0.5f) * this.Amplitude * 0.05f;

        return (y1 + y2 + y3 + y4) / 4;
    }
}
