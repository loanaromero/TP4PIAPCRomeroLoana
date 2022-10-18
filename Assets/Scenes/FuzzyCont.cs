using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyCont : MonoBehaviour
{
    public float gravedad = 9.81f;
    public float velocidad_y;
    public float posicion_y;
    public float velocidad_ventilador;
    [HideInInspector]
    public float altura_objetivo = 80;

    public Transform obj;

    void Start()
    {
        obj = gameObject.transform;
        obj.position += new Vector3(0, Random.Range(-100.0f, 100.0f), 0);
        posicion_y = obj.position.y;
    }

    void FixedUpdate()
    {
        fuzzy_logic();
        float caos = Random.Range(-5.0f, 5.0f);
        velocidad_y += (gravedad - velocidad_ventilador + caos) * 0.01f;
        posicion_y += velocidad_y;
        obj.position += new Vector3(0, velocidad_y, 0);
    }


    public float m_grado(float x, float y, float z)
    {
        float grado_pert = 0;
        if (x <= y)
        {
            grado_pert = 0;
        }
        else
        {
            if (x > y && x < z)
            {
                grado_pert = (x / (z - y)) - (y / (z - y));
            }
            else
            {
                if (x >= z)
                {
                    grado_pert = 1;
                }
            }
        }
        return grado_pert;
    }

    public float m_grado_invertido(float x, float y, float z)
    {
        float grado_pert = 0;
        if (x <= y)
        {
            grado_pert = 1;
        }
        else
        {
            if (x > y && x < z)
            {
                grado_pert = (x / (z - y)) - (z / (z - y));
            }
            else
            {
                if (x >= z)
                {
                    grado_pert = 0;
                }
            }
        }
        return grado_pert;
    }

    public float m_triangulo(float x, float a, float b, float c)
    {
        float grado_pert = 0;
        if (x <= a)
        {
            grado_pert = 0;
        }
        else
        {
            if (x > a && x <= b)
            {
                grado_pert = (x / (b - a)) - (a / (b - a));
            }
            else
            {
                if (x > b && x <= c)
                {
                    grado_pert = -(x / (c - b)) + (c / (c - b));
                }
                else
                {
                    if (x > c)
                    {
                        grado_pert = 0;
                    }
                }
            }
        }
        return grado_pert;
    }

    public float m_trapezoide(float x, float a, float b, float c, float d)
    {
        float grado_pert = 0;
        if (x <= a)
        {
            grado_pert = 0;
        }
        else
        {
            if (x > a && x <= b)
            {
                grado_pert = (x / (b - a)) - (a / (b - a));
            }
            else
            {
                if (x > b && x <= c)
                {
                    grado_pert = 1;
                }
                else
                {
                    if (x > c && x <= d)
                    {
                        grado_pert = -(x / (d - c)) + (d / (d - c));
                    }
                    else
                    {
                        if (x > d)
                        {
                            grado_pert = 0;
                        }
                    }
                }
            }
        }
        return grado_pert;
    }

    public void fuzzy_logic()
    {
        float distancia = altura_objetivo - posicion_y;

        float centrado = m_triangulo(distancia, -40, 0, 40);

        float cercaA = m_trapezoide(distancia, 20, 80, 120, 180);
        float normalA = m_trapezoide(distancia, 120, 160, 240, 280);
        float lejosA = m_grado(distancia, 240, 300);

        float cercaB = m_trapezoide(distancia, -180, -120, -80, -20);
        float normalB = m_trapezoide(distancia, -280, -240, -160, -120);
        float lejosB = m_grado_invertido(distancia, -300, -240);

        float numerador = centrado * 9.8f + cercaA * 4 + normalA * 2 + lejosA * 1 + cercaB * 14 + normalB * 15.5f + lejosB * 18;
        float denominador = centrado + cercaA + normalA + lejosA + cercaB + normalB + lejosB;

        velocidad_ventilador = numerador / denominador;
    }

}
