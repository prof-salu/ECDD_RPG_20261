using UnityEngine;

// Mudamos o menuName para ficar organizado
[CreateAssetMenu(fileName = "Novo Equipamento", menuName = "Sistema RPG/Equipamento")]
public class DadosEquipamento : DadosItem  // <--- HERANÇA! Veja que não usamos : ScriptableObject
{
    [Header("Combate")]
    public int bonusDeAtaque;
    public int bonusDeDefesa;

    // Obs: Ele não precisa declarar 'nomeDoItem' ou 'icone', pois já herdou do Pai!
}

