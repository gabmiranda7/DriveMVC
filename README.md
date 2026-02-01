# 📂 DriveMVC

![.NET 10](https://img.shields.io/badge/.NET%2010-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)

Aplicação web desenvolvida com **ASP.NET Core MVC** e **.NET 10** para o armazenamento e gerenciamento centralizado de arquivos. Este projeto foca na implementação de operações de I/O (Input/Output) de alta performance, persistência de metadados e organização de documentos com uma interface moderna.

## 🚀 Stack

O projeto utiliza as tecnologias de ponta do ecossistema Microsoft:

* **[.NET 10](https://dotnet.microsoft.com/)** - Plataforma de desenvolvimento de última geração.
* **[ASP.NET Core MVC](https://dotnet.microsoft.com/apps/aspnet/mvc)** - Arquitetura robusta para aplicações web.
* **[Entity Framework Core](https://docs.microsoft.com/ef/core/)** - ORM avançado para manipulação de dados.
* **[SQL Server 2022](https://www.microsoft.com/sql-server)** - Banco de dados relacional.
* **[Bootstrap](https://getbootstrap.com/)** - Framework front-end para interfaces responsivas.
* **[Visual Studio 2026](https://visualstudio.microsoft.com/insiders/)** - Ambiente de desenvolvimento (IDE).

## ⚙️ Funcionalidades

O sistema oferece um controle completo sobre o ciclo de vida dos arquivos:

* **Upload Otimizado**: Envio de arquivos para o servidor com validação de tipos e tamanho.
* **Gerenciamento de Metadados**: Armazenamento de informações como nome, extensão, tamanho e data de envio.
* **Download Seguro**: Recuperação e download direto dos arquivos armazenados.
* **Exclusão Física e Lógica**: Remoção de arquivos do disco e do banco de dados.
* **Visualização Listada**: Interface clara para navegação entre os documentos.

## 📂 Estrutura Arquitetural

O projeto segue os padrões de organização MVC:

* **📂 Controllers**: Gerenciam as requisições HTTP e a lógica de upload/download (`ArquivoController`).
* **📂 Models**: Definem as entidades e regras de negócio dos arquivos.
* **📂 Views**: Telas construídas com Razor Syntax (.cshtml) e Bootstrap.
* **📂 Data**: Contexto do banco de dados e configurações de acesso.
* **📂 wwwroot**: Diretório para armazenamento estático e recursos web.

## 🔧 Configuração e Execução

### Pré-requisitos
* SDK do **.NET 10** instalado.
* Instância do **SQL Server** (Local ou Docker).
* **Visual Studio 2026** (com suporte a arquivos `.slnx`).

### Passo a Passo

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/gabmiranda7/DriveMVC.git
    ```

2.  **Configure o Banco de Dados:**
    Abra o arquivo `appsettings.json` e ajuste a **Connection String** para o seu servidor SQL Server.

3.  **Execute as Migrations:**
    No Visual Studio, abra o **Package Manager Console** e execute os comandos para estruturar o banco:
    ```powershell
    Add-Migration criacaoDB
    Update-Database
    ```

4.  **Inicie a Aplicação:**
    Pressione `F5` no Visual Studio ou execute via terminal:
    ```bash
    dotnet run
    ```

5.  **Acesse o Sistema:**
    A aplicação estará disponível no navegador, geralmente em:
    > `https://localhost:PORT`

## 💡 Detalhes da Implementação (Back-end)

O sistema utiliza uma estratégia de armazenamento **"Database-First"**, onde os arquivos são convertidos para binário e salvos diretamente no banco de dados, garantindo que o backup dos dados inclua também os documentos.

Abaixo, a lógica do método `Upload` no Controller, que processa o arquivo recebido via `IFormFile`:

```csharp
[HttpPost]
public IActionResult Upload(IFormFile arquivo)
{
    if(arquivo != null && arquivo.Length > 0)
    {
        // Cria um fluxo de memória temporário
        using (var ms = new MemoryStream())
        {
            // Copia o arquivo enviado para a memória
            arquivo.CopyTo(ms);

            var arquivoModel = new ArquivoModel
            {
                // Extrai apenas o nome sem extensão
                NomeArquivo = Path.GetFileNameWithoutExtension(arquivo.FileName),
                // Extrai e limpa a extensão (remove o ponto)
                Extensao = Path.GetExtension(arquivo.FileName).TrimStart('.'),
                // Captura o tipo MIME (ex: application/pdf)
                TipoMime = arquivo.ContentType,
                DataUpload = DateTime.Now,
                // Converte o stream para array de bytes (BLOB) para salvar no banco
                ArquivoBytes = ms.ToArray()
            };

            _context.Arquivos.Add(arquivoModel);
            _context.SaveChanges();
        }
    }

    return RedirectToAction("Index");
}
