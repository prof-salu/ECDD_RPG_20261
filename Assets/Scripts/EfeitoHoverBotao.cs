using UnityEngine;
using UnityEngine.EventSystems; 
using TMPro;

public class EfeitoHoverBotao : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Referências da UI na Tela")]
    public TextMeshProUGUI feedbackMercador; 
    
    [Header("Itens (Deixar vazio para upgrades)")]
    public DadosItem item;
    
    // Esta função dispara sozinha no milissegundo em que o mouse ENTRA no botão
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.name == "Botao_AfiarEspada")
        {
            feedbackMercador.text = $"Afiar espada: {DadosGlobais.precoBonusAtaque} moedas";
        }else if (gameObject.name == "Botao_ReforcarArmadura")
        {
            feedbackMercador.text = $"Reforçar Armadura: {DadosGlobais.precoBonusDefesa} moedas";
        }else if (gameObject.name == "Botao_ComprarPocao" && item != null)
        {
            feedbackMercador.text = $"Poção de cura: {item.valorEmOuro} moedas";
        }
    }

    // Esta função dispara sozinha no milissegundo em que o mouse SAI do botão
    public void OnPointerExit(PointerEventData eventData)
    {
        feedbackMercador.text = $"Como posso ajudar você?";
    }
}