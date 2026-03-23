using System.Collections;
using UnityEngine;

public class EfeitoCamera : MonoBehaviour
{
    // Outro pequeno Singleton para a Arena!
    public static EfeitoCamera instancia;

    private Vector3 posicaoOriginal;

    void Awake()
    {
        instancia = this;
        posicaoOriginal = transform.localPosition;
    }

    // Chamamos esta função e ela dispara a animação matemática
    public void TremerTela(float duracao, float magnitudeForca)
    {
        StartCoroutine(RotinaTremor(duracao, magnitudeForca));
    }

    IEnumerator RotinaTremor(float duracao, float magnitude)
    {
        float tempoPassado = 0.0f;

        while (tempoPassado < duracao)
        {
            // Gera posições aleatórias rapidamente
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, posicaoOriginal.z);

            tempoPassado += Time.deltaTime;
            yield return null; // Espera o próximo frame
        }

        // Devolve a camera ao centro exato para não ficar torta!
        transform.localPosition = posicaoOriginal;
    }
}