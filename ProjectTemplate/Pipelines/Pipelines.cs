using PipeliningLibrary;
using ProjectTemplate.Pipes.Excel;
using ProjectTemplate.Pipes.Navegador;
using ProjectTemplate.Pipes.Sydle;
using ProjectTemplate.Pipes.Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Pipelines
{
    internal class Pipelines : PipelineGroup
    {
        public Pipelines() 
        {
            Pipeline("IniciarNavegador")
                .Pipe<IniciarNavegador>()
                ;

            Pipeline("NavegarAmazon")
                .Pipe<NavegarAmazon>()
                ;     
            
            Pipeline("NavegarKabum")
                .Pipe<NavegarKabum>()
                ;

            Pipeline("NavegadorMercadoLivre")
                .Pipe<NavegadorMercadoLivre>()
                ;
            
            Pipeline("NavegarMagazineLuiza")
                .Pipe<NavegarMagazineLuiza>()
                ;
            
            Pipeline("BuildReport")
                .Pipe<BuildReport>()
                ;
        }
    }
}
