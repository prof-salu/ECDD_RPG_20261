using UnityEngine;

public class RecompensaInimigo : MonoBehaviour
{
    [Header("Recompensas Base (Se for Nivel 1)")]
    public int xpDrop = 50;
    public int moedasDrop = 20;

    void Start()
    {
        AtributosCombate atributos = GetComponent<AtributosCombate>();

        // MULTIPLICADOR DE NIVEL:
        // Se o designer configurou este monstro como Nivel 2 ou mais, o premio aumenta!
        // Ganha 50% extra de bonus por cada nivel.
        if (atributos != null && atributos.nivel > 1)
        {
            xpDrop += Mathf.RoundToInt(xpDrop * 0.5f * (atributos.nivel - 1));
            moedasDrop += Mathf.RoundToInt(moedasDrop * 0.5f * (atributos.nivel - 1));
        }
    }
}