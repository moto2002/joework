using UnityEngine;
using System.Collections;
using System.Threading;

public class WaveAnim : MonoBehaviour
{
    public int waveWidth = 128;
    public int waveHeight = 128;

    float[,] waveA;
    float[,] waveB;

    bool isRun = true;
    float sleepTime;
    Color[] colorBuffer;

    Texture2D tex_uv;

    void Start()
    {
        waveA = new float[waveWidth, waveHeight];
        waveB = new float[waveWidth, waveHeight];

        tex_uv = new Texture2D(waveWidth, waveHeight);
        colorBuffer = new Color[waveWidth * waveHeight];

        GetComponent<Renderer>().material.SetTexture("_WaveTex", tex_uv);

        Thread th = new Thread(new ThreadStart(ComputeWave));
        th.Start();
    }

    void Update()
    {
        sleepTime = Time.deltaTime * 1000;
        tex_uv.SetPixels(colorBuffer);
        tex_uv.Apply();

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 pos = hit.point;
                //获取点击模型的本地坐标
                pos = transform.worldToLocalMatrix.MultiplyPoint(pos);

                //将本地坐标转成UV坐标
                int w = (int)((pos.x + 0.5) * waveWidth);
                int h = (int)((pos.y + 0.5) * waveHeight);

                PutPop(w, h);
            }
        }
        //ComputeWave();    
    }

    //放置起波点
    private void PutPop(int x, int y)
    {
        int radius = 20;
        float dist;

        for (int i = -radius; i < radius; i++)
        {
            for (int j = -radius; j < radius; j++)
            {
                //控制在uv纹理范围内
                if ((x + i >= 0) && (x + i < waveWidth - 1) && (y + j >= 0) && (y + j < waveHeight - 1))
                {
                    dist = Mathf.Sqrt(i * i + j * j);
                    if (dist < radius)
                    {
                        waveA[x + i, y + j] = Mathf.Cos(dist * Mathf.PI / radius);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 波纹的UV计算
    /// </summary>
    private void ComputeWave()
    {
        while (isRun)
        {
            for (int w = 1; w < waveWidth - 1; w++)
            {
                for (int h = 1; h < waveHeight - 1; h++)
                {
                    waveB[w, h] = (waveA[w - 1, h] + waveA[w + 1, h]
                        + waveA[w, h + 1] + waveA[w, h - 1]
                        + waveA[w - 1, h + 1] + waveA[w - 1, h - 1]
                        + waveA[w + 1, h + 1] + waveA[w + 1, h - 1]) / 4 - waveB[w, h];

                    waveB[w, h] = waveB[w, h] > 1 ? 1 : waveB[w, h];
                    waveB[w, h] = waveB[w, h] < -1 ? -1 : waveB[w, h];

                    //控制范围[-1,1]
                    float offset_u = (waveB[w - 1, h] - waveB[w + 1, h]) / 2;
                    float offset_v = (waveB[w, h - 1] - waveB[w, h + 1]) / 2;

                    float r = offset_v / 2 + 0.5f;
                    float g = offset_u / 2 + 0.5f;

                    //tex_uv.SetPixel(w, h, new Color(r, g, 0));
                    colorBuffer[w + waveWidth * h] = new Color(r, g, 0);

                    //进行波纹能量衰减
                    waveB[w, h] -= waveB[w, h] * 0.025f;
                }
            }

            //tex_uv.Apply();

            float[,] temp = waveA;
            waveA = waveB;
            waveB = temp;

            Thread.Sleep((int)sleepTime);
        }
    }

    void OnDestory()
    {
        isRun = false;
    }
}