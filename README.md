# 🧠 ManInDotnet – Missione Clean Architecture  
*Sessione presentata al DevFest Pescara 2025*

---

## 📘 Introduzione

Questo repository contiene il materiale della sessione **"ManInDotnet – Missione Clean Architecture"**, presentata al **DelFest Pescara 2025**.  
All’interno troverai codice di esempio, configurazioni e risorse utili per comprendere come applicare i principi della **Clean Architecture** in un progetto **.NET 8**, con il supporto di **Dapr** e **Podman** per la gestione dei microservizi e della containerizzazione.

L’obiettivo è mostrare un approccio pratico e modulare per la costruzione di applicazioni scalabili, manutenibili e indipendenti dalle infrastrutture sottostanti.

---

## ⚙️ Prerequisiti

Per eseguire correttamente la demo e i progetti inclusi nel repository, assicurati di avere installato e configurato i seguenti strumenti:

### 1. 🐳 Podman + Podman Desktop
Podman è un motore di container compatibile con Docker, mentre Podman Desktop fornisce un’interfaccia grafica intuitiva.

**Installazione:**
- Scarica e installa Podman e Podman Desktop dal sito ufficiale:  
  👉 [https://podman.io/getting-started/installation](https://podman.io/getting-started/installation)
- Dopo l’installazione, verifica che Podman sia attivo eseguendo:
  ```bash
  podman info
  ```

---

### 2. 🚀 Dapr (con Podman)
Dapr (Distributed Application Runtime) gestisce comunicazioni, eventi e configurazioni tra microservizi.

**Installazione:**
- Installa Dapr CLI seguendo la guida ufficiale:  
  👉 [https://docs.dapr.io/getting-started/install-dapr-cli/](https://docs.dapr.io/getting-started/install-dapr-cli/)
- Inizializza Dapr per funzionare con Podman:
  ```bash
  dapr init --runtime podman
  ```
- Verifica che i componenti Dapr siano attivi:
  ```bash
  dapr status
  ```

---

### 3. 💻 .NET 8 SDK
Il progetto è sviluppato con **.NET 8**.

**Installazione:**
- Scarica il SDK dal sito ufficiale:  
  👉 [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- Verifica l’installazione:
  ```bash
  dotnet --version
  ```
  Dovresti vedere un numero di versione che inizia con `8.`

---

## ▶️ Esecuzione della demo

Dopo aver installato i prerequisiti, puoi avviare la demo clonando il repository e lanciando i progetti .NET o i container Podman configurati.  
Ulteriori istruzioni specifiche per la demo saranno fornite nelle sottocartelle o nel materiale della sessione.

---

## 📄 Licenza

Distribuito sotto licenza **MIT**.  
Consulta il file `LICENSE` per i dettagli.

---

