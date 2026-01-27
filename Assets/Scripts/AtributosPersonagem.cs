using UnityEngine;

public class AtributosPersonagem : MonoBehaviour
{
    [Header("Atributos Base")]
    public int forcaNatural = 10; // Força do personagem "pelado"
    public int defesa = 0; // Defesa do personagem "pelado"

    [Header("Equipamento")]
    // Aqui pedimos especificamente um DadosEquipamento.
    // Se tentarmos arrastar uma Poção aqui, a Unity NÃO VAI DEIXAR. Isso é segurança de código!
    public DadosEquipamento armaEquipada;
    public DadosEquipamento escudoEquipado;

    // Função matemática que calcula o total
    public int CalcularDanoTotal()
    {
        int danoTotal = forcaNatural;

        // Só somamos a arma se tivermos uma equipada (diferente de null)
        if (armaEquipada != null)
        {
            danoTotal += armaEquipada.bonusDeAtaque;
        }

        return danoTotal;
    }

    public int CalcularDefesaTotal()
    {
        int defesaTotal = defesa;

        // Só somamos a arma se tivermos uma equipada (diferente de null)
        if (escudoEquipado != null)
        {
            defesaTotal += escudoEquipado.bonusDeDefesa;
        }

        return defesaTotal;
    }

    // Função para trocar de arma
    public void EquiparArma(DadosEquipamento novaArma)
    {
        armaEquipada = novaArma;
        Debug.Log($"<color=green>EQUIPADO:</color> {novaArma.nomeDoItem} (Raridade: {novaArma.raridade})");
    }

    // Função para trocar de escudo
    public void EquiparEscudo(DadosEquipamento novoEscudo)
    {
        escudoEquipado = novoEscudo;
        Debug.Log($"<color=green>EQUIPADO:</color> {novoEscudo.nomeDoItem} (Raridade: {novoEscudo.raridade})");
    }
}