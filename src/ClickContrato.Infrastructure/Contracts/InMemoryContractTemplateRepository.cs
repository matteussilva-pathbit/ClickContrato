namespace ClickContrato.Infrastructure.Contracts;

public sealed class InMemoryContractTemplateRepository : IContractTemplateRepository
{
    private static readonly IReadOnlyList<ContractTemplate> Templates = Seed();

    public Task<IReadOnlyList<ContractTemplate>> ListAsync(CancellationToken ct) =>
        Task.FromResult(Templates);

    public Task<ContractTemplate?> FindByCodeAsync(string code, int? version, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(code)) return Task.FromResult<ContractTemplate?>(null);
        var normalized = code.Trim().ToLowerInvariant();

        var matches = Templates.Where(t => t.Code == normalized);
        var template = version is null
            ? matches.OrderByDescending(t => t.Version).FirstOrDefault()
            : matches.FirstOrDefault(t => t.Version == version.Value);

        return Task.FromResult(template);
    }

    public Task<ContractTemplate?> FindByIdAsync(Guid id, int? version, CancellationToken ct)
    {
        var matches = Templates.Where(t => t.Id == id);
        var template = version is null
            ? matches.OrderByDescending(t => t.Version).FirstOrDefault()
            : matches.FirstOrDefault(t => t.Version == version.Value);

        return Task.FromResult(template);
    }

    private static IReadOnlyList<ContractTemplate> Seed()
    {
        var prestacao = new ContractTemplate(
            id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            code: "prestacao-servicos",
            name: "Prestação de Serviços",
            version: 1,
            body: """
                  CONTRATO DE PRESTAÇÃO DE SERVIÇOS
                  
                  Pelo presente instrumento, as partes:
                  
                  CONTRATANTE: {{ContratanteNome}}, CPF/CNPJ {{ContratanteDocumento}}
                  CONTRATADO: {{ContratadoNome}}, CPF/CNPJ {{ContratadoDocumento}}
                  
                  resolvem celebrar o presente contrato, na data de {{DataHoje}}, mediante as cláusulas seguintes:
                  
                  1. OBJETO: {{Objeto}}
                  2. VALOR: R$ {{Valor}}
                  3. PRAZO: {{Prazo}}
                  
                  E por estarem de acordo, assinam.
                  """,
            fields: new List<ContractFieldDefinition>
            {
                new("ContratanteNome", "Nome do Contratante", FieldType.Text, true, "Ex.: João da Silva", 120),
                new("ContratanteDocumento", "CPF/CNPJ do Contratante", FieldType.CpfCnpj, true, "Ex.: 000.000.000-00", 20),
                new("ContratadoNome", "Nome do Contratado", FieldType.Text, true, "Ex.: Maria Souza", 120),
                new("ContratadoDocumento", "CPF/CNPJ do Contratado", FieldType.CpfCnpj, true, "Ex.: 00.000.000/0001-00", 20),
                new("Objeto", "Objeto do Serviço", FieldType.Text, true, "Descreva o serviço", 500),
                new("Valor", "Valor (R$)", FieldType.Money, true, "Ex.: 1500,00", 30),
                new("Prazo", "Prazo", FieldType.Text, true, "Ex.: 30 dias", 60)
            }
        );

        var prestacaoV2 = new ContractTemplate(
            id: Guid.Parse("66666666-6666-6666-6666-666666666666"),
            code: "prestacao-servicos",
            name: "Prestação de Serviços (Completo)",
            version: 2,
            body: """
                  CONTRATO DE PRESTAÇÃO DE SERVIÇOS
                  
                  CONTRATANTE: {{ContratanteNome}}, CPF/CNPJ {{ContratanteDocumento}}, Email {{ContratanteEmail}}
                  CONTRATADO: {{ContratadoNome}}, CPF/CNPJ {{ContratadoDocumento}}, Email {{ContratadoEmail}}
                  
                  Data: {{DataHoje}}
                  
                  1. OBJETO
                  {{Objeto}}
                  
                  2. PRAZO
                  O prazo para execução/entrega é: {{Prazo}}.
                  
                  3. VALOR E FORMA DE PAGAMENTO
                  Valor total: R$ {{Valor}}.
                  Forma de pagamento: {{FormaPagamento}}.
                  
                  4. CONFIDENCIALIDADE
                  As partes se comprometem a manter sigilo sobre informações confidenciais por {{PrazoSigilo}}.
                  
                  5. RESCISÃO
                  O contrato poderá ser rescindido mediante aviso prévio de {{AvisoPrevio}}.
                  
                  6. MULTA
                  Em caso de descumprimento, aplica-se multa de {{MultaPercentual}}% sobre o valor do contrato.
                  
                  7. FORO
                  Fica eleito o foro da comarca de {{ForoCidadeUF}}.
                  
                  E por estarem de acordo, assinam.
                  """,
            fields: new List<ContractFieldDefinition>
            {
                new("ContratanteNome", "Nome do Contratante", FieldType.Text, true, "Ex.: João da Silva", 120),
                new("ContratanteDocumento", "CPF/CNPJ do Contratante", FieldType.CpfCnpj, true, "Ex.: 000.000.000-00", 20),
                new("ContratanteEmail", "Email do Contratante", FieldType.Email, true, "Ex.: joao@exemplo.com", 120),
                new("ContratadoNome", "Nome do Contratado", FieldType.Text, true, "Ex.: Maria Souza", 120),
                new("ContratadoDocumento", "CPF/CNPJ do Contratado", FieldType.CpfCnpj, true, "Ex.: 00.000.000/0001-00", 20),
                new("ContratadoEmail", "Email do Contratado", FieldType.Email, true, "Ex.: maria@exemplo.com", 120),
                new("Objeto", "Objeto do Serviço", FieldType.Text, true, "Descreva o serviço (escopo)", 900),
                new("Prazo", "Prazo", FieldType.Text, true, "Ex.: 30 dias / até 10/02/2026", 80),
                new("Valor", "Valor (R$)", FieldType.Money, true, "Ex.: 1500,00", 30),
                new("FormaPagamento", "Forma de pagamento", FieldType.Text, true, "Ex.: 50% entrada + 50% na entrega / PIX à vista", 120),
                new("PrazoSigilo", "Prazo de sigilo", FieldType.Text, true, "Ex.: 2 anos", 40),
                new("AvisoPrevio", "Aviso prévio", FieldType.Text, true, "Ex.: 7 dias", 40),
                new("MultaPercentual", "Multa (%)", FieldType.Number, true, "Ex.: 10", 3),
                new("ForoCidadeUF", "Foro (Cidade/UF)", FieldType.Text, true, "Ex.: São Paulo/SP", 60)
            }
        );

        var freelancer = new ContractTemplate(
            id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            code: "freelancer",
            name: "Contrato Freelancer",
            version: 1,
            body: """
                  CONTRATO FREELANCER
                  
                  Cliente: {{ClienteNome}}, CPF/CNPJ {{ClienteDocumento}}
                  Freelancer: {{FreelancerNome}}, CPF/CNPJ {{FreelancerDocumento}}
                  
                  Data: {{DataHoje}}
                  
                  Escopo: {{Escopo}}
                  Entregáveis: {{Entregaveis}}
                  Pagamento: R$ {{Pagamento}}
                  
                  Observações: {{Observacoes}}
                  """,
            fields: new List<ContractFieldDefinition>
            {
                new("ClienteNome", "Nome do Cliente", FieldType.Text, true, "Ex.: Empresa X", 120),
                new("ClienteDocumento", "CPF/CNPJ do Cliente", FieldType.CpfCnpj, true, "Ex.: 00.000.000/0001-00", 20),
                new("FreelancerNome", "Nome do Freelancer", FieldType.Text, true, "Ex.: Fulano", 120),
                new("FreelancerDocumento", "CPF/CNPJ do Freelancer", FieldType.CpfCnpj, true, "Ex.: 000.000.000-00", 20),
                new("Escopo", "Escopo", FieldType.Text, true, "O que será feito", 800),
                new("Entregaveis", "Entregáveis", FieldType.Text, true, "Lista de entregas", 800),
                new("Pagamento", "Pagamento (R$)", FieldType.Money, true, "Ex.: 2500,00", 30),
                new("Observacoes", "Observações", FieldType.Text, false, "Opcional", 800)
            }
        );

        var locacao = new ContractTemplate(
            id: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            code: "locacao-residencial",
            name: "Locação Residencial (Aluguel)",
            version: 1,
            body: """
                  CONTRATO DE LOCAÇÃO RESIDENCIAL
                  
                  Pelo presente instrumento, as partes:
                  
                  LOCADOR(A): {{LocadorNome}}, CPF/CNPJ {{LocadorDocumento}}
                  LOCATÁRIO(A): {{LocatarioNome}}, CPF/CNPJ {{LocatarioDocumento}}
                  
                  têm entre si justo e contratado, na data de {{DataHoje}}, o que segue:
                  
                  1. IMÓVEL: {{ImovelEndereco}}
                  2. PRAZO: {{PrazoMeses}} meses, iniciando em {{DataInicio}}.
                  3. ALUGUEL: R$ {{ValorAluguel}} com vencimento todo dia {{DiaVencimento}}.
                  4. GARANTIA: {{GarantiaLocaticia}}.
                  5. REAJUSTE: {{IndiceReajuste}}.
                  6. MULTA POR ATRASO: {{MultaAtrasoPercentual}}%.
                  
                  E por estarem de acordo, assinam.
                  """,
            fields: new List<ContractFieldDefinition>
            {
                new("LocadorNome", "Nome do Locador", FieldType.Text, true, "Ex.: João da Silva", 120),
                new("LocadorDocumento", "CPF/CNPJ do Locador", FieldType.CpfCnpj, true, "Ex.: 000.000.000-00", 20),
                new("LocatarioNome", "Nome do Locatário", FieldType.Text, true, "Ex.: Maria Souza", 120),
                new("LocatarioDocumento", "CPF/CNPJ do Locatário", FieldType.CpfCnpj, true, "Ex.: 000.000.000-00", 20),
                new("ImovelEndereco", "Endereço do Imóvel", FieldType.Text, true, "Ex.: Rua X, 123, Bairro, Cidade/UF", 220),
                new("PrazoMeses", "Prazo (meses)", FieldType.Number, true, "Ex.: 12", 4),
                new("DataInicio", "Data de Início", FieldType.Date, true, "Ex.: 01/02/2026", 20),
                new("ValorAluguel", "Valor do Aluguel (R$)", FieldType.Money, true, "Ex.: 1800,00", 30),
                new("DiaVencimento", "Dia de vencimento", FieldType.Number, true, "Ex.: 10", 2),
                new("GarantiaLocaticia", "Garantia locatícia", FieldType.Text, true, "Ex.: Caução / Fiador / Seguro-fiança", 80),
                new("IndiceReajuste", "Índice de reajuste", FieldType.Text, true, "Ex.: IGP-M / IPCA", 40),
                new("MultaAtrasoPercentual", "Multa por atraso (%)", FieldType.Number, true, "Ex.: 10", 3)
            }
        );

        var compraVenda = new ContractTemplate(
            id: Guid.Parse("44444444-4444-4444-4444-444444444444"),
            code: "compra-e-venda",
            name: "Compra e Venda (Bem Móvel/Usado)",
            version: 1,
            body: """
                  CONTRATO DE COMPRA E VENDA
                  
                  VENDEDOR(A): {{VendedorNome}}, CPF/CNPJ {{VendedorDocumento}}
                  COMPRADOR(A): {{CompradorNome}}, CPF/CNPJ {{CompradorDocumento}}
                  
                  Na data de {{DataHoje}}, as partes ajustam a compra e venda do bem descrito abaixo:
                  
                  1. BEM: {{BemDescricao}}
                  2. ESTADO DO BEM: {{BemEstado}}
                  3. VALOR: R$ {{Valor}}
                  4. FORMA DE PAGAMENTO: {{FormaPagamento}}
                  5. ENTREGA: {{DataEntrega}} em {{LocalEntrega}}
                  6. OBSERVAÇÕES: {{Observacoes}}
                  
                  E por estarem de acordo, assinam.
                  """,
            fields: new List<ContractFieldDefinition>
            {
                new("VendedorNome", "Nome do Vendedor", FieldType.Text, true, "Ex.: João da Silva", 120),
                new("VendedorDocumento", "CPF/CNPJ do Vendedor", FieldType.CpfCnpj, true, "Ex.: 000.000.000-00", 20),
                new("CompradorNome", "Nome do Comprador", FieldType.Text, true, "Ex.: Maria Souza", 120),
                new("CompradorDocumento", "CPF/CNPJ do Comprador", FieldType.CpfCnpj, true, "Ex.: 000.000.000-00", 20),
                new("BemDescricao", "Descrição do Bem", FieldType.Text, true, "Ex.: Notebook Dell Inspiron, i5, 8GB RAM, SSD 256GB", 400),
                new("BemEstado", "Estado do Bem", FieldType.Text, true, "Ex.: Novo / Usado / Recondicionado", 40),
                new("Valor", "Valor (R$)", FieldType.Money, true, "Ex.: 2500,00", 30),
                new("FormaPagamento", "Forma de Pagamento", FieldType.Text, true, "Ex.: PIX à vista / 3x no cartão / entrada + parcelas", 120),
                new("DataEntrega", "Data de Entrega", FieldType.Date, true, "Ex.: 05/02/2026", 20),
                new("LocalEntrega", "Local de Entrega", FieldType.Text, true, "Ex.: Cidade/UF ou endereço", 160),
                new("Observacoes", "Observações", FieldType.Text, false, "Opcional", 500)
            }
        );

        var nda = new ContractTemplate(
            id: Guid.Parse("55555555-5555-5555-5555-555555555555"),
            code: "nda",
            name: "Termo de Confidencialidade (NDA)",
            version: 1,
            body: """
                  TERMO DE CONFIDENCIALIDADE (NDA)
                  
                  PARTES:
                  PARTE REVELADORA: {{ReveladoraNome}}, CPF/CNPJ {{ReveladoraDocumento}}, Email {{ReveladoraEmail}}
                  PARTE RECEPTORA: {{ReceptoraNome}}, CPF/CNPJ {{ReceptoraDocumento}}, Email {{ReceptoraEmail}}
                  
                  Data: {{DataHoje}}
                  
                  1. OBJETO
                  As partes firmam o presente termo para proteção de informações confidenciais relacionadas a: {{Objeto}}.
                  
                  2. INFORMAÇÕES CONFIDENCIAIS
                  Considera-se confidencial toda e qualquer informação compartilhada, incluindo documentos, dados, estratégias e materiais técnicos.
                  
                  3. OBRIGAÇÕES
                  A parte receptora se compromete a:
                  - não divulgar informações confidenciais a terceiros sem autorização;
                  - utilizar as informações apenas para a finalidade descrita;
                  - adotar medidas razoáveis de proteção.
                  
                  4. PRAZO DE SIGILO
                  O dever de confidencialidade vigorará por {{PrazoSigilo}}.
                  
                  5. EXCEÇÕES
                  Não se consideram confidenciais informações que: (i) já eram públicas; (ii) foram obtidas legitimamente de terceiros; (iii) foram desenvolvidas de forma independente.
                  
                  6. FORO
                  Fica eleito o foro da comarca de {{ForoCidadeUF}}.
                  
                  E por estarem de acordo, assinam.
                  """,
            fields: new List<ContractFieldDefinition>
            {
                new("ReveladoraNome", "Nome da Parte Reveladora", FieldType.Text, true, "Ex.: Empresa X", 120),
                new("ReveladoraDocumento", "CPF/CNPJ da Reveladora", FieldType.CpfCnpj, true, "Ex.: 00.000.000/0001-00", 20),
                new("ReveladoraEmail", "Email da Reveladora", FieldType.Email, true, "Ex.: contato@empresa.com", 120),
                new("ReceptoraNome", "Nome da Parte Receptora", FieldType.Text, true, "Ex.: Fulano", 120),
                new("ReceptoraDocumento", "CPF/CNPJ da Receptora", FieldType.CpfCnpj, true, "Ex.: 000.000.000-00", 20),
                new("ReceptoraEmail", "Email da Receptora", FieldType.Email, true, "Ex.: fulano@exemplo.com", 120),
                new("Objeto", "Objeto do NDA", FieldType.Text, true, "Ex.: negociação comercial / desenvolvimento de software", 300),
                new("PrazoSigilo", "Prazo de sigilo", FieldType.Text, true, "Ex.: 2 anos", 40),
                new("ForoCidadeUF", "Foro (Cidade/UF)", FieldType.Text, true, "Ex.: São Paulo/SP", 60)
            }
        );

        return new List<ContractTemplate> { prestacao, prestacaoV2, freelancer, locacao, compraVenda, nda };
    }
}


