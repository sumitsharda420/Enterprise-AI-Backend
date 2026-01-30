# Enterprise AI Backend (.NET + OpenAI)

A production-ready AI backend built with ASP.NET Core (.NET 8) using OpenAI, featuring
session-based chat, enterprise decision engine, and document-based RAG (PDF/Word).
## Why this project exists

Most AI integrations fail in production because they lack:
- Control over AI output
- Cost and reliability safeguards
- Document-grounded answers
- Human approval workflows

This project demonstrates how to build **enterprise-grade AI systems**
instead of simple chatbots.
## Key Features

### AI Chat API
- Session-based chat memory
- Context window management
- Retry, timeout, and error handling
- Config-driven prompts and models

### AI Decision Engine
- Structured JSON responses
- Confidence scoring
- Human-in-the-loop readiness
- Risk-aware and policy-driven decisions

### Document-based RAG
- PDF and Word document ingestion
- Text chunking and embeddings
- Vector similarity search
- Answers grounded strictly in document content
- Source references for traceability

### Production Safeguards
- No secrets in code or config
- Environment variable-based API keys
- Cost-aware input limits
- Logging-ready architecture
- ## Tech Stack

- ASP.NET Core (.NET 8)
- OpenAI .NET SDK
- OpenAI Chat Completions
- OpenAI Embeddings
- In-memory Vector Store (pluggable)
- Swagger / REST APIs
- ## Architecture Overview

Controllers
→ AI Orchestration Layer
→ Memory / Decision / RAG Engines
→ OpenAI Services
→ Controlled, auditable outputs

The system is designed to evolve easily toward:
- Persistent vector databases
- Multi-document RAG
- Human approval workflows
- Cloud deployment
- ## API Endpoints

### Chat
POST `/api/chat`

### Decision Engine
POST `/api/decision/suggest`

### Document Upload (RAG)
POST `/api/documents/upload`

### Ask from Document
POST `/api/documents/{documentId}/ask`
## Running Locally

### 1. Clone the repository
```bash
git clone https://github.com/sumitsharda420/Enterprise-AI-Backend.git
Set OpenAI API Key (Environment Variable)
Windows

setx OPENAI_API_KEY "your-api-key"

Linux / macOS

export OPENAI_API_KEY="your-api-key"
3. Run the application
dotnet run


Swagger UI:

https://localhost:<port>/swagger
# 🧱 STEP 9 — SECURITY NOTE (VERY IMPORTANT)

```md
## Security Notes

- API keys are never stored in source code or configuration files
- All secrets are injected via environment variables
- This project follows production-safe secret management practices
## Intended Use Cases

- Internal AI assistants
- AI-assisted decision systems
- Chat with company documents
- Compliance-aware AI workflows
- Enterprise AI prototypes and pilots
## Author Notes

This project focuses on **AI system design**, not experimentation.
It demonstrates how to safely integrate OpenAI into real-world
enterprise backends using .NET.
