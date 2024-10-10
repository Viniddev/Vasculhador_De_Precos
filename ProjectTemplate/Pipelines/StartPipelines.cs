﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Pipelines
{
    public class StartPipelines
    {
        public static int numRelatorio { get; set; }

        public StartPipelines() 
        {
            ExpandoObject input = new ExpandoObject();
            Pipelines pipeline = new Pipelines();
            bool execucaoFinalizou = false;
            int contadorErros = 0;

            do
            {
                try 
                {
                    pipeline["IniciarNavegador"].Run(input);
                    pipeline["NavegarAmazon"].Run(input);
                    pipeline["NavegarKabum"].Run(input);
                    pipeline["NavegarMercadoLivre"].Run(input);

                    execucaoFinalizou = true;

                }catch (Exception ex)
                {
                    contadorErros++;
                    string erro = ex.Message.ToString();
                    switch (erro)
                    {
                        case var erroElemento when erroElemento.Contains("nao conectou ao BD"):
                            Console.WriteLine("nao conectou ao BD");
                            break;
                        case var erroElemento when erroElemento.Contains("não baixou o relatorio"):
                            Console.WriteLine("não baixou o relatorio");
                            break;
                        default:
                            Console.WriteLine("ERRO :: " + ex.ToString());
                            Console.ReadLine();
                            break;
                    };
                }
            } while (!execucaoFinalizou && contadorErros < 10);

        }
    }
}