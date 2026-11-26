using UnityEngine;
using UnityEngine.AI;

public class Mundo : MonoBehaviour
{
    [SerializeField] private Transform[] lugaresDescansar;
    [SerializeField] private Transform[] lugaresComer;
    [SerializeField] private Transform[] lugaresBaño;
    [SerializeField] private Transform[] puntosRandom;




    public Transform GetLugarDescanso(Vector3 posAldeano)
    {
        return buscarCercano(posAldeano, lugaresDescansar);
    }
    public Transform GetLugarComer(Vector3 posAldeano)
    {
        return buscarCercano(posAldeano, lugaresComer);
    }

    public Transform GetLugarBaño(Vector3 posAldeano)
    {
        return buscarCercano(posAldeano, lugaresBaño);
    }

    public Transform GetLugarRandom()
    {
        return puntosRandom[Random.Range(0, puntosRandom.Length)];
    }

    private Transform buscarCercano (Vector3 pos, Transform[] lugares)
    {
        Transform cercano = null;
        float minDist = Mathf.Infinity;
        NavMeshPath camino = new NavMeshPath(); // reutilizar para no gastar tantos recursos

        Vector3 posInicioValida = pos; 
        if (NavMesh.SamplePosition(pos, out NavMeshHit hitInicio, 2.0f, NavMesh.AllAreas))
        {
            posInicioValida = hitInicio.position;
        }

        foreach (Transform lugar in lugares)
        {
            Vector3 posDestinoValida = lugar.position;
            if (NavMesh.SamplePosition(lugar.position, out NavMeshHit hitDestino, 2.0f, NavMesh.AllAreas))
            {
                posDestinoValida = hitDestino.position;
            }

            if (NavMesh.CalculatePath(posInicioValida, posDestinoValida, NavMesh.AllAreas, camino))
            {
                if (camino.status == NavMeshPathStatus.PathComplete) // evita que escoja un destino al que no puede llegar
                {
                    float longCamino = CalcularLongitudCamino(camino);
                    if (longCamino < minDist)
                    {
                        minDist = longCamino;
                        cercano = lugar;
                    }
                }
            }

            if (cercano == null && lugares.Length > 0)
            {
                return lugares[0]; //no se ha encontrado y se pone el primero por defecto
            }

        }
        return cercano; 
    }

    private float CalcularLongitudCamino(NavMeshPath Camino)
    {
        float longi = 0;
        for (int i = 1; i < Camino.corners.Length; i++)
        {
            longi += Vector3.Distance(Camino.corners[i - 1], Camino.corners[i]);
        }
        return longi; 
    }

}
