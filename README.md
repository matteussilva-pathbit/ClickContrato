<<<<<<< HEAD
# ClickContrato

Gerador de contratos simples (MVP) em **C# / .NET 10**, com **DDD/Clean Architecture**, **API** e autenticação **JWT**.

> Neste início, tudo é **in-memory** (sem Postgres). A ideia é destravar o fluxo e evoluir para persistência depois.

## Como rodar

Pré-requisitos:
- .NET SDK **10.0.100** (o projeto inclui `global.json`)

Rodando a API:

```bash
dotnet run --project src/ClickContrato.Api
```

Abra o Swagger em `http://localhost:5100/swagger`.

## Arquitetura (DDD/Clean)

- `src/ClickContrato.Domain`: entidades/valores do domínio (ex.: `User`, `ContractTemplate`, `ContractDraft`)
- `src/ClickContrato.Application`: casos de uso/serviços (ex.: `AuthService`, `ContractService`) + DTOs/interfaces
- `src/ClickContrato.Infrastructure`: implementações (JWT, password hashing, repositórios em memória) + DI
- `src/ClickContrato.Api`: endpoints HTTP (Minimal APIs) + Swagger + Auth/Authorization

## JWT (config)

Em `src/ClickContrato.Api/appsettings.json`:

- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:SigningKey` (**trocar em produção**)
- `Jwt:AccessTokenMinutes`

## Endpoints (MVP)

### Auth
- `POST /auth/register`
- `POST /auth/login`

Resposta (exemplo):
- `accessToken`
- `expiresAtUtc`

### Contratos (protegid0s por JWT)
- `GET /contract-templates`
- `POST /contracts/drafts`
- `PUT /contracts/drafts/{draftId}/fields`
- `GET /contracts/drafts/{draftId}/preview`

## Fluxo rápido de teste (via Swagger)

1) `POST /auth/register` (pega o `accessToken`)
2) No Swagger, clique em **Authorize** e cole: `Bearer {token}`
3) `GET /contract-templates`
4) `POST /contracts/drafts` com `templateCode` (ex.: `"prestacao-servicos"` ou `"freelancer"`)
5) `PUT /contracts/drafts/{id}/fields` preenchendo os campos
6) `GET /contracts/drafts/{id}/preview`

## Próximos passos sugeridos

- Persistir no Postgres (repositórios EF Core)
- Geração de PDF (ex.: QuestPDF ou HTML->PDF)
- Pagamento (Stripe/Mercado Pago) e liberação do PDF final após confirmação
- Gestão de templates e cláusulas (Admin)


=======
# ClickContrato
>>>>>>> 650add669e061aaf950b80bcb6d42b0a105eef9a
