using UnityEngine;

public class ProgressoJogador : MonoBehaviour
{
    public int xpAtual = 0;

    [Header("Tabela de XP (A Colecao)")]
    [Tooltip("Indice 0 = Para chegar no Nivel 2. Indice 1 = Para chegar no Nivel 3...")]
    // Preencha os numeros que o seu grupo desenhou na atividade!
    public int[] xpNecessariaPorNivel = new int[] { 100, 250, 500, 1000, 5000 };

    private AtributosCombate meusAtributos;

    [Tooltip("Texto LEVEL UP")]
    public GameObject efeitoLevelUp;

    void Start()
    {
        meusAtributos = GetComponent<AtributosCombate>();

        IniciadorBatalha.CarregarDadosJogador(gameObject);

        // Esconde o texto de Level Up visual ao nascer
        if (efeitoLevelUp != null) efeitoLevelUp.SetActive(false);
    }

    public void GanharXP(int quantidade)
    {
        xpAtual += quantidade;
        Debug.Log("Voce ganhou " + quantidade + " de XP! Total: " + xpAtual);

        // Se o Heroi ainda nao chegou ao nivel maximo (Ex: Nivel 5)
        if (meusAtributos.nivel < xpNecessariaPorNivel.Length + 1)
        {
            // Le o Array: Se estou no nivel 1, olho para a gaveta [0] do Array.
            int metaDeXP = xpNecessariaPorNivel[meusAtributos.nivel - 1];

            if (metaDeXP > 0 && xpAtual >= metaDeXP)
            {
                LevelUp(metaDeXP);
            }
        }
    }

    void LevelUp(int xpGasta)
    {
        meusAtributos.nivel++;
        xpAtual -= xpGasta; // Paga a XP usada e guarda a que sobrou

        // Recalcula os bonus de atributos do AtributosCombate e CURA O HEROI!
        meusAtributos.CalcularStatus();
        meusAtributos.hpAtual = meusAtributos.hpMaximo;
        meusAtributos.AtualizarBarra();

        Debug.Log("LEVEL UP! O Heroi alcanaou o Nivel " + meusAtributos.nivel + "!");

        // --- MAGICA VISUAL UNIVERSAL ---
        if (efeitoLevelUp != null)
        {
            efeitoLevelUp.SetActive(true);
            Invoke("EsconderLevelUp", 3f); // Apaga sozinho apos 3 segundos
        }

        // Verifica se subiu dois niveis de uma vez (casos raros de muita XP)
        GanharXP(0);
    }

    void EsconderLevelUp()
    {
        if (efeitoLevelUp != null) efeitoLevelUp.SetActive(false);
    }
}