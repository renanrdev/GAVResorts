# Gav Resorts
## Desafio técnico
#### Projeto ASP NET CORE API WEB usando o conceito MSC – Model Service Controller ou Minimal API para:

1 – Realizar o login a partir de usuário e senha.
  1.1 – Deve haver 2 tipos de usuário: ADM e USR
  1.2 – Não é necessário controle de vida do usuário, como, por exemplo, trocar senhas.

2 – O Login deve ofertar, quando sucesso, uma chave Token JWT que deve ser utilizada nos próximos endpoints.
  2.1 – Insucesso deve retornar código de não autorizado.

3 – O Único serviço é de cadastro de contatos e:
  3.1 – Deve ofertar uma lista de contatos.
  3.2 – Deve trazer detalhes de um único contato.
  3.3 – Deve adicionar novo contato.
  3.4 – Deve remover um contato.
  3.5 – Deve atualizar um contato único.

4 - Os controllers devem oferecer estas opções previstas no serviço.
  4.1 O controle de remover só pode ser executado por usuários ADM.

5 - Os dados devem ser salvos em SQLite.

#### Instruções
- A API foi documentada utilizando Swagger 
- Para rodar o projeto basta iniciar pelo Visual Studio(ou IDE de sua preferência)
- Ao rodar o projeto será aberta uma tela do navegador padrão com a documentação Swagger.
