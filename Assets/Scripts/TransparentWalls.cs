using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class TransparentWalls : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Stack<GameObject> obstructions = new Stack<GameObject>();

    void Start()
    {
        player = GameObject.Find("Player Body");
    }

    // Update is called once per frame
    void Update()
    {
        transparentWalls();
    }

    private void transparentWalls()
    {
        RaycastHit[] hits;
        Vector3 camToPlayer = player.transform.position - transform.position;
        int layerMask = 1 << LayerMask.NameToLayer("Wall");
        hits = Physics.RaycastAll(transform.position, camToPlayer, 200.0f, layerMask);
        if (hits.Length > obstructions.Count)
        {
            for (int i = obstructions.Count; i < hits.Length; i++)
            {
                GameObject obs = hits[i].collider.gameObject;
                ChangeTransparency(obs, 0.3f);
                obstructions.Push(obs);
            }
        }
        else if (hits.Length < obstructions.Count)
        {
            while (obstructions.Count > hits.Length)
            {
                GameObject obs = obstructions.Pop();
                ChangeTransparency(obs, 1.0f);
            }
        }
    }
    private void ChangeTransparency(GameObject obstruction, float alpha)
    {
        Renderer rend = obstruction.GetComponent<Renderer>();
        if (rend == null) return;

        Material mat = rend.material;
        Color c = mat.color;
        mat.color = new Color(c.r, c.g, c.b, alpha);

        // Enable transparency
        mat.SetFloat("_Mode", 3); // Transparent
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }
    
}
