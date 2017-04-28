using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 思路：遍历对象的所有子对象的SkinnedMeshRenderer
/// 然后把所有的动态对象组合成一个大的对象
/// </summary>
public class CombineMesh : MonoBehaviour
{
    void Start()
    {
        CombineToMesh(this.gameObject);
    }

    public static void CombineToMesh(GameObject _go)
    {
        SkinnedMeshRenderer[] _smr = _go.GetComponentsInChildren<SkinnedMeshRenderer>();

        List<CombineInstance> _combineInst = new List<CombineInstance>();

        List<Material> _mat = new List<Material>();

        List<Transform> _transf = new List<Transform>();

        for (int i = 0; i < _smr.Length; i++)
        {
            _mat.AddRange(_smr[i].materials);
            _transf.AddRange(_smr[i].bones);

            for (int sub = 0; sub < _smr[i].sharedMesh.subMeshCount; sub++)
            {
                CombineInstance _ci = new CombineInstance();
                _ci.mesh = _smr[i].sharedMesh;
                _ci.subMeshIndex = sub;
                _combineInst.Add(_ci);        
            }
            Destroy(_smr[i].gameObject);
        }

        SkinnedMeshRenderer _r = _go.GetComponent<SkinnedMeshRenderer>();
        if (_r == null)
        {
            _r = _go.AddComponent<SkinnedMeshRenderer>();
        }
        _r.sharedMesh = new Mesh();
        _r.bones = _transf.ToArray();
        _r.materials = new Material[] { _mat[1]};//选择网格合并后，要用的材质
        _r.rootBone = _go.transform;

        _r.sharedMesh.CombineMeshes(_combineInst.ToArray(),true,false);

    }
}
