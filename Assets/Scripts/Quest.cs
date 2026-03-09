using UnityEngine;

public enum TipoQuest { EncontrarNPC, CacarMonstros, ColetarItens }

[CreateAssetMenu(fileName = "NovaQuest", menuName = "Sistema RPG/Missao")]
public class Quest : ScriptableObject
{
    public string nomeQuest;
    public TipoQuest tipoDeMissao;

    [Header("Os Atores da Missão")]
    [Tooltip("Nome do NPC que DÁ esta missão ao jogador")]
    public string nomeNpcEmissor; 
    [Tooltip("Nome do NPC que RECEBE/CONCLUI esta missão")]
    public string nomeNpcDestino; 

    [Header("Textos da Missão")]
    [TextArea] public string falaInicio;    // Quando o NPC dá a quest
    [TextArea] public string falaAndamento; // Quando o jogador ainda não acabou
    [TextArea] public string falaConclusao; // Quando o jogador entrega a quest
    [TextArea] public string descricaoNoHUD; // O texto do Rastreador

    [Header("Objetivos (Se for Caça ou Coleta)")]
    public int quantidadeObjetivo;
    [Tooltip("Ex: Carne de Lobo, Ervas Curativas (Apenas para texto)")]
    public string nomeItemColeta; 

    [Header("Recompensas")]
    public int recompensaOuro;
    public int recompensaXP;

    [Header("A Sequência (Questline)")]
    [Tooltip("A missão que ficará disponível APÓS terminar esta. Deixe vazio se for a última!")]
    public Quest proximaQuestDisponivel; 
}