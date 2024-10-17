# Vasculhador de Precos

  Esse é um projeto desenvolvido com o intuito de obter e gerar uma comparação entre o preço de
  um produto em diferentes sites.

  Essa aplicação utiliza Selenium WebDriver, ClosedXml e Pipelining Architecture como dependências
  de projeto.

## Observações

  1) Todo o fluxo da automação é gerenciado pelos Pipes e é iniciado dentro de StartPipelines;
  2) Toda classe que deve se comportar como pipeline deve implementar a interface IPipe;
  3) Todo pipeline deve ser nomeado dentro do arquivo Pipelines;
  4) A passagem de parâmetros entre os pipes é realizado por meio do dynamic object input;
  5) Você pode resgatar qualquer valor inserido dentro do input em qualquer etapa dos pipelines;
  6) O gerenciamento e tratativa de exceções ocorre no StartPipelines;
  7) É possivel criar classes que não implementem IPipe;

