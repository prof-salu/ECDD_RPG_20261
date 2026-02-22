using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GatilhoGuardiao : MonoBehaviour
{
    [Header("Formação Inimiga")]
    [Tooltip("Arraste os PREFABS dos inimigos da pasta do projeto para cá")]
    public List<GameObject> inimigosDesteGrupo;

    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Player"))
        {
            DadosGlobais.prefabsInimigos = new List<GameObject>(inimigosDesteGrupo);
            SceneManager.LoadScene("CenaBatalha");
        }
    }
}