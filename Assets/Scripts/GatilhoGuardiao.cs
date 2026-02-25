using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GatilhoGuardiao : MonoBehaviour
{
    [Header("Identificação")]
    [Tooltip("Dê um nome único. Ex: Guardiao_Ponte_01")]
    public string idUnico;
    public List<GameObject> inimigosDesteGrupo;

    void Start()
    {
        // Verifica a lista de mortos!
        if (DadosGlobais.inimigosDerrotados.Contains(idUnico))
        {
            gameObject.SetActive(false); // Desaparece se já foi derrotado!
        }
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Player"))
        {
            IniciadorBatalha iniciador = GetComponent<IniciadorBatalha>();
            if (iniciador != null)
                iniciador.DispararBatalha(colision.gameObject, idUnico, inimigosDesteGrupo);
        }
    }
}