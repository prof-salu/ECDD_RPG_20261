using UnityEngine;

public class ProgressoJogador : MonoBehaviour
{
    public int xpAtual = 0;

    [Header("Tabela de XP (A Coleção)")]
    [Tooltip("Índice 0 = Para chegar no Nível 2. Índice 1 = Para chegar no Nível 3...")]
    // Preencha os números que o seu grupo desenhou na atividade!
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
        Debug.Log("Você ganhou " + quantidade + " de XP! Total: " + xpAtual);

        // Se o Herói ainda não chegou ao nível máximo (Ex: Nível 5)
        if (meusAtributos.nivel < xpNecessariaPorNivel.Length + 1)
        {
            // Lê o Array: Se estou no nível 1, olho para a gaveta [0] do Array.
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

        // Recalcula os bónus de atributos do AtributosCombate e CURA O HERÓI!
        meusAtributos.CalcularStatus();
        meusAtributos.hpAtual = meusAtributos.hpMaximo;
        meusAtributos.AtualizarBarra();

        Debug.Log("LEVEL UP! O Herói alcançou o Nível " + meusAtributos.nivel + "!");

        // --- MÁGICA VISUAL UNIVERSAL ---
        if (efeitoLevelUp != null)
        {
            efeitoLevelUp.SetActive(true);
            Invoke("EsconderLevelUp", 3f); // Apaga sozinho após 3 segundos
        }

        // Verifica se subiu dois níveis de uma vez (casos raros de muita XP)
        GanharXP(0);
    }

    void EsconderLevelUp()
    {
        if (efeitoLevelUp != null) efeitoLevelUp.SetActive(false);
    }
}