# InfoPC

Programa em C# que exibe informações detalhadas do computador:
- Nome do computador
- Usuário
- Sistema operacional
- Arquitetura
- Diretório do sistema
- Quantidade de processadores
- Machine GUID
- Serial do HD
- Chave do Windows
- Chave do Office (se disponível)

## Como compilar

1. Instale o .NET 6 SDK.
2. Abra o terminal na pasta `InfoPC`.
3. Execute:
   ```powershell
   dotnet restore
   dotnet build -c Release
   ```
4. O executável estará em `bin\Release\net6.0\InfoPC.exe`.

## Observações
- Para ler chaves do Windows/Office, execute como administrador.
- Algumas informações podem não ser encontradas em versões recentes do Windows/Office por segurança.
