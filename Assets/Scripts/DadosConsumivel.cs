using UnityEngine;

[CreateAssetMenu(fileName = "Novo Consumivel", menuName = "Sistema RPG/Consumivel")]
public class DadosConsumivel : DadosItem // <--- Herda de DadosItem
{
    [Header("Efeito")]
    public int vidaParaRecuperar;
}